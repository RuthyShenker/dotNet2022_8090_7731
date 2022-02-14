using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    class Simulator
    {
        // maybe good for calculate distances
        //  https://www.google.com/search?q=geography+location+and+calculate+distance+c%23&oq=geography+location+and+calculate+distance+c%23&aqs=chrome..69i57.22174j0j9&sourceid=chrome&ie=UTF-8

        private const int DELAY = 500;
        private const double VELOCITY = 1.0;
        private const double TIME_STEP = DELAY / 1000.0;
        private const double STEP = VELOCITY / TIME_STEP;
        private Maintenance maintenance = Maintenance.Assigning;
        private BL bl;
        Action updateView;
        Func<bool> checkStop;
        DO.Parcel parcel;
        int stationId;
        Station station;
        Customer customer;
        DalApi.IDal dal;
        double distance;
        bool pickedUp;
        double[] powerConsumption;

        int droneFree = 0;
         
        //        powerConsumptionFree,
        //        powerConsumptionLight,
        //        powerConsumptionMedium,
        //        powerConsumptionHeavy,
        //        chargingRate
        public enum Maintenance { Assigning, GoingTowardStation, Charging };

        public Simulator(BL blInstance, int droneId, Action updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            dal = blInstance.dal;
            powerConsumption= bl.GetPowerConsumption().ToArray();

            Drone drone = bl.GetDrone(droneId);
            updateView = updateViewAction;
            checkStop = checkStopFunc;

            do
            {
                switch (drone.DroneStatus)
                {
                    case DroneStatus.Free:
                        TryAssumingParcel(drone);
                        break;
                    case DroneStatus.Maintenance:
                        CompleteCharging(drone);
                        break;
                    case DroneStatus.Delivery:
                        CompleteDelivery(drone);
                        break;
                    default:
                        break;
                }
            } while (!checkStop());
        }

        private void TryAssumingParcel(Drone drone)
        {
            if (!SleepDelayTime()) return;

            lock (bl)
            {
                var optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation);
                if (optionalParcels.Any())
                {
                    var parcel = optionalParcels.First();

                    switch (parcel.Id, drone.BatteryStatus)
                    {
                        // if there is no parcel to assign and drone's battery is full.  
                        case (0, 1.0):
                            break;

                        // if the is no parcel to assign and the the drone's battery is not full.
                        case (0, _):
                            station = bl.ClosestStation(drone.CurrLocation, true);
                            stationId = station.Id;

                            if (stationId != default)
                            {
                                drone.DroneStatus = DroneStatus.Maintenance;
                                maintenance = Maintenance.Assigning;
                                dal.Update<DO.BaseStation>(stationId, station.NumAvailablePositions - 1, nameof(station.NumAvailablePositions));
                                dal.Add(new DO.ChargingDrone(drone.Id, stationId, DateTime.Now));
                            }
                            break;

                        // if there is parcel to assign and there is enough battery.
                        case (_, _):
                            init(parcel.Id);
                            drone.DroneStatus = DroneStatus.Delivery;

                            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(DO.Parcel.BelongParcel));
                            dal.Update<DO.Parcel>(parcel.Id, drone.Id, nameof(DO.Parcel.DroneId));
                            break;

                        default:
                    }

                }
            }
        }

        //var sender =bl.GetCustomer(parcel.SenderId);
        //var getter = bl.GetCustomer(parcel.GetterId);
        //drone.PInTransfer = new ParcelInTransfer()
        //{
        //    PId = parcel.Id,
        //    MPriority = (Priority)parcel.MPriority,
        //    Weight = (WeightCategories)parcel.Weight,
        //    Sender = new() { Id = sender.Id, Name = sender.Name },
        //    Getter = new() { Id = getter.Id, Name = getter.Name },
        //    TransDistance = Extensions.CalculateDistance(sender.Location, getter.Location),
        //    CollectionLocation = sender.Location,
        //    DeliveryLocation = getter.Location,
        //    IsInWay = false
        //};
        private void CompleteCharging(Drone drone)
        {
            switch (maintenance)
            {
                //TODO what happens when there is no available station
                case Maintenance.Assigning:
                    station = bl.ClosestStation(drone.CurrLocation, true);
                    distance = Extensions.CalculateDistance(drone.CurrLocation, station.Location);
                    maintenance = Maintenance.GoingTowardStation;
                    break;

                case Maintenance.GoingTowardStation:
                    if (distance < 0.01)
                        lock (bl)
                        {
                            drone.CurrLocation = station.Location;
                            maintenance = Maintenance.Charging;
                        }
                    else
                    {
                        if (!SleepDelayTime()) break;
                        lock (bl)
                        {
                            double delta = distance < STEP ? distance : STEP;
                            distance -= delta;
                            //drone.Battery = Max(0.0, drone.Battery - delta * bl.BatteryUsages[DRONE_FREE]);
                        }
                    //lock (bl)
                    {
                        while (distance > 0.01 && !checkStop())
                        {
                            if (!SleepDelayTime()) break;
                            double delta = distance < STEP ? distance : STEP;
                            distance -= delta;
                            drone.BatteryStatus = Math.Max(0.0, drone.BatteryStatus - delta * []);
                                bl.
                            updateView();
                        }
                        if (distance <= 0.01)
                            lock (bl)
                            {
                                drone.CurrLocation = station.Location;
                                maintenance = Maintenance.Charging;
                            }
                    }
                    break;

                case Maintenance.Charging:
                    while (drone.BatteryStatus < 1.0 && !checkStop())
                    {
                        if (!SleepDelayTime()) break;
                        //lock (bl) למה בפרויקט לדוג עשו כאן?
                        {
                            drone.BatteryStatus = Math.Min(1.0, drone.BatteryStatus + chargingRate * TIME_STEP);
                            updateView();
                        }
                    }
                    if (drone.BatteryStatus >= 1.0)
                    {
                        drone.DroneStatus = DroneStatus.Free;
                    }
                    break;
                default:
                    break;
            }
        }

        private void init(int parcelId)
        {
            parcel = dal.GetFromDalById<DO.Parcel>(parcelId);
            pickedUp = parcel.PickingUp.HasValue;
            customer = bl.GetCustomer(pickedUp ? parcel.GetterId : parcel.SenderId);

            batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
        }

        private void CompleteDelivery(Drone drone)
        {
            lock (bl)
            {
                if (parcel.Equals(default)) init(drone.PInTransfer.PId); 
                distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);
            }

            if (distance < 0.01 || drone.BatteryStatus == 0.0)
                lock (bl)
                {
                    drone.CurrLocation = customer.Location;
                    if (pickedUp)
                    {
                        dal.ParcelDelivery((int)parcel?.Id);
                        drone.Status = DroneStatuses.Available;
                    }
                    else
                    {
                        dal.ParcelPickup((int)parcel?.Id);
                        customer = bl.GetCustomer((int)parcel?.TargetId);
                        pickedUp = true;
                    }
                }
            else
            {
                if (!sleepDelayTime()) break;
                lock (bl)
                {
                    double delta = distance < STEP ? distance : STEP;
                    double proportion = delta / distance;
                    drone.Battery = Max(0.0, drone.Battery - delta * bl.BatteryUsages[pickedUp ? batteryUsage : DRONE_FREE]);
                    double lat = drone.Location.Latitude + (customer.Location.Latitude - drone.Location.Latitude) * proportion;
                    double lon = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * proportion;
                    drone.Location = new() { Latitude = lat, Longitude = lon };
                }
            }
            break;
        }

        private static bool SleepDelayTime()
        {
            try
            {
                Thread.Sleep(DELAY);
            }
            catch (ThreadInterruptedException)
            {
                return false;
            }
            return true;
        }
    }
}

