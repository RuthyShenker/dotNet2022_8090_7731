using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using System.Runtime.CompilerServices;

namespace BL
{
    class Simulator
    {
        // maybe good for calculate distances
        // https://www.google.com/search?q=geography+location+and+calculate+distance+c%23&oq=geography+location+and+calculate+distance+c%23&aqs=chrome..69i57.22174j0j9&sourceid=chrome&ie=UTF-8
        class State
        {
            internal double remainDistance;
            internal bool pickedUp;
            internal int stationId;
            internal bool inWayToStation;
        }
        private const int DELAY = 40;
        private const double VELOCITY = 1.0;
        private const double TIME_STEP = DELAY / 5000.0;
        private const double STEP = VELOCITY / TIME_STEP;
        private BL bl;
        Action<object> updateView;
        Func<bool> checkStop;
        DO.Parcel parcel;
        Station station;
        Customer customer;
        DalApi.IDal dal;
        double distance;
        bool parcelPickedUp;
        int droneFree = 0;
        DroneToList drone;
        //double[] powerConsumption;

        //        powerConsumptionFree,
        //        powerConsumptionLight,
        //        powerConsumptionMedium,
        //        powerConsumptionHeavy,
        //        chargingRate
        //public enum Maintenance { Assigning, GoingTowardStation, Charging };

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Simulator(BL blInstance, int droneId, Action<object> updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            dal = blInstance.dal;
            updateView = updateViewAction;
            checkStop = checkStopFunc;
            lock (bl)
            {
                drone = bl.lDroneToList.First(d => d.Id == droneId);
            }

            do
            {
                switch (drone.DStatus)
                {
                    case DroneStatus.Free:
                        TryAssumingParcel();
                        break;
                    case DroneStatus.Maintenance:
                        CompleteMaintenance();
                        break;
                    case DroneStatus.Delivery:
                        CompleteDelivery();
                        break;
                    default:
                        break;
                }
            } while (!checkStop());
        }

        private void TryAssumingParcel()
        {
            if (!SleepDelayTime()) return;

            List<DO.Parcel> optionalParcels;
            lock (bl)
            {
                lock (dal)
                {
                    optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation).ToList();
                }
            }

            if (!optionalParcels.Any())
            {
                if (drone.BatteryStatus >= 100)
                {
                    while (!optionalParcels.Any() && !checkStop())
                    {
                        if (!SleepDelayTime()) break;
                        if (!SleepDelayTime()) break;
                        optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation).ToList();
                    }
                    return;
                }

                else
                {
                    lock (bl)
                    {
                        lock (dal)
                        {
                            station = bl.ClosestStation(drone.CurrLocation, true);

                            if (station.Id != default)
                            {
                                drone.DStatus = DroneStatus.Maintenance;
                                dal.Add(new DO.ChargingDrone() { DroneId = drone.Id, StationId = station.Id, EnteranceTime = DateTime.Now });

                                updateView(new State() { remainDistance = 0, pickedUp = false, stationId = station.Id, inWayToStation = true });

                                return;
                            }
                        }
                    }
                }
            }

            AssumingParcel(optionalParcels.First());
        }

        private void AssumingParcel(DO.Parcel matchParcel)
        {
            lock (bl)
            {
                lock (dal)
                {
                    parcel = matchParcel;
                    parcelPickedUp = parcel.PickingUp.HasValue;
                    customer = bl.GetCustomer(parcelPickedUp ? parcel.GetterId : parcel.SenderId);
                    distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);

                    drone.DStatus = DroneStatus.Delivery;
                    drone.DeliveredParcelId = parcel.Id;

                    dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(DO.Parcel.BelongParcel));
                    dal.Update<DO.Parcel>(parcel.Id, drone.Id, nameof(DO.Parcel.DroneId));

                    updateView(new State() { remainDistance=distance  , pickedUp = parcelPickedUp, stationId = 0, inWayToStation = false });
                }
            }
        }

        private void Init(int parcelId, DroneToList drone)
        {
            parcel = dal.GetFromDalById<DO.Parcel>(parcelId);
            parcelPickedUp = parcel.PickingUp.HasValue;
            customer = bl.GetCustomer(parcelPickedUp ? parcel.GetterId : parcel.SenderId);

            //why???
            distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);

            //batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
        }
        #region newPInTransferIfNeeded
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
        #endregion

        private void CompleteMaintenance()
        {
            if (!SleepDelayTime()) return; //TODO return?
                                           //TODO what happens when there is no available station
                                           // Assigning:
            lock (bl)
            {
                lock (dal)
                {
                    station = bl.ClosestStation(drone.CurrLocation, true);
                }
            }
            distance = Extensions.CalculateDistance(drone.CurrLocation, station.Location);
            updateView(new State() { remainDistance = distance, pickedUp = false, stationId = station.Id, inWayToStation = true });


            // Going toward station
            while (distance > 0.01 && !checkStop())
            {
                if (!SleepDelayTime()) break;
                lock (bl)
                {
                    GoAhead(station.Location, drone);
                    updateView(new State() { remainDistance = distance, pickedUp = false, stationId = station.Id, inWayToStation = true });
                }
            }

            if (distance <= 0.01)
            {
                drone.CurrLocation = station.Location;

                // charging 
                while (drone.BatteryStatus < 100 && !checkStop())
                {
                    if (!SleepDelayTime()) break;
                    lock (bl) // why בפרויקט לדוג עשו כאן?
                    {
                        drone.BatteryStatus = Math.Min(drone.BatteryStatus + (chargingRate * TIME_STEP), 100);
                        updateView(new State() { remainDistance = 0, pickedUp = false, stationId = station.Id, inWayToStation = false });
                    }
                }

                if (drone.BatteryStatus >= 100)
                {
                    drone.DStatus = DroneStatus.Free;
                    updateView(new State() { remainDistance = 0, pickedUp = false, stationId = 0, inWayToStation = false });

                }
            }
        }

        private void CompleteDelivery()
        {
            // what to do when battery finish?

            /* if (parcel.Equals(default)) */
            Init((int)drone.DeliveredParcelId, drone);

            while (distance > 0.01 && drone.BatteryStatus > 0 && !checkStop())
            {
                if (!SleepDelayTime()) break;
                lock (bl)
                {
                    GoAhead(customer.Location, drone);
                    updateView(new State() { remainDistance = distance, pickedUp = parcelPickedUp, stationId = 0, inWayToStation = false });
                }
            }
            lock (bl)
            {
                lock (dal)
                {
                    if ((distance <= 0.01 /*|| drone.BatteryStatus == 0.0*/) && !checkStop())
                    {
                        drone.CurrLocation = customer.Location;
                        if (parcelPickedUp)
                        {
                            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.Arrival));

                            drone.DStatus = DroneStatus.Free;
                            updateView(new State() { remainDistance = distance, pickedUp = parcelPickedUp, stationId = 0, inWayToStation = false });
                        }
                        else
                        {
                            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.PickingUp));
                            customer = bl.GetCustomer(parcel.GetterId);
                            parcelPickedUp = true;
                            updateView(new State() { remainDistance = 0, pickedUp = parcelPickedUp, stationId = 0, inWayToStation = false });
                        }
                    }
                }
            }
        }

        private void GoAhead(Location targetLocation, DroneToList drone)
        {
            double delta = distance < STEP ? distance : STEP;
            //distance = Extensions.CalculateDistance(drone.CurrLocation, targetLocation);

            double proportion = delta / distance;

            drone.BatteryStatus = Math.Max(0.0, drone.BatteryStatus - (delta * (parcelPickedUp ? PowerConsumptionForParcelWeight() : PowerConsumptionFree)));
            drone.CurrLocation = new()
            {
                Latitude = drone.CurrLocation.Latitude + ((targetLocation.Latitude - drone.CurrLocation.Latitude) * proportion),
                Longitude = drone.CurrLocation.Longitude + ((targetLocation.Longitude - drone.CurrLocation.Longitude) * proportion),
            };
            distance -= delta;
        }

        private double PowerConsumptionForParcelWeight()
        {
            return parcel.Weight == DO.WeightCategories.Light
                ? powerConsumptionLight
                : parcel.Weight == DO.WeightCategories.Medium
                ? powerConsumptionMedium
                : powerConsumptionHeavy;
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
