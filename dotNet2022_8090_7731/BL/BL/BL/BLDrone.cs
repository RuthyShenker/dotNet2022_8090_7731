using BO;
using DalObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    partial class BL
    {
        private void InitializeDroneList()
        {
            lDroneToList = new ObservableCollection<DroneToList>();
            foreach (var drone in dal.GetListFromDal<IDAL.DO.Drone>())
                lDroneToList.Add(ConvertToList(drone));
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Drone and Expands it to object of 
        /// IBL.BO.DroneToList Considering of course with logic.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private DroneToList ConvertToList(IDAL.DO.Drone drone)
        {
            DroneToList nDrone = CopyCommon(drone);
            var parcel = dal.GetFromDalByCondition<IDAL.DO.Parcel>(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
            if (!parcel.Equals(default(IDAL.DO.Parcel)))
            {
                return CalculateDroneInDelivery(nDrone, parcel);
            }
            else
            {
                //nDrone.NumOfParcel = null; // אולי לא צריך שורה זו
                return CalculateUnDeliveryingDrone(nDrone);
            }
        }

        /// <summary>
        /// Gets drone and parcel (which belonged to the drone)
        /// Calculate the fields of drone, and return it. 
        /// </summary>
        /// <param name="nDrone"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private DroneToList CalculateDroneInDelivery(DroneToList nDrone, IDAL.DO.Parcel parcel)
        {
            nDrone.DStatus = DroneStatus.Delivery;

            //location:
            var sender = dal.GetFromDalByCondition<IDAL.DO.Customer>(customer => customer.Id == parcel.SenderId);
            if (parcel.BelongParcel != null && parcel.PickingUp == null)
            {
                nDrone.CurrLocation = ClosestStation(new Location(sender.Longitude, sender.Latitude)).Location;
            }
            else if (parcel.Arrival == null && parcel.PickingUp != null)
            {
                nDrone.CurrLocation = new Location(sender.Longitude, sender.Latitude);
            }
            // battery Status:
            Location destinationLocation = GetCustomer(parcel.GetterId).Location;
            Location closestStationLocation = ClosestStation(destinationLocation).Location;
            double distance = Extensions.CalculateDistance(nDrone.CurrLocation, destinationLocation, closestStationLocation);
            double minBattery = MinBattery(distance, (WeightCategories)parcel.Weight);
            nDrone.BatteryStatus = rand.NextDouble() * (100 - minBattery) + minBattery;
            nDrone.DeliveredParcelId = parcel.Id;
            return nDrone;
        }

        /// <summary>
        /// Get drone which his status is not 'Delivery'
        /// Calculate his fields and returns it.
        /// </summary>
        /// <param name="nDrone"></param>
        /// <returns></returns>
        private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
        {
            //it rands free or maintance.
            nDrone.DStatus = (DroneStatus)rand.Next((int)DroneStatus.Delivery);
            if (nDrone.DStatus == DroneStatus.Maintenance)
            {
                var stationDalList = dal.GetListFromDal<IDAL.DO.BaseStation>();
                var station = stationDalList.ElementAt(rand.Next(stationDalList.Count()));
                nDrone.CurrLocation = new Location(station.Longitude, station.Latitude);
                nDrone.BatteryStatus = rand.NextDouble() * 20;
            }
            else // droneToList.DStatus == Free
            {
                var customersList = CustomersWithProvidedParcels();
                if (customersList.Count > 0)
                {
                    nDrone.CurrLocation = customersList[rand.Next(customersList.Count)].Location;
                    var closetStation = ClosestStation(nDrone.CurrLocation);
                    double distance = Extensions.CalculateDistance(nDrone.CurrLocation, closetStation.Location);
                    nDrone.BatteryStatus = rand.NextDouble() * (100 - MinBattery(distance)) + MinBattery(distance);
                }
                //מה אם אין לקוחות שסופקו להם חבילות.
            }
            return nDrone;
        }

        /// <summary>
        /// A function that builds new DroneToList object and gets an object of IDAL.DO.Drone
        /// and copies from the object-IDAL.DO.Drone the common fields.
        /// </summary>
        private DroneToList CopyCommon(IDAL.DO.Drone source)
        {
            return new DroneToList(
            source.Id,
            source.Model,
            (WeightCategories)source.MaxWeight);
        }

        /// <summary>
        ///  
        /// A function that gets an object of IDAL.DO.Drone
        /// and Expands it to Drone object and returns this object.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>returns Drone object </returns>
        private Drone ConvertToBL(IDAL.DO.Drone drone)
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
            //TODO:האם ככה מחשבים דרך מבחינת אחוזים וכו...
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
            var belongedParcel = dal.GetFromDalByCondition<IDAL.DO.Parcel>(parcel => parcel.DroneId == droneId);
            if (belongedParcel.Equals(default(IDAL.DO.Parcel)))
            {
                return new ParcelInTransfer();
            }
            else
            {
                var parcel = ConvertToBL(belongedParcel);
                Location senderLocation = GetCustomer(parcel.Sender.Id).Location;
                Location getterLocation = GetCustomer(parcel.Getter.Id).Location;
                double distance = Extensions.CalculateDistance(senderLocation, getterLocation);
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
            if (GetNumOfAvailablePositionsInStation(closetdStation.Id) == 0)
            {
                throw new InValidActionException("There ara no stations with available positions!");
            }

            double distanceFromDroneToStation = Extensions.CalculateDistance(closetdStation.Location, drone.CurrLocation);
            double minBattery = MinBattery(distanceFromDroneToStation, drone.Weight);
            if (drone.BatteryStatus - minBattery < 0)
            {
                throw new InValidActionException("The drone has no enough battery in order to get to the closest charging station");
            }

            drone.BatteryStatus = minBattery;
            drone.CurrLocation = closetdStation.Location;
            drone.DStatus = DroneStatus.Maintenance;
            //--closetBaseStation.NumAvailablePositions;
            //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
            dal.Add(new IDAL.DO.ChargingDrone(drone.Id, closetdStation.Id, DateTime.Now));
        }

        /// <summary>
        /// A function that gets an id of drone and releasing it from charging.
        /// </summary>
        /// <param name="dId"></param>
        public void ReleasingDrone(int dId)
        {
            DroneToList drone = FindDroneInList(dId);
            IDAL.DO.ChargingDrone chargingDrone = dal.GetFromDalByCondition<IDAL.DO.ChargingDrone>(d => d.DroneId == dId);
            double timeInCharging = DateTime.Now.Subtract(chargingDrone.EnteranceTime).TotalMinutes;

            switch (drone.DStatus)
            {
                case DroneStatus.Free:
                    throw new InValidActionException(typeof(Drone), dId, $"status of drone is Free ");
                case DroneStatus.Delivery:
                    throw new InValidActionException(typeof(Drone), dId, $"status of drone is Delivery ");
                default:
                    drone.DStatus = DroneStatus.Free;
                    drone.BatteryStatus += timeInCharging * chargingRate;
                    var ChargingDroneToRemove = dal.GetFromDalByCondition<IDAL.DO.ChargingDrone>(charge => charge.DroneId == drone.Id);
                    dal.Remove(ChargingDroneToRemove);
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
            var optionParcels = dal.GetDalListByCondition<IDAL.DO.Parcel>
                (parcel => parcel.Weight <= (IDAL.DO.WeightCategories)droneToList.Weight &&
                droneToList.BatteryStatus >= MinBattery(GetDistance(droneToList.CurrLocation, parcel), (WeightCategories)parcel.Weight))
                .OrderByDescending(parcel => parcel.MPriority)
                .ThenByDescending(parcel => parcel.Weight)
                .ThenBy(parcel => GetDistance(droneToList.CurrLocation, parcel));

            var parcel = optionParcels.FirstOrDefault(parcel => parcel.BelongParcel.HasValue);
            if (!optionParcels.Any() || parcel.Equals(default(IDAL.DO.Parcel)))
            {
                if (!dal.GetListFromDal<IDAL.DO.Parcel>().Any())
                {
                    throw new ListIsEmptyException(typeof(IDAL.DO.Parcel));
                }
                throw new ThereIsNoMatchObjectInList(typeof(IDAL.DO.Parcel), $"There is no match parcels to drone with id {dId} in");
            }

            droneToList.DStatus = DroneStatus.Delivery;

            dal.Update<IDAL.DO.Parcel>(parcel.Id, dId, nameof(parcel.DroneId));
            dal.Update<IDAL.DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.BelongParcel));
        }

        /// <summary>
        /// A function that calculates the distance that the drone has to pass in order to give 
        /// a parcel to the destination and go the the closet base station in order to
        /// go charging .
        /// </summary>
        /// <param name="droneLocation"></param>
        /// <param name="parcel"></param>
        /// <returns>the function returns this distance</returns>
        private double GetDistance(Location droneLocation, IDAL.DO.Parcel parcel)
        {
            Location senderLocation = GetCustomer(parcel.SenderId).Location;
            Location getterLocation = GetCustomer(parcel.SenderId).Location;
            return Extensions.CalculateDistance(droneLocation, senderLocation, getterLocation, ClosestStation(getterLocation).Location);
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
            if (dal.IsIdExistInList<IDAL.DO.Drone>(bLDrone.Id))
            {
                throw new IdIsAlreadyExistException(typeof(IDAL.DO.Drone), bLDrone.Id);
            }
            try
            {
                IDAL.DO.BaseStation station = dal.GetFromDalById<IDAL.DO.BaseStation>(StationId);
                if (GetNumOfAvailablePositionsInStation(StationId) == 0)
                {
                    throw new InValidActionException(typeof(IDAL.DO.BaseStation), StationId, "There aren't free positions ");
                }

                bLDrone.BatteryStatus = rand.Next(20, 41);
                bLDrone.DroneStatus = DroneStatus.Maintenance;
                dal.Add(new IDAL.DO.ChargingDrone(bLDrone.Id, StationId, DateTime.Now));

                lDroneToList.Add(new DroneToList(
                    bLDrone.Id,
                    bLDrone.Model,
                    bLDrone.Weight,
                    bLDrone.BatteryStatus,
                    bLDrone.DroneStatus,
                    new Location(station.Longitude, station.Latitude),
                    null
                    ));

                dal.Add(new IDAL.DO.Drone()
                {
                    Id = bLDrone.Id,
                    MaxWeight = (IDAL.DO.WeightCategories)bLDrone.Weight,
                    Model = bLDrone.Model
                });
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDAL.DO.BaseStation), StationId);
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
            try
            {
                dal.Update<IDAL.DO.Drone>(droneId, newModel, nameof(IDAL.DO.Drone.Model));
                droneToList.Model = newModel;
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Drone), droneId);
            }
        }
        public IEnumerable<DroneToList> GetDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
            {
                return lDroneToList.Select(d => new DroneToList(d));
            }
            else
            {
                return lDroneToList.Where(predicate).Select(d=>new DroneToList(d));
            }

        }

        public Drone GetDrone(int droneId)
        {
            try
            {
                var dDrone = dal.GetFromDalById<IDAL.DO.Drone>(droneId);
                return ConvertToBL(dDrone);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Drone), droneId);
            }
        }
    }
}

