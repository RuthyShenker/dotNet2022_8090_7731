﻿using IBL.BO;
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
            IDal.DO.Parcel parcel = new IDal.DO.Parcel(
               newParcel.Sender.Id,
               newParcel.Getter.Id,
           (IDal.DO.WeightCategories)newParcel.Weight,
               (IDal.DO.UrgencyStatuses)newParcel.MPriority,
              DateTime.Now,
              new DateTime(),
             new DateTime(),
              new DateTime());
            dal.AddingItemToDList(parcel);
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
            FindDroneInList(dId);
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
                throw new InValidActionException("the drone had already picked up all the parcels ");
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
            FindDroneInList(dId);
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
                    dal.ProvidingPackage(parcel.Id);
                    deliveryed = true;
                }
            }
            if (!deliveryed)
            {
                throw new InValidActionException("parcels status doesnt match");
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
            ParcelToList nParcel = new(
            parcel.Id,
            dal.GetFromDalById<IDal.DO.Customer>(parcel.SenderId).Name,
            dal.GetFromDalById<IDal.DO.Customer>(parcel.GetterId).Name,
            (IBL.BO.WeightCategories)parcel.Weight,
            (Priority)parcel.MPriority,
            GetParcelStatus(parcel)
            );
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
                dInParcel, parcel.MakingParcel, parcel.BelongParcel,
                parcel.PickingUp, parcel.Arrival);
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


