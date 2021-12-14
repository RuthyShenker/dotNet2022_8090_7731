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
        /// A function that gets weight of drone
        /// and distance and returns the minimum battery that 
        /// the drone needs in order to flight.
        /// Default value of weight=0.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>the minimum battery in double</returns>
        private double MinBattery(double distance, WeightCategories weight = 0)
        {
            return weight switch
            {
                WeightCategories.Light => powerConsumptionLight * distance,
                WeightCategories.Heavy => powerConsumptionHeavy * distance,
                WeightCategories.Medium => powerConsumptionMedium * distance,
                _ => powerConsumptionFree * distance,
            };
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
                Location senderLocation = GetCustomer(parcel.Sender.Id).CLocation;
                Location getterLocation = GetCustomer(parcel.Getter.Id).CLocation;
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

            Station closetdStation = ClosestStation(drone.CurrLocation, true);
            if (dal.AreThereFreePositions(closetdStation.Id) == 0)
            {
                throw new InValidActionException("There ara no stations with available positions!");
            }

            double distanceFromDroneToStation = CalculateDistance(closetdStation.SLocation, drone.CurrLocation);
            double minBattery = MinBattery(distanceFromDroneToStation, drone.Weight);
            if (drone.BatteryStatus - minBattery < 0)
            {
                throw new InValidActionException("The drone has no enough battery in order to get to the closest charging station");
            }

            drone.BatteryStatus = minBattery;
            drone.CurrLocation = closetdStation.SLocation;
            drone.DStatus = DroneStatus.Maintenance;
            //--closetBaseStation.NumAvailablePositions;
            //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
            dal.AddingToData(new IDal.DO.ChargingDrone(drone.Id, closetdStation.Id));
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
            DroneToList droneToList = FindDroneInList(dId);
            if (droneToList.DStatus != DroneStatus.Free)
            {
                string dStatus = droneToList.DStatus.ToString();
                throw new InValidActionException(typeof(Drone), dId, $"status of drone is {dStatus} ");
            }
            var optionParcels = dal.GetDalListByCondition<IDal.DO.Parcel>
                (parcel => parcel.Weight <= (IDal.DO.WeightCategories)droneToList.Weight &&
                droneToList.BatteryStatus >= MinBattery(GetDistance(droneToList.CurrLocation, parcel), (WeightCategories)parcel.Weight))
                .OrderByDescending(parcel => parcel.MPriority)
                .ThenByDescending(parcel => parcel.Weight)
                .ThenBy(parcel => GetDistance(droneToList.CurrLocation, parcel));

            var parcel = optionParcels.FirstOrDefault(parcel => parcel.BelongParcel.HasValue);
            if (!optionParcels.Any() || parcel.Equals(default(IDal.DO.Parcel)))
            {
                if (!dal.GetListFromDal<IDal.DO.Parcel>().Any())
                {
                    throw new ListIsEmptyException(typeof(IDal.DO.Parcel));
                }
                throw new ThereIsNoMatchObjectInList(typeof(IDal.DO.Parcel), $"There is no match parcels to drone with id {dId} in");
            }

            droneToList.DStatus = DroneStatus.Delivery;

            dal.UpdatingInData<IDal.DO.Parcel>(parcel.Id, dId, nameof(parcel.DroneId));
            dal.UpdatingInData<IDal.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.BelongParcel));
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
            Location senderLocation = GetCustomer(parcel.SenderId).CLocation;
            Location getterLocation = GetCustomer(parcel.SenderId).CLocation;
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
                throw new IdIsAlreadyExistException(typeof(IDal.DO.Drone), bLDrone.Id);
            }
            try
            {
                IDal.DO.BaseStation station = dal.GetFromDalById<IDal.DO.BaseStation>(StationId);
                if (dal.AreThereFreePositions(StationId) == 0)
                {
                    throw new InValidActionException(typeof(IDal.DO.BaseStation), StationId, "There aren't free positions ");
                }

                bLDrone.BatteryStatus = DalObject.DataSource.Rand.Next(20, 41);
                bLDrone.DroneStatus = DroneStatus.Maintenance;
                dal.AddingToData(new IDal.DO.ChargingDrone(bLDrone.Id, StationId));

                lDroneToList.Add(new DroneToList(
                    bLDrone.Id,
                    bLDrone.Model,
                    bLDrone.Weight,
                    bLDrone.BatteryStatus,
                    bLDrone.DroneStatus,
                    new Location(station.Longitude, station.Latitude),
                    null
                    ));

                dal.AddingToData(new IDal.DO.Drone()
                {
                    Id = bLDrone.Id,
                    MaxWeight = (IDal.DO.WeightCategories)bLDrone.Weight,
                    Model = bLDrone.Model
                });
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDal.DO.BaseStation), StationId);
            }
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

            IDal.DO.Drone dDrone;
            try
            {
                dDrone = dal.GetFromDalById<IDal.DO.Drone>(droneId);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Drone), droneId);
            }
            dal.UpdatingInData<IDal.DO.Drone>(droneId, newModel, nameof(dDrone.Model));
        }
        public IEnumerable<DroneToList> GetDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
            {
                return lDroneToList;
            }
            else
            {
                return lDroneToList.Where(predicate);
            }

        }

        public Drone GetDrone(int droneId)
        {
            try
            {
                var dDrone = dal.GetFromDalById<IDal.DO.Drone>(droneId);
                return ConvertToBL(dDrone);
            }
            catch (DalObject.IdIsNotExistException) 
            { 
                throw new IdIsNotExistException(typeof(Drone), droneId); 
            }
        }
    }
}

