using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    partial class BL
    {

        /* //יש גנרי
         public IEnumerable<ParcelToList> GetParcels()
         {
             IEnumerable<ParcelToList> bParcelList = new List<ParcelToList>();
             List<IDal.DO.Parcel> dParcelList = GetList<IDal.DO.Parcel>();
             //IEnumerable<IDal.DO.Parcel> dParcelList = dal.GetParcels();
             foreach (var parcel in dParcelList)
             {
                 bParcelList.Add(MapToList(parcel));
             }
             return bParcelList;
         }*/


        /// <summary>
        /// A function that gets a parcel and adds it to the data base,the function
        /// doesn't return anything.
        /// </summary>
        /// <param name="newParcel"></param>
        public void AddingParcel(Parcel newParcel)
        {
            IDal.DO.Parcel parcel = new()
            {
                SenderId = newParcel.Sender.Id,
                GetterId = newParcel.Getter.Id,
                Weight = (IDal.DO.WeightCategories)newParcel.Weight,
                MPriority = (IDal.DO.UrgencyStatuses)newParcel.MPriority,
                DroneId = 0,
                MakingParcel = DateTime.Now,
                BelongParcel = null,
                PickingUp = null,
                Arrival = null
            };

            dal.AddingParcel(parcel);
        }
        /// <summary>
        /// A function that gets an id of drone and
        /// causes the drone to pick up the parcel that 
        /// it needs to take to the destination,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        public void PickingUpParcel(int dId)
        {
            try
            {
                lDroneToList.Exists(drone => drone.Id == dId);
            }
            catch (ArgumentNullException)
            {
                throw; // ID isnt exist
            }
            try
            {
                var parcels = dal.GetDalListByCondition<IDal.DO.Parcel>(parcel => parcel.DroneId == dId);
                bool pickedUp = false;
                foreach (var parcel in parcels)
                {
                    if (parcel.PickingUp == null)
                    {
                        var drone = lDroneToList.Find(drone => drone.Id == dId);
                        Location senderLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.SenderId).CLocation;
                        drone.BatteryStatus -= MinBattery(CalculateDistance(drone.CurrLocation, senderLocation));
                        drone.CurrLocation = senderLocation;
                        // לא היה כתוב לשנות status
                        drone.DStatus = DroneStatus.Delivery;
                        dal.PickingUpParcel(parcel.Id);
                        pickedUp = true;
                    }
                }
                if (!pickedUp)
                {
                    throw new Exception(); // חבילות כבר נאספו
                }
            }
            catch (ArgumentNullException)
            {
                throw;// שום חבילה לא משויכת לרחפן זה
            }
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
            bool deliveryed = false;
            try
            {
                lDroneToList.Exists(drone => drone.Id == dId);
                var drone = lDroneToList.Find(drone => drone.Id == dId);

                var parcels = dal.GetDalListByCondition<IDal.DO.Parcel>(parcel => parcel.DroneId == dId);

                foreach (var parcel in parcels)
                {
                    if (parcel.PickingUp != null && parcel.Arrival == null)
                    {
                        Location getterLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.GetterId).CLocation;
                        drone.BatteryStatus -= MinBattery(CalculateDistance(drone.CurrLocation, getterLocation));
                        drone.CurrLocation = getterLocation;
                        drone.DStatus = DroneStatus.Free;
                        dal.ProvidePackage(parcel.Id);
                        deliveryed = true;
                    }
                }
                if (!deliveryed)
                {
                    throw new Exception();// parcels status isnnt match
                }
            }
            catch (ArgumentNullException)
            {
                throw; // ID isnt exist
            }

        }


        /// <summary>
        /// A function that returns a list of unbelong parcels.
        /// </summary>
        /// <returns>returns a list of unbelong parcels.</returns>
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            var unbelongParcelsList = dal.GetUnbelongParcels();
            var bParcelList = new List<ParcelToList>();
            foreach (var parcel in unbelongParcelsList)
            {
                bParcelList.Add(ConvertToList(parcel));
            }
            return bParcelList;
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
            ParcelToList nParcel = new ParcelToList();
            nParcel.Id = parcel.Id;
            nParcel.SenderName = dal.GetFromDalById<IDal.DO.Customer>(parcel.SenderId).Name;
            nParcel.GetterName = dal.GetFromDalById<IDal.DO.Customer>(parcel.GetterId).Name; /*Extensions.GetById<Customer>(parcel.GetterId).Name;*/
            //nParcel.SenderName = customerDalList.First(customer => customer.Id == parcel.SenderId).Name;
            //nParcel.GetterName = customerDalList.First(customer => customer.Id == parcel.GetterId).Name;
            nParcel.Weight = (IBL.BO.WeightCategories)parcel.Weight;
            nParcel.MyPriority = (Priority)parcel.MPriority;
            nParcel.Status = GetParcelStatus(parcel);
            return nParcel;
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
                dInParcel, parcel.MakingParcel,parcel.BelongParcel,
                parcel.PickingUp, parcel.Arrival );
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

    }
}


