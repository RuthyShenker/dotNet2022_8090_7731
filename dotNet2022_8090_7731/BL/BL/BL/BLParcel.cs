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
            IDAL.DO.Customer sender;
            IDAL.DO.Customer getter;
            try
            {
                sender = dal.GetFromDalById<IDAL.DO.Customer>(newParcel.Sender.Id);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDAL.DO.Customer), newParcel.Sender.Id);
            }

            try
            {
                getter = dal.GetFromDalById<IDAL.DO.Customer>(newParcel.Getter.Id);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDAL.DO.Customer), newParcel.Getter.Id);
            }

            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel(
                newParcel.Sender.Id,
                newParcel.Getter.Id,
                (IDAL.DO.WeightCategories)newParcel.Weight,
                (IDAL.DO.UrgencyStatuses)newParcel.MPriority,
                DateTime.Now,
                new DateTime(),
                new DateTime(),
                new DateTime());
            dal.Add(parcel);
            return parcel.Id;
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
                throw new InValidActionException(typeof(IDAL.DO.Drone), dId, $"status of drone is {drone.DStatus} ");
            }
            if (!drone.DeliveredParcelId.HasValue)
            {
                throw new InValidActionException(typeof(IDAL.DO.Drone), dId, $"there is no assined parcel ");
            }

            IDAL.DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<IDAL.DO.Parcel>(drone.DeliveredParcelId.Value);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), drone.DeliveredParcelId.Value);
            }

            if (parcel.PickingUp != null)
            {
                throw new InValidActionException("Parcel assigned to drone was already picked up");
            }

            Location senderLocation = GetCustomer(parcel.SenderId).Location;
            drone.BatteryStatus -= MinBattery(Extensions.CalculateDistance(drone.CurrLocation, senderLocation));
            drone.CurrLocation = senderLocation;
            // לא היה כתוב לשנות status
            drone.DStatus = DroneStatus.Delivery;
            dal.Update<IDAL.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.PickingUp));
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

            IDAL.DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<IDAL.DO.Parcel>(drone.DeliveredParcelId.Value);
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

            Location getterLocation = GetCustomer(parcel.GetterId).Location;
            drone.BatteryStatus -= MinBattery(Extensions.CalculateDistance(drone.CurrLocation, getterLocation));
            drone.CurrLocation = getterLocation;
            drone.DStatus = DroneStatus.Free;
            dal.Update<IDAL.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.Arrival));
        }

        /// <summary>
        /// A function that gets IDAL.DO.Parcel object and returns its status
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelStatus of specific parcel </returns>
        private ParcelStatus GetParcelStatus(IDAL.DO.Parcel parcel)
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
            return dal.GetListFromDal<IDAL.DO.Parcel>().Select(s => ConvertToList(s));
        }

        /// <summary>
        /// A function that returns a list of unbelong parcels.
        /// </summary>
        /// <returns>returns a list of unbelong parcels.</returns>
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            try
            {
                return dal.GetDalListByCondition<IDAL.DO.Parcel>(parcel => parcel.DroneId == 0)
                    .Select(parcel => ConvertToList(parcel));
            }

            catch (DalObject.InValidActionException)
            {
                throw new InValidActionException("There is no match object in the list ");
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Parcel
        /// and Expands it to a new object of 
        /// ParcelToList and returns it.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelToList object</returns>
        private ParcelToList ConvertToList(IDAL.DO.Parcel parcel)
        {
            string senderName = dal.GetFromDalById<IDAL.DO.Customer>(parcel.SenderId).Name;
            string getterName = dal.GetFromDalById<IDAL.DO.Customer>(parcel.GetterId).Name;

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
            try
            {
                var dParcel = dal.GetFromDalById<IDAL.DO.Parcel>(parcelId);
                return ConvertToBL(dParcel);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), parcelId);
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Parcel
        /// and Expands it to a new object of 
        /// Parcel and returns it.
        /// <param name="parcel"></param>
        /// <returns>returns Parcel object</returns>
        private Parcel ConvertToBL(IDAL.DO.Parcel parcel)
        {
            CustomerInParcel sender = NewCustomerInParcel(parcel.SenderId);
            CustomerInParcel getter = NewCustomerInParcel(parcel.GetterId);
            DroneInParcel dInParcel = NewDroneInParcel(parcel.DroneId);
            return new Parcel(parcel.Id, sender, getter,
                (WeightCategories)parcel.Weight, (Priority)parcel.MPriority,
                dInParcel, parcel.CreatedTime, parcel.BelongParcel,
                parcel.PickingUp, parcel.Arrival);
        }

        /// <summary>
        /// A function that gets id of drone and bulids from it an object of 
        /// DroneInParcel and of course considering logic and returns 
        /// the new object of DroneInParcel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>returns 
        /// the new object of DroneInParcel</returns>
        private DroneInParcel NewDroneInParcel(int Id)
        {
            if (lDroneToList.FirstOrDefault(d => d.Id == Id) != null)
            {
                var drone = lDroneToList.FirstOrDefault(drone => drone.Id == Id);
                return new DroneInParcel(Id, drone.BatteryStatus, drone.CurrLocation);
            }
            else
            {
                return new DroneInParcel(Id,0,null);
            }

        }
    }
}


