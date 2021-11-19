using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

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
        private double MinBattery(double distance,  WeightCategories weight = 0)
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
           double distanceFromDroneToStation= CalculateDistance( closetBaseStation.SLocation,drone.CurrLocation);
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
            dal.
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
        void BelongingParcel(int dId)
        {
            //if(!dal.ExistsInDroneList(dId))
            //{
            //    throw new Exception("this drone doesnt exist in drone list!");
            //}
            try
            {
                IDal.DO.Drone drone = dal.GetDrone(dId);
                DroneToList droneToList = lDroneToList.Find(drone => drone.Id == dId);
                if (droneToList.DStatus != IBL.BO.DroneStatus.Free)
                {
                    if (droneToList.DStatus == IBL.BO.DroneStatus.Maintenance) throw new Exception("this drone in maintance state and cant be belonging to a parcel!");
                    else throw new Exception("this drone in delivery state and cant be belonging to a parcel!!");
                }
               // IEnumerable<IBL.BO.Customer> customerList = GetBList<IBL.BO.Customer, IDal.DO.Customer>(MapCustomer);
                IEnumerable<IBL.BO.Parcel> ParcelList = GetBList<IBL.BO.Parcel, IDal.DO.Parcel>(MapParcel)
                    .Where(parcel => parcel.Weight <= drone.MaxWeight);
                var conditionParcel = ParcelList.OrderByDescending(p => p.MPriority)
                      .ThenByDescending(p => p.Weight)
                .ThenBy(p => CalculateDistance(dal.GetFromDalById<IDal.DO.Customer>(p.SenderId).mapToBl(), droneToList.CurrLocation));
                IBL.BO.Parcel resultParcel= conditionParcel.FirstOrDefault();
                double way = CalculateDistance(droneToList.CurrLocation, resultParcel.SenderId);
                if ()
                {

                }
            }
            catch (DAL.IdNotExistInAListException)
            {
                throw new BelongingParcelException();
            }
        }

        private IBL.BO.Customer MapCustomer(IDal.DO.Customer input)
        {
            throw new NotImplementedException();
        }

            IEnumerable<IDal.DO.Parcel> ParcelList = Extensions.GetListFromDal<IDal.DO.Parcel>().Where(parcel=>parcel.Weight<=drone.Weight);
            ParcelList.OrderBy(p => p.).ThenBy(p => p.Weight).t


        private IBL.BO.Parcel MapParcel(IDal.DO.Parcel input)
        {
            throw new NotImplementedException();
        }
    }
}
