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
        /// <summary>
        /// ?????????????
        /// </summary>
        /// <returns></returns>
        IEnumerable<DroneToList> GetDrones()
        {
            return lDroneToList;
        }

        /// <summary>
        ///  A function that map from IDal.DO.Drone to DroneToList only the common fields.
        /// </summary>
        /// 
        private DroneToList copyCommon(IDal.DO.Drone source)
        {
            DroneToList nDroneToList = new DroneToList();
            nDroneToList.Id = source.Id;
            nDroneToList.Model = source.Model;
            nDroneToList.Weight = source.MaxWeight;
            return nDroneToList;
        }

        /// <summary>
        /// A function that gets weight of drone
        /// and distance and returns the minimum battery that 
        /// the drone needs in order to flight.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>the minimum battery in double</returns>
        // weight=0 ערך ברירת מחדל לפונקציה
        private double MinBattery(double distance,  IBL.BO.WeightCategories weight = 0)
        {
            switch (weight)
            {
                case IBL.BO.WeightCategories.Light:
                    return pConsumLight * distance;
                case IBL.BO.WeightCategories.Medium:
                    return pConsumMedium * distance;
                case IBL.BO.WeightCategories.Heavy:
                    return pConsumHeavy * distance;
                default:
                    return pConsumFree * distance;
            }
        }

        /// <summary>
        /// A function that gets an id od drone and sending it to charging.
        /// </summary>
        /// <param name="IdDrone"></param>
        void SendingDroneToCharge(int IdDrone)
        {
            if(!dal.ExistsInDroneList(IdDrone))
            {
                throw new Exception("the id of this drone doesnt exist");
            }
            DroneToList drone=lDroneToList.Find(drone => drone.Id == IdDrone);
            if (drone.DStatus!=IBL.BO.DroneStatus.Free)
            {
                if (drone.DStatus == IBL.BO.DroneStatus.Maintenance)
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
            drone.DStatus = IBL.BO.DroneStatus.Maintenance;

            //--closetBaseStation.NumAvailablePositions;
            //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
            dal.AddDroneToCharge(drone.Id,closetBaseStation.Id);

        }

        /// <summary>
        /// A function that gets an id of drone and releasing it from charging.
        /// </summary>
        /// <param name="dId"></param>
        /// <param name="timeInCharging"></param>
        void ReleasingDrone(int dId, double timeInCharging)
        {
            if(!dal.ExistsInDroneList(dId))
            {
                throw new Exception("this id doesnt exist in the drone list!");
            }
            DroneToList drone=lDroneToList.Find(drone => drone.Id == dId);
            if(drone.DStatus!= IBL.BO.DroneStatus.Maintenance)
            {
                if (drone.DStatus == IBL.BO.DroneStatus.Free) throw new Exception("this drone in free state, it cant relese from charging!");
                else throw new Exception("this drone in delivery state,it cant relese from charging!");
            }
            drone.DStatus = IBL.BO.DroneStatus.Free;
            drone.BatteryStatus += timeInCharging * chargingRate;
            dal.ReleasingDrone(drone.Id);
        }

        /// <summary>
        /// A function that gets an id of drone and belonging to it a parcel.
        /// </summary>
        /// <param name="dId"></param>
        void BelongingParcel(int dId)
        {
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
                IEnumerable<IDal.DO.Parcel> dalParcelList = dal.GetListFromDal<IDal.DO.Parcel>().Where(parcel => parcel.Weight <= drone.MaxWeight);
                IEnumerable<IBL.BO.Parcel> blParcelList =;
                var conditionParcel = blParcelList.OrderByDescending(p => p.MPriority)
                      .ThenByDescending(p => p.Weight).ThenBy(p => CalculateDistance(new Location((dal.GetFromDalById<IDal.DO.Customer>(p.Sender.Id)).Longitude, (dal.GetFromDalById<IDal.DO.Customer>(p.Sender.Id)).Latitude, droneToList.CurrLocation));
                IBL.BO.Parcel resultParcel= conditionParcel.FirstOrDefault();
                IDal.DO.Customer getter = dal.GetFromDalById<IDal.DO.Customer>(resultParcel.Getter.Id);
                IDal.DO.Customer sender = dal.GetFromDalById<IDal.DO.Customer>(resultParcel.Sender.Id);
                double way = CalculateDistance(droneToList.CurrLocation,
                    new Location((dal.GetFromDalById<IDal.DO.Customer>(resultParcel.Sender.Id)).Longitude, (dal.GetFromDalById<IDal.DO.Customer>(resultParcel.Sender.Id)).Latitude),
                    new Location(Getter.Longitude,Getter.Latitude)
               
            }
            catch (DAL.IdNotExistInAListException)
            {
                throw new BelongingParcelException();
            }
        }
        public void AddingDrone(IBL.BO.Drone bLDrone, int StationId)
        {
            if (dal.ExistsInDroneList(bLDrone.Id))
            {
                throw new Exception("The id is already exists in the Drone list!");
            }
            if (!dal.ExistsInBaseStation(StationId))
            {
                throw new Exception("this base station doesnt exists!");
            }
            if (!dal.ThereAreFreePositions(StationId))
            {
                throw new Exception("There arent free positions for this base station!");
            }
            DroneToList droneToList = new DroneToList()
            {
                Id = bLDrone.Id,
                Model = bLDrone.Model,
                Weight = bLDrone.Weight,
                BatteryStatus = bLDrone.BatteryStatus,
                DStatus = bLDrone.DroneStatus,
                CurrLocation = new Location(dal.GetBaseStation(StationId).Longitude, dal.GetBaseStation(StationId).Latitude),
                NumOfParcel = null
            };

            IDal.DO.Drone drone = new IDal.DO.Drone() { Id = bLDrone.Id, MaxWeight = bLDrone.Weight, Model = bLDrone.Model };
            dal.AddingDrone(drone);
        }
        public void UpdatingDroneName(int droneId, int newModel)
        {
            try
            {
                IDal.DO.Drone drone = dal.GetDrone(droneId);
                drone.Model = newModel;
                dal.UpdateDrone(droneId, drone);
            }
            catch (DAL.IdNotExistInTheListException)
            {
                //bl exception-new
                throw new UpdatingFailedIdNotExistsException("this id doesnt exist in drone list!");
            }
        }

        private IBL.BO.Customer MapCustomer(IDal.DO.Customer input)
        {
            throw new NotImplementedException();
        }
        private IBL.BO.Parcel MapParcel(IDal.DO.Parcel input)
        {
            throw new NotImplementedException();
        }
    }
}
