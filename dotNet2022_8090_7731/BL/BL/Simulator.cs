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
        //double[] powerConsumption;
        int droneFree = 0;

        //        powerConsumptionFree,
        //        powerConsumptionLight,
        //        powerConsumptionMedium,
        //        powerConsumptionHeavy,
        //        chargingRate
        //public enum Maintenance { Assigning, GoingTowardStation, Charging };

        public Simulator(BL blInstance, int droneId, Action updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            dal = blInstance.dal;
            //powerConsumption = bl.GetPowerConsumption().ToArray();
          DroneToList drone = bl.lDroneToList.First(d => d.Id == droneId);
            
            //Drone drone = bl.GetDrone(droneId);
            updateView = updateViewAction;
            checkStop = checkStopFunc;

            do
            {
                switch (drone.DStatus)
                {
                    case DroneStatus.Free:
                        TryAssumingParcel(drone);
                        break;
                    case DroneStatus.Maintenance:
                        CompleteMaintenance(drone);
                        break;
                    case DroneStatus.Delivery:
                        CompleteDelivery(drone);
                        break;
                    default:
                        break;
                }
            } while (!checkStop());
        }

        private void TryAssumingParcel(DroneToList drone)
        {
            lock (bl)
            {
                DO.Parcel parcel;
                List<DO.Parcel> optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation).ToList();

                if (!optionalParcels.Any())
                {
                    if (drone.BatteryStatus == 1.0)
                    {
                        while (!optionalParcels.Any() && !checkStop())
                        {
                            if (!SleepDelayTime()) break;
                            optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation).ToList();
                        }
                    }
                    else
                    {
                        station = bl.ClosestStation(drone.CurrLocation, true);

                        if (station.Id != default)
                        {
                            drone.DStatus = DroneStatus.Maintenance;
                            dal.Add(new DO.ChargingDrone(drone.Id, station.Id, DateTime.Now));
                            bl.GetDrone(drone.Id);
                            updateView();
                            return;
                        }
                    }
                }

                parcel = (optionalParcels.First());
                Init((int)(parcel.Id));
                drone.DStatus = DroneStatus.Delivery;

                dal.Update<DO.Parcel>((int)(parcel.Id), DateTime.Now, nameof(DO.Parcel.BelongParcel));
                dal.Update<DO.Parcel>((int)(parcel.Id), drone.Id, nameof(DO.Parcel.DroneId));
                ////if (optionalParcels.Any())
                //{
                //    if (!SleepDelayTime()) return;

                //    switch (parcel, drone.BatteryStatus)
                //    {
                //        // if there is no parcel to assign and drone's battery is full.  
                //        case (null, 1.0):
                //            // TODO
                //            while (!optionalParcels.Any() && !checkStop()) 
                //            {
                //                optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation).ToList();
                //                if (!SleepDelayTime()) return;
                //            }
                //            break;

                //        // if the is no parcel to assign and the the drone's battery is not full.
                //        case (null, _):
                //            station = bl.ClosestStation(drone.CurrLocation, true);

                //            if (station.Id != default)
                //            {
                //                drone.DroneStatus = DroneStatus.Maintenance;

                //                dal.Update<DO.BaseStation>(station.Id, station.NumAvailablePositions - 1, nameof(station.NumAvailablePositions));
                //                dal.Add(new DO.ChargingDrone(drone.Id, station.Id, DateTime.Now));
                //            }
                //            break;

                //        // if there is parcel to assign and there is enough battery.
                //        case (_, _):
                //            Init((int)(parcel?.Id));
                //            drone.DroneStatus = DroneStatus.Delivery;

                //            dal.Update<DO.Parcel>((int)(parcel?.Id), DateTime.Now, nameof(DO.Parcel.BelongParcel));
                //            dal.Update<DO.Parcel>((int)(parcel?.Id), drone.Id, nameof(DO.Parcel.DroneId));
                //            break;

                //        default:
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
        private void CompleteMaintenance(DroneToList drone)
        {
            if (!SleepDelayTime()) return; //TODO return?
                                           //TODO what happens when there is no available station

            // Assigning:
            station = bl.ClosestStation(drone.CurrLocation, true);
            distance = Extensions.CalculateDistance(drone.CurrLocation, station.Location);
            updateView();

            // Going toward station
            //lock (bl)
            {
                while (distance > 0.01 && !checkStop())
                {
                    if (!SleepDelayTime()) break;
                    double delta = distance < STEP ? distance : STEP;
                    distance -= delta;
                    drone.BatteryStatus = Math.Max(0.0, drone.BatteryStatus - delta * powerConsumptionFree);
                    updateView();
                }
                if (distance <= 0.01)
                {
                    drone.CurrLocation = station.Location;

                    // charging
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
                        drone.DStatus = DroneStatus.Free;
                        updateView();
                    }
                }
            }
        }

        private void Init(int parcelId)
        {
            parcel = dal.GetFromDalById<DO.Parcel>(parcelId);
            pickedUp = parcel.PickingUp.HasValue;
            customer = bl.GetCustomer(pickedUp ? parcel.GetterId : parcel.SenderId);

            //distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);

            //batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
        }

        private void CompleteDelivery(DroneToList drone)
        {
            if (parcel.Equals(default)) Init((int)drone.DeliveredParcelId);

            while (distance > 0.01 && drone.BatteryStatus != 0 && !checkStop())
            {
                if (!SleepDelayTime()) break;
                lock (bl)
                {
                    double delta = distance < STEP ? distance : STEP;
                    double proportion = delta / distance;
                    drone.BatteryStatus = Math.Max(0.0, drone.BatteryStatus - delta * (pickedUp ? powerConsumptionFree : powerConsumptionFree));
                    double lat = drone.CurrLocation.Latitude + (customer.Location.Latitude - drone.CurrLocation.Latitude) * proportion;
                    double lon = drone.CurrLocation.Longitude + (customer.Location.Longitude - drone.CurrLocation.Longitude) * proportion;
                    drone.CurrLocation = new() { Latitude = lat, Longitude = lon };

                    distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);
                    updateView();
                }
            }
            if ((distance <= 0.01 || drone.BatteryStatus == 0.0) && !checkStop())
            {
                drone.CurrLocation = customer.Location;
                if (pickedUp)
                {
                    dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.Arrival));
                    drone.DStatus = DroneStatus.Free;
                }
                else
                {
                    dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.PickingUp));

                    customer = bl.GetCustomer(parcel.GetterId);
                    pickedUp = true;
                }
            }
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


