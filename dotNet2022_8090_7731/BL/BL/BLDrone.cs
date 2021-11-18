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
        IEnumerable<DroneToList> GetDrones()
        {
            return lDroneToList;
        }

        private DroneToList copyCommon(IDal.DO.Drone source)
        {
            DroneToList nDroneToList = new DroneToList();
            nDroneToList.Id = source.Id;
            nDroneToList.Model = source.Model;
            nDroneToList.Weight = source.MaxWeight;
            return nDroneToList;
        }

        // weight=0 ערך ברירת מחדל לפונקציה
        private double  MinBattery(double distance,  WeightCategories weight = 0)
        {
            switch (weight)
            {
                case WeightCategories.Light:
                    return pConsumLight * distance;
                case WeightCategories.Medium:
                    return pConsumMedium * distance;
                case WeightCategories.Heavy:
                    return pConsumHeavy * distance;
                default:
                    return pConsumFree * distance;
            }
        }
        void SendingDroneToCharge(int IdDrone)
        {
            if(!dal.ExistsInDroneList(IdDrone))
            {
                throw new Exception("the id of this drone doesnt exist");
            }
            DroneToList drone=lDroneToList.Find(drone => drone.Id == IdDrone);
            if (drone.DStatus!=DroneStatus.Free)
            {
                if (drone.DStatus == DroneStatus.Maintenance)
                    throw new Exception("this drone in maintenance,it cant go to charge");
                throw new Exception("this drone in delivery ,it cant go to charge");
            }
            Station closetBaseStation=closestStation(drone.CurrLocation);
            if (closetBaseStation.NumAvailablePositions==0)
            {
                throw new Exception("The closet Station doesnt have available positions!");
            }
           double distanceFromDroneToStation= calDistance( closetBaseStation.SLocation,drone.CurrLocation);
           double minBattery = MinBattery(distanceFromDroneToStation, drone.Weight);
            if (drone.BatteryStatus-minBattery<0)
            {
                throw new Exception("There isnt enough battery to the drone in order to go to the closet station to be charging");
            }
            
            drone.BatteryStatus = minBattery;
            drone.CurrLocation = closetBaseStation.SLocation;
            drone.DStatus = DroneStatus.Maintenance;

            //--closetBaseStation.NumAvailablePositions;
            //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
            dal.AddDroneToCharge(drone.Id,closetBaseStation.Id);

        }
        void ReleasingDrone(int dId, double timeInCharging)
        {
            if(!dal.ExistsInDroneList(dId))
            {
                throw new Exception("this id doesnt exist in the drone list!");
            }
            DroneToList drone=lDroneToList.Find(drone => drone.Id == dId);
            if(drone.DStatus!=DroneStatus.Maintenance)
            {
                if (drone.DStatus == DroneStatus.Free) throw new Exception("this drone in free state, it cant relese from charging!");
                else throw new Exception("this drone in delivery state,it cant relese from charging!");
            }
            drone.DStatus = DroneStatus.Free;
            drone.BatteryStatus += timeInCharging * chargingRate;
            dal.ReleasingDrone(drone.Id);
        }
        void BelongingParcel(int pId)
        {

        }
    }
}
