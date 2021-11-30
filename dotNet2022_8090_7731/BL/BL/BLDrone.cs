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
        ///  
        /// A function that gets an object of IDal.DO.Drone
        /// and Expands it to Drone object and returns this object.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>returns Drone object </returns>
        private Drone ConvertToBL(IDal.DO.Drone drone)
        {
            ParcelInTransfer parcelInTransfer = CalculateParcelInTransfer(drone.Id);
            var wantedDrone = lDroneToList.FirstOrDefault(droneToList => droneToList.Id == drone.Id);
            return new Drone(wantedDrone.Id, wantedDrone.Model, wantedDrone.Weight, wantedDrone.BatteryStatus,
                wantedDrone.DStatus, parcelInTransfer, wantedDrone.CurrLocation);
        }

        /// <summary>
        /// A function that creates ParcelInTransferand
        /// Calculates bills for specific drone id 
        /// and returns this ParcelInTransfer.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>returns ParcelInTransfer object.</returns>
        private ParcelInTransfer CalculateParcelInTransfer(int droneId)
        {
            var parcelsDalList = dal.GetListFromDal<IDal.DO.Parcel>();
            var belongedParcel = parcelsDalList.FirstOrDefault(parcel => parcel.DroneId == droneId);
            if (belongedParcel.Equals(default(IDal.DO.Parcel)))
            {
                return new ParcelInTransfer();
            }
            else
            {
                var parcel = ConvertToBL(belongedParcel);
                Location senderLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.Sender.Id).CLocation;
                Location getterLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.Getter.Id).CLocation;
                double distance = CalculateDistance(senderLocation, getterLocation);
                return new ParcelInTransfer(parcel.Id, parcel.PickingUp.HasValue, parcel.MPriority,
                    parcel.Weight, parcel.Sender, parcel.Getter, senderLocation, getterLocation, distance);
            }
        }

        /// <summary>
        /// A function that gets an id of drone and sending it to charging,the 
        /// function doesn't return anything.
        /// </summary>
        /// <param name="IdDrone"></param>
        public void SendingDroneToCharge(int IdDrone)
        {
            try
            {
                DroneToList drone = FindDroneInList(IdDrone); 
                switch (drone.DStatus)
                {
                    case DroneStatus.Maintenance:
                        throw new InValidActionException(typeof(Drone), IdDrone, $"status of drone is Maintenance ");
                    case DroneStatus.Delivery:
                        throw new InValidActionException(typeof(Drone), IdDrone, $"status of drone is Delivery ");
                    default:
                        break;
                }
                Station closetdStation = ClosestStation(drone.CurrLocation);
                if (closetdStation.NumAvailablePositions == 0)
                {
                    throw new InValidActionException("The closet Station doesnt have available positions!");
                }
                double distanceFromDroneToStation = CalculateDistance(closetdStation.SLocation, drone.CurrLocation);
                double minBattery = MinBattery(distanceFromDroneToStation, drone.Weight);
                if (drone.BatteryStatus - minBattery < 0)
                {
                    throw new InValidActionException("There isnt enough battery to the drone in order to go to the closet station to be charging");
                }
                drone.BatteryStatus = minBattery;
                drone.CurrLocation = closetdStation.SLocation;
                drone.DStatus = IBL.BO.DroneStatus.Maintenance;
                //--closetBaseStation.NumAvailablePositions;
                //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
                dal.AddingDroneToCharge(drone.Id, closetdStation.Id);
            }
            catch (ArgumentNullException)
            {
                throw new ListIsEmptyException(typeof(Drone));
            }
        }

        /// <summary>
        /// A function that gets an id of drone and releasing it from charging.
        /// </summary>
        /// <param name="dId"></param>
        /// <param name="timeInCharging"></param>
        public void ReleasingDrone(int dId, double timeInCharging)
        {
            DroneToList drone = FindDroneInList(dId);

            switch (drone.DStatus)
            {
                case DroneStatus.Free:
                    throw new InValidActionException(typeof(Drone), dId, $"status of drone is Free ");
                case DroneStatus.Delivery:
                    throw new InValidActionException(typeof(Drone), dId, $"status of drone is Delivery ");
                default:
                    drone.DStatus = DroneStatus.Free;
                    drone.BatteryStatus += timeInCharging * chargingRate;
                    dal.ReleasingDrone(drone.Id);
                    break;
            }
        }

        private DroneToList FindDroneInList(int dId)
        {
            try
            {
                return lDroneToList.First(drone => drone.Id == dId);
            }
            catch (ArgumentNullException)
            {
                throw new ListIsEmptyException(typeof(Drone));
            }
            catch (InvalidOperationException)
            {
                throw new IdIsNotExistException(typeof(Drone), dId);
            }
        }

        /// <summary>
        /// A function that gets an id of drone and belonging to it a parcel.
        /// </summary>
        /// <param name="dId"></param>
        public void BelongingParcel(int dId)
        {
            if (!dal.IsIdExistInList<IDal.DO.Drone>(dId))
            {
                throw new IdIsAlreadyExistException(typeof(Drone), dId);
            }
            DroneToList droneToList = lDroneToList.Find(drone => drone.Id == dId);
            if (droneToList.DStatus != DroneStatus.Free)
            {
                string dStatus = droneToList.DStatus.ToString();
                throw new InValidActionException(typeof(Drone), dId, $"status of drone is {dStatus} ");
            }
            var optionParcels = dal.GetDalListByCondition<IDal.DO.Parcel>(parcel => parcel.Weight <= (IDal.DO.WeightCategories)droneToList.Weight)
                .OrderByDescending(parcel => parcel.MPriority).ThenByDescending(parcel => parcel.Weight).ThenBy(parcel => GetDistance(droneToList.CurrLocation, parcel));
            bool belonged = false;
            foreach (var parcel in optionParcels)
            {
                if (droneToList.BatteryStatus >= MinBattery(GetDistance(droneToList.CurrLocation, parcel), (IBL.BO.WeightCategories)parcel.Weight))
                {
                    droneToList.DStatus = DroneStatus.Delivery;
                    dal.UpdateBelongedParcel(parcel, droneToList.Id);
                    belonged = true;
                    break;
                }
            }
            if (!belonged)
            {
                throw new InValidActionException("There is no match parcel to belong the drone");
            }
        }

        /// <summary>
        /// A function that calculates the distance that the drone has to pass in order to give 
        /// a parcel to the destination and go the the closet base station in order to
        /// go charging .
        /// </summary>
        /// <param name="droneLocation"></param>
        /// <param name="parcel"></param>
        /// <returns>the function returns this distance</returns>
        private double GetDistance(Location droneLocation, IDal.DO.Parcel parcel)
        {
            Location senderLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.SenderId).CLocation;
            Location getterLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.SenderId).CLocation;
            return CalculateDistance(droneLocation, senderLocation, getterLocation, ClosestStation(getterLocation).SLocation);
        }
        /// <summary>
        /// A function that gets Drone and station id and adds this Drone to the data base and sending
        /// this drone to charging in the StationId that the function gets,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bLDrone"></param>
        /// <param name="StationId"></param>
        public void AddingDrone(Drone bLDrone, int StationId)
        {
            if (dal.IsIdExistInList<IDal.DO.Drone>(bLDrone.Id))
            {
                throw new DalObject.IdIsAlreadyExistException(typeof(IDal.DO.Drone), bLDrone.Id);
            }
            if (!dal.IsIdExistInList<IDal.DO.BaseStation>(StationId))
            {
                throw new DalObject.IdIsNotExistException(typeof(IDal.DO.BaseStation), StationId);
            }
            if (!dal.AreThereFreePositions(StationId))
            {
                throw new DalObject.InValidActionException(typeof(IDal.DO.BaseStation), StationId, "There aren't free positions ");
            }
            bLDrone.BatteryStatus = DalObject.DataSource.Rand.Next(20, 41);
            bLDrone.DroneStatus = IBL.BO.DroneStatus.Maintenance;
            dal.AddingDroneToCharge(bLDrone.Id, StationId);
            IDal.DO.BaseStation station = dal.GetFromDalById<IDal.DO.BaseStation>(StationId);
            DroneToList droneToList = new DroneToList(
                bLDrone.Id,
                bLDrone.Model,
                bLDrone.Weight,
                bLDrone.BatteryStatus,
                bLDrone.DroneStatus,
                new Location(station.Longitude, station.Latitude),
                null);
            lDroneToList.Add(droneToList);
            IDal.DO.Drone drone = new IDal.DO.Drone()
            {
                Id = bLDrone.Id,
                MaxWeight = (IDal.DO.WeightCategories)bLDrone.Weight,
                Model = bLDrone.Model
            };
            dal.AddingItemToDList(drone);
        }

        /// <summary>
        /// A function that gets droneId and newModel and updates the drone with the id of 
        /// droneId to be with the model of newModel, the function doesn't return anything.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newModel"></param>
        public void UpdatingDroneName(int droneId, string newModel)
        {
            DroneToList droneToList = FindDroneInList(droneId);
            droneToList.Model = newModel;
            IDal.DO.Drone dalDrone = new()
            {
                Id = droneToList.Id,
                Model = droneToList.Model,
                MaxWeight = (IDal.DO.WeightCategories)droneToList.Weight
            };
            dal.UpdateDrone(droneId, dalDrone);
        }
    }
}

