using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// A function that gets a parcel and adds it to the data base,the function
        /// doesn't return anything.
        /// </summary>
        /// <param name="newParcel"></param>
        public int AddingParcel(Parcel newParcel)
        {
            IDal.DO.Customer sender = default(IDal.DO.Customer);
            IDal.DO.Customer getter = default(IDal.DO.Customer);

            try
            {
                sender = dal.GetFromDalById<IDal.DO.Customer>(newParcel.Sender.Id);
                getter = dal.GetFromDalById<IDal.DO.Customer>(newParcel.Getter.Id);
                IDal.DO.Parcel parcel = new IDal.DO.Parcel(
                    newParcel.Sender.Id,
                    newParcel.Getter.Id,
                    (IDal.DO.WeightCategories)newParcel.Weight,
                    (IDal.DO.UrgencyStatuses)newParcel.MPriority,
                    DateTime.Now,
                    new DateTime(),
                    new DateTime(),
                    new DateTime());
                dal.AddingToData(parcel);
                return parcel.Id;
            }
            catch (DalObject.IdIsNotExistException)
            {
                if (sender.Equals(default(IDal.DO.Customer)))
                {
                    throw new IdIsNotExistException(typeof(IDal.DO.Customer), newParcel.Sender.Id); ;
                }
                throw new IdIsNotExistException(typeof(IDal.DO.Customer), newParcel.Getter.Id); ;
            }
        }

        /// <summary>
        /// A function that gets an id of drone and
        /// causes the drone to pick up the parcel that 
        /// it needs to take to the destination,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        public void PickingUpParcel(int dId)
        {
            var drone = FindDroneInList(dId);
            
            if (drone.DStatus != DroneStatus.Delivery)
            {
                throw new InValidActionException(typeof(IDal.DO.Drone), dId, $"status of drone is {drone.DStatus} ");
            }
            if (!drone.DeliveredParcelId.HasValue)
            {
                throw new InValidActionException(typeof(IDal.DO.Drone), dId, $"there is no assined parcel ");
            }

            IDal.DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<IDal.DO.Parcel>(drone.DeliveredParcelId.Value);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), drone.DeliveredParcelId.Value);
            }

            if (parcel.PickingUp != null)
            {
                throw new InValidActionException("Parcel assigned to drone was already picked up");
            }

            Location senderLocation = GetCustomer(parcel.SenderId).CLocation;
            drone.BatteryStatus -= MinBattery(CalculateDistance(drone.CurrLocation, senderLocation));
            drone.CurrLocation = senderLocation;
            // לא היה כתוב לשנות status
            drone.DStatus = DroneStatus.Delivery;
            dal.UpdatingInData<IDal.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.PickingUp));
        }

        /// <summary>
        /// A function that gets an id of drone
        /// and causes this drone to Delivery the Package
        /// that it needs to take 
        /// to the destination,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        public void DeliveryPackage(int dId)
        {
            var drone = FindDroneInList(dId);

            if (drone.DeliveredParcelId == null)
            {
                throw new InValidActionException(typeof(Drone), dId, $"There is no assigned parcel ");
            }
            if (drone.DStatus != DroneStatus.Delivery)
            {
                throw new InValidActionException(typeof(Drone), dId, $"status of drone is {drone.DStatus} ");
            }

            IDal.DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<IDal.DO.Parcel>(drone.DeliveredParcelId.Value);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), drone.DeliveredParcelId.Value);
            }

            if (parcel.PickingUp == null)
            {
                throw new InValidActionException("Parcel assigned to drone had not been picked up yet");
            }
            if (parcel.Arrival != null)
            {
                throw new InValidActionException("Parcel assigned has already supplied to destination");
            }

            Location getterLocation = GetCustomer(parcel.GetterId).CLocation;
            drone.BatteryStatus -= MinBattery(CalculateDistance(drone.CurrLocation, getterLocation));
            drone.CurrLocation = getterLocation;
            drone.DStatus = DroneStatus.Free;
            dal.UpdatingInData<IDal.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.Arrival));
        }

        /// <summary>
        /// A function that gets IDal.DO.Parcel object and returns its status
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelStatus of specific parcel </returns>
        private ParcelStatus GetParcelStatus(IDal.DO.Parcel parcel)
        {
            if (parcel.Arrival.HasValue)
            {
                return ParcelStatus.InDestination;
            }
            if (parcel.PickingUp.HasValue)
            {
                return ParcelStatus.collected;
            }
            if (parcel.BelongParcel.HasValue)
            {
                return ParcelStatus.belonged;
            }
            return ParcelStatus.made;
        }

        //------Get-------------------------------------------------------

        public IEnumerable<ParcelToList> GetParcels()
        {
            var dList = dal.GetListFromDal<IDal.DO.Parcel>();
            var bList = new List<ParcelToList>();
            foreach (dynamic parcel in bList)
            {
                bList.Add(ConvertToList(parcel));
            }
            return bList;
        }

        /// <summary>
        /// A function that returns a list of unbelong parcels.
        /// </summary>
        /// <returns>returns a list of unbelong parcels.</returns>
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            try
            {
                //var unbelongParcelsList = dal.GetUnbelongParcels();
                var unbelongParcelsList = dal.GetDalListByCondition<IDal.DO.Parcel>(parcel => parcel.DroneId == 0);
                var bParcelList = new List<ParcelToList>();
                foreach (var parcel in unbelongParcelsList)
                {
                    bParcelList.Add(ConvertToList(parcel));
                }
                return bParcelList;
            }

            catch (DalObject.InValidActionException)
            {
                throw new InValidActionException("There is no match object in the list ");
            }
        }

        /// <summary>
        /// A function that gets an object of IDal.DO.Parcel
        /// and Expands it to a new object of 
        /// ParcelToList and returns it.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelToList object</returns>
        private ParcelToList ConvertToList(IDal.DO.Parcel parcel)
        {
            string senderName = dal.GetFromDalById<IDal.DO.Customer>(parcel.SenderId).Name;
            string getterName = dal.GetFromDalById<IDal.DO.Customer>(parcel.GetterId).Name;

            ParcelToList nParcel = new(
            parcel.Id,
            senderName,
            getterName,
            (WeightCategories)parcel.Weight,
            (Priority)parcel.MPriority,
            GetParcelStatus(parcel)
            );
            return nParcel;
        }

        public Parcel GetParcel(int parcelId)
        {
            var dParcel = dal.GetFromDalById<IDal.DO.Parcel>(parcelId);
            return ConvertToBL(dParcel);
        }


        /// <summary>
        /// A function that gets an object of IDal.DO.Parcel
        /// and Expands it to a new object of 
        /// Parcel and returns it.
        /// <param name="parcel"></param>
        /// <returns>returns Parcel object</returns>
        private Parcel ConvertToBL(IDal.DO.Parcel parcel)
        {
            CustomerInParcel sender = NewCustomerInParcel(parcel.SenderId);
            CustomerInParcel getter = NewCustomerInParcel(parcel.GetterId);
            DroneInParcel dInParcel = NewDroneInParcel(parcel.DroneId);
            return new Parcel(parcel.Id, sender, getter,
                (WeightCategories)parcel.Weight, (Priority)parcel.MPriority,
                dInParcel, parcel.MakingParcel, parcel.BelongParcel,
                parcel.PickingUp, parcel.Arrival);
        }
    }
}


