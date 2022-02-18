using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Extensions;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// An internal sealed partial class BL inherits from Singleton<BL>,and impliments BlApi.IBL,
    /// </summary>
    partial class BL:BlApi.IBL
    {
        /// <summary>
        /// A function that gets drone-Id ,updateView ,checkStop and open simulator to this drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="updateView"></param>
        /// <param name="checkStop"></param>
        public void StartSimulator(int droneId, Action<object> updateView, Func<bool> checkStop)
        {
            new Simulator(this, droneId, updateView, checkStop);
        }

        /// <summary>
        /// A function that gets Drone and station id and adds this Drone to the data base and sending
        /// this drone to charging in the StationId that the function gets,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bLDrone"></param>
        /// <param name="StationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddingDrone(Drone bLDrone, int StationId)
        {
            lock (dal)
            {
                try
                {
                    if (dal.IsIdExistInList<DO.Drone>(bLDrone.Id))
                    {
                        throw new BO.IdAlreadyExistsException(typeof(Drone), bLDrone.Id);
                    }
                }
                catch (DO.XMLFileLoadCreateException ex)
                {
                    throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
                }
                try
                {
                    DO.BaseStation station = dal.GetFromDalById<DO.BaseStation>(StationId);
                    if (GetNumOfAvailablePositionsInStation(StationId) == 0)
                    {
                        throw new BO.InValidActionException(typeof(DO.BaseStation), StationId, "There aren't free positions ");
                    }

                    dal.Add( new DO.ChargingDrone()
                    {
                        DroneId = bLDrone.Id,
                        StationId = StationId, 
                        EnteranceTime = DateTime.Now
                    });

                    lDroneToList.Add(new()
                    {
                        Id = bLDrone.Id,
                        Model = bLDrone.Model,
                        Weight = bLDrone.Weight,
                        BatteryStatus = 0,
                        DStatus = DroneStatus.Maintenance,
                        CurrLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude },
                        DeliveredParcelId = null
                    });

                    dal.Add(new DO.Drone()
                    {
                        Id = bLDrone.Id,
                        MaxWeight = (DO.WeightCategories)bLDrone.Weight,
                        Model = bLDrone.Model
                    });
                }
                catch (DO.IdDoesNotExistException)
                {
                    throw new BO.IdIsNotExistException(typeof(DO.BaseStation), StationId);
                }
                catch (ArgumentNullException )
                {
                    throw ;
                }
                catch (DO.XMLFileLoadCreateException ex )
                {
                    throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
                }
            }
        }

        /// <summary>
        /// A function that gets droneId and newModel and updates the drone with the id of 
        /// droneId to be with the model of newModel, the function doesn't return anything.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newModel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatingDroneName(int droneId, string newModel)
        {
            DroneToList droneToList = FindDroneInList(droneId);
            try
            {
                lock (dal)
                {
                    dal.Update<DO.Drone>(droneId, newModel, nameof(DO.Drone.Model));
                }
                droneToList.Model = newModel;
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new BO.IdIsNotExistException(typeof(Drone), droneId);
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (NotSupportedException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// A function that gets predicate ,if the predicate is null returns the whole lDroneToList
        /// else returns from lDroneToList all the drones that stands on this predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>if the predicate is null returns the whole lDroneToList
        /// else returns from lDroneToList all the drones that stands on this predicate.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDrones(Func<DroneToList, bool> predicate = null)
        {
            return predicate == null
                ? lDroneToList.Select(d => new DroneToList()
                {
                    Id = d.Id,
                    Weight = d.Weight,
                    Model = d.Model,
                    DStatus = d.DStatus,
                    DeliveredParcelId = d.DeliveredParcelId,
                    CurrLocation = d.CurrLocation,
                    BatteryStatus = d.BatteryStatus
                })
                : lDroneToList.Where(predicate).Select(d => new DroneToList()
                {
                    Id = d.Id,
                    Weight = d.Weight,
                    Model = d.Model,
                    DStatus = d.DStatus,
                    DeliveredParcelId = d.DeliveredParcelId,
                    CurrLocation = d.CurrLocation,
                    BatteryStatus = d.BatteryStatus
                });
        }

        /// <summary>
        /// A function that gets id of drone land returns the instance with this id after converts it to bl type.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns> returns the instance with this id after convers it to bl type.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            try
            {
                lock (dal)
                {
                    var dDrone = dal.GetFromDalById<DO.Drone>(droneId);
                    return ConvertToBL(dDrone);
                }
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new BO.IdIsNotExistException(typeof(Drone), droneId);
            }
            catch (DO.XMLFileLoadCreateException ex)
            {
                throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// A function that gets id of drone and deletes it from the db,returns string that the action performed successfully
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>returns string that the action performed successfully</returns>
        public string DeleteDrone(int droneId)
        {
            try
            {
                lock (dal)
                {
                    dal.Remove(dal.GetFromDalById<DO.Drone>(droneId));
                }
                lDroneToList.Remove(lDroneToList.Find(d => d.Id == droneId));
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Drone), droneId);
            }
            catch (ArgumentNullException)
            {
                throw ;
            }
            catch (DO.XMLFileLoadCreateException ex)
            {
                throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
            }
            return $"The drone with Id: {droneId} was successfully removed from the system";
        }

        /// <summary>
        /// A function that returns list of Power Consumptions.
        /// </summary>
        /// <returns>returns list of Power Consumptions </returns>
        public IEnumerable<double> GetPowerConsumption()
        {
            return new List<double>
            {
                PowerConsumptionFree,
                powerConsumptionLight,
                powerConsumptionMedium,
                powerConsumptionHeavy,
                chargingRate
            };
        }


        /// <summary>
        /// A function that gets an id of drone and releasing it from charging.
        /// </summary>
        /// <param name="dId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleasingDrone(int dId)
        {
            DroneToList drone = FindDroneInList(dId);
            DO.ChargingDrone chargingDrone;
            try
            {
                lock (dal)
                {
                    chargingDrone = dal.GetFromDalByCondition<DO.ChargingDrone>(d => d.DroneId == dId);
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
            }
            double timeInCharging = DateTime.Now.Subtract(chargingDrone.EnteranceTime).TotalMinutes;

            switch (drone.DStatus)
            {
                case DroneStatus.Free:
                    throw new BO.InValidActionException(typeof(Drone), dId, $"status of drone is Free ");
                case DroneStatus.Delivery:
                    throw new BO.InValidActionException(typeof(Drone), dId, $"status of drone is Delivery ");
                default:
                    //is correct?

                    drone.BatteryStatus = Math.Min(drone.BatteryStatus + timeInCharging * chargingRate, 100);
                    drone.DStatus = DroneStatus.Free;
                    try
                    {
                        lock (dal)
                        {
                            var ChargingDroneToRemove = dal.GetFromDalByCondition<DO.ChargingDrone>(charge => charge.DroneId == drone.Id);
                            dal.Remove(ChargingDroneToRemove);
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        throw;
                    }
                    catch (BO.XMLFileLoadCreateException ex)
                    {
                        throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
                    }
                    break;
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Drone and Expands it to object of 
        /// IBL.BO.DroneToList Considering of course with logic.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private DroneToList ConvertToList(DO.Drone drone)
        {
            DroneToList nDrone = CopyCommon(drone);
            DO.Parcel parcel;
            try
            {
                // return parcel which the drone has to delivery, otherwise- default(IDAL.DO.Parcel)
                parcel = dal.GetFromDalByCondition<DO.Parcel>(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                throw new BO.XMLFileLoadCreateException(ex.xmlFilePath, $"fail to load xml file: {ex.xmlFilePath}", ex);
            }
            if (!parcel.Equals(default(DO.Parcel)))
            {
                //+ checking if it can be:
                return CalculateDroneInDelivery(nDrone, parcel);
            }
            else
            {
                //nDrone.NumOfParcel = null; // אולי לא צריך שורה זו
                return CalculateUnDeliveryingDrone(nDrone);
            }
        }

        /// <summary>
        /// Gets drone and parcel (which assigned to the drone)
        /// Calculate the fields of drone, and return it. 
        /// </summary>
        /// <param name="nDrone"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private DroneToList CalculateDroneInDelivery(DroneToList nDrone, DO.Parcel parcel)
        {
            try
            {
                //location:
                var sender = dal.GetFromDalByCondition<DO.Customer>(customer => customer.Id == parcel.SenderId);
                if (parcel.BelongParcel != null && parcel.PickingUp == null)
                {
                    nDrone.CurrLocation = ClosestStation(new() { Longitude = sender.Longitude, Latitude = sender.Latitude }).Location;
                }
                else if (parcel.Arrival == null && parcel.PickingUp != null)
                {
                    nDrone.CurrLocation = new() { Longitude = sender.Longitude, Latitude = sender.Latitude };
                }

                // battery Status:
                Location source = GetCustomer(parcel.SenderId).Location;
                Location destination = GetCustomer(parcel.GetterId).Location;
                Location nearestDestinationStation = ClosestStation(destination).Location;
                double minBattry = MinBattery(CalculateDistance(nDrone.CurrLocation, source))
                    + MinBattery(CalculateDistance(source, destination), (WeightCategories)parcel.Weight)
                    + MinBattery(CalculateDistance(destination, nearestDestinationStation));

                if (minBattry > 100)
                {
                    //נשאר  באותו מיקום?
                    nDrone.DStatus = DroneStatus.Free;
                    nDrone.BatteryStatus = RandBetweenRange(0, 100);
                    nDrone.DeliveredParcelId = default;
                    dal.Update<DO.Parcel>(parcel.Id, null, nameof(DO.Parcel.DroneId));
                    dal.Update<DO.Parcel>(parcel.Id, null, nameof(DO.Parcel.BelongParcel));
                    dal.Update<DO.Parcel>(parcel.Id, null, nameof(DO.Parcel.PickingUp));
                    //var e = dal.GetFromDalById<DO.Parcel>(parcel.Id);
                }
                else
                {
                    nDrone.DStatus = DroneStatus.Delivery;
                    nDrone.BatteryStatus = RandBetweenRange(minBattry, 100);
                    nDrone.DeliveredParcelId = parcel.Id;
                }
            }
            catch (DO.IdDoesNotExistException  )
            {
                throw new IdIsNotExistException();
            }
            catch (ArgumentNullException)
            {
                throw new BO.ThereIsNoMatchObjectInListException();
            }
            return nDrone;
        }

        /// <summary>
        /// A function that gets min value and max value and rand a double number in this range.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>returns the number that is randed.</returns>
        private double RandBetweenRange(double min, double max)
        {
            return (rand.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Get drone which his status is not 'Delivery'
        /// Calculate his fields and returns it.
        /// </summary>
        /// <param name="nDrone"></param>
        /// <returns></returns>
        private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
        {
            //if the list of charging drone empty or this drone is not on charging so it is free.
            if (!dal.GetListFromDal<DO.ChargingDrone>().Any() ||
                dal.GetListFromDal<DO.ChargingDrone>().FirstOrDefault(chargingDrone => chargingDrone.DroneId == nDrone.Id).Equals(default(DO.ChargingDrone)))
                nDrone.DStatus = DroneStatus.Free;
            else
                nDrone.DStatus = DroneStatus.Maintenance;

            var customersList = CustomersWithProvidedParcels();


            if (nDrone.DStatus == DroneStatus.Maintenance || !customersList.Any())
            {
                #region
                //var availableSlotsList = AvailableSlots();
                //if (availableSlotsList.Any())
                //{
                //    var station = availableSlotsList.ElementAt(rand.Next(availableSlotsList.Count()));
                //    //maybe to rand to closet station?.
                //    nDrone.CurrLocation = GetStation(station.Id).Location;
                //    nDrone.BatteryStatus = rand.NextDouble() * 20;

                //    if (nDrone.DStatus == DroneStatus.Maintenance)
                //    {
                //        dal.Add(new DO.ChargingDrone(nDrone.Id, station.Id, DateTime.Now));
                //    }
                //}
                //else
                //{

                //}
                #endregion

                if (nDrone.DStatus == DroneStatus.Maintenance)
                {
                    int stationId = dal.GetDalListByCondition<DO.ChargingDrone>(c => c.DroneId == nDrone.Id).First().StationId;
                    var station = dal.GetFromDalByCondition<DO.BaseStation>(s => s.Id == stationId);
                    nDrone.CurrLocation = new() { Longitude = station.Longitude, Latitude = station.Latitude };

                }
                else //customersList.Count() == 0
                {
                    var idStation = GetStations().ElementAt(rand.Next(GetStations().Count())).Id;
                    var longitude = dal.GetFromDalById<DO.BaseStation>(idStation).Longitude;
                    var latitude = dal.GetFromDalById<DO.BaseStation>(idStation).Latitude;
                    nDrone.CurrLocation = new() { Longitude = longitude, Latitude = latitude };
                }
                nDrone.BatteryStatus = rand.NextDouble() * 20;
            }
            else //free + customersList.Count() != 0
            {
                nDrone.CurrLocation = customersList.ElementAt(rand.Next(customersList.Count())).Location;
                var closetStation = ClosestStation(nDrone.CurrLocation);
                double minBattery = MinBattery(CalculateDistance(nDrone.CurrLocation, closetStation.Location));
                nDrone.BatteryStatus = RandBetweenRange(minBattery, 100);
            }
            return nDrone;
        }

        /// <summary>
        /// A function that builds new DroneToList object and gets an object of IDAL.DO.Drone
        /// and copies from the object-IDAL.DO.Drone the common fields.
        /// </summary>
        private DroneToList CopyCommon(DO.Drone source) => new()
        {
            Id = source.Id,
            Model = source.Model,
            Weight = (WeightCategories)source.MaxWeight
        };

        /// <summary>
        ///  
        /// A function that gets an object of IDAL.DO.Drone
        /// and Expands it to Drone object and returns this object.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>returns Drone object </returns>
        private Drone ConvertToBL(DO.Drone drone)
        {
            ParcelInTransfer parcelInTransfer = CalculateParcelInTransfer(drone.Id);
            var requiredDrone = lDroneToList.FirstOrDefault(droneToList => droneToList.Id == drone.Id);
            return new()
            {
                Id = requiredDrone.Id,
                Model = requiredDrone.Model,
                Weight = requiredDrone.Weight,
                BatteryStatus = requiredDrone.BatteryStatus,
                DroneStatus = requiredDrone.DStatus,
                CurrLocation = requiredDrone.CurrLocation,
                PInTransfer = parcelInTransfer
            };
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
        private double MinBattery(double distance, WeightCategories weight = 0) => weight switch
        {
            WeightCategories.Light => powerConsumptionLight * distance,
            WeightCategories.Heavy => powerConsumptionHeavy * distance,
            WeightCategories.Medium => powerConsumptionMedium * distance,
            _ => PowerConsumptionFree * distance,
        };

        /// <summary>
        /// A function that creates ParcelInTransferand
        /// Calculates bills for specific drone id 
        /// and returns this ParcelInTransfer.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>returns ParcelInTransfer object.</returns>
        private ParcelInTransfer CalculateParcelInTransfer(int droneId)
        {
            //DO.Parcel belongedParcel = dal.GetFromDalByCondition<DO.Parcel>(
            //    parcel => parcel.DroneId == droneId
            //    //&& parcel.PickingUp.HasValue 
            //    //&& !parcel.Arrival.HasValue
            //    );
            IEnumerable<DO.Parcel> belongedParcels = dal.GetDalListByCondition<DO.Parcel>(p => p.DroneId == droneId);

            if (belongedParcels.Count() == 0) 
            {
                return null;
            }

            // הורמה ולא הגיעה
            var parcel = belongedParcels.FirstOrDefault(p => p.PickingUp != null && p.Arrival == null);

            if (parcel.Equals(default(DO.Parcel)))
            {
                // שויכה ולא הורמה
                parcel = belongedParcels.FirstOrDefault(p => p.BelongParcel != null && p.PickingUp == null);
            }
            if (parcel.SenderId==0)
            {
                var a = 9;
            }
            if (parcel.Equals(default(DO.Parcel)))
            {
                // שויכה ולא הורמה
                return null;
            }
            var BLParcel = ConvertToBL(parcel);
            Location senderLocation = GetCustomer(BLParcel.Sender.Id).Location;
            Location getterLocation = GetCustomer(BLParcel.Getter.Id).Location;
            double distance = Extensions.CalculateDistance(senderLocation, getterLocation);
            return new()
            {
                PId = BLParcel.Id,
                IsInWay = BLParcel.PickingUp.HasValue,
                MPriority = BLParcel.MPriority,
                Weight = BLParcel.Weight,
                Sender = BLParcel.Sender,
                Getter = BLParcel.Getter,
                CollectionLocation = senderLocation,
                DeliveryLocation = getterLocation,
                TransDistance = distance
            };
        }

        /// <summary>
        /// A function that gets an id of drone and sending it to charging,the 
        /// function doesn't return anything.
        /// </summary>
        /// <param name="IdDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendingDroneToCharge(int IdDrone)
        {
            DroneToList drone = FindDroneInList(IdDrone);
            switch (drone.DStatus)
            {
                case DroneStatus.Maintenance:
                    throw new BO.InValidActionException(typeof(Drone), IdDrone, $"status of drone is Maintenance ");
                case DroneStatus.Delivery:
                    throw new BO.InValidActionException(typeof(Drone), IdDrone, $"status of drone is Delivery ");
                default:
                    break;
            }

            Station closetStation = ClosestStation(drone.CurrLocation, true);
            if (closetStation.Equals(default))
            {
                throw new BO.InValidActionException("There ara no stations with available positions!");
            }

            double distanceFromDroneToStation = CalculateDistance(drone.CurrLocation, closetStation.Location);
            double minBattery = MinBattery(distanceFromDroneToStation);
            if (minBattery > drone.BatteryStatus)
            {
                throw new BO.InValidActionException("The drone has no enough battery in order to get to the closest charging station");
            }

            drone.BatteryStatus -= minBattery;
            drone.CurrLocation = closetStation.Location;
            drone.DStatus = DroneStatus.Maintenance;
           
            dal.Add(new DO.ChargingDrone()
            {
                DroneId = drone.Id,
                StationId = closetStation.Id,
                EnteranceTime = DateTime.Now
            });
        }

        /// <summary>
        /// A function that gets an id of drone and releasing it from charging.
        /// </summary>
        /// <param name="dId"></param>
        /// <returns>returns the drone with this id from the lDroneToList</returns>
        private DroneToList FindDroneInList(int dId)
        {
            try
            {
                return lDroneToList.First(drone => drone.Id == dId);
            }
            catch (ArgumentNullException)
            {
                throw new BO.ListIsEmptyException(typeof(Drone));
            }
            catch (InvalidOperationException)
            {
                throw new BO.IdIsNotExistException(typeof(Drone), dId);
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
        private double GetDistance(Location droneLocation, DO.Parcel parcel)
        {
            Location senderLocation = GetCustomer(parcel.SenderId).Location;
            //Location getterLocation = GetCustomer(parcel.GetterId).Location;
            return Extensions.CalculateDistance(droneLocation, senderLocation/*, getterLocation, ClosestStation(getterLocation).Location*/);
        }

        /// <summary>
        /// A function that initializes the lDroneToListof the BL.
        /// </summary>
        private void InitializeDroneList()
        {
            lDroneToList = new List<DroneToList>();
            lock (dal)
            {
                foreach (var drone in dal.GetListFromDal<DO.Drone>())
                {
                    lDroneToList.Add(ConvertToList(drone));
                }
            }
        }
    }
}

