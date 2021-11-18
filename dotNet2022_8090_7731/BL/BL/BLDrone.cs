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
        private double MinButtery(double distance,  WeightCategories weight = 0)
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
        void ChargingDrone(int IdDrone)
        {
            if(lDroneToList.Exist(IdDrone))
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
           MinButtery(distanceFromDroneToStation, drone.Weight);
        }
       

    }
}
