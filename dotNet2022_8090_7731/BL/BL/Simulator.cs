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

        private const int DELAY = 500;
        private const double VELOCITY = 1.0;
        private const double TIME_STEP = DELAY / 3000.0;
        private const double STEP = VELOCITY / TIME_STEP;
        private BL bl;
        Action updateView;
        Func<bool> checkStop;
        DO.Parcel parcel;
        Station station;
        Customer customer;
        DalApi.IDal dal;
        double distance;
        bool pickedUp;
        int droneFree = 0;
        //double[] powerConsumption;

        //        powerConsumptionFree,
        //        powerConsumptionLight,
        //        powerConsumptionMedium,
        //        powerConsumptionHeavy,
        //        chargingRate
        //public enum Maintenance { Assigning, GoingTowardStation, Charging };

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Simulator(BL blInstance, int droneId, Action updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            dal = blInstance.dal;
            DroneToList drone = bl.lDroneToList.First(d => d.Id == droneId);
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
                    if (drone.BatteryStatus >= 1.0)
                    {
                        while (!optionalParcels.Any() && !checkStop())
                        {
                            drone.DStatus = DroneStatus.Free;
                            updateView();
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
                            dal.Add(new DO.ChargingDrone() { DroneId = drone.Id, StationId = station.Id, EnteranceTime = DateTime.Now });
                            bl.GetDrone(drone.Id);
                            updateView();
                            return;
                        }
                    }
                }

                parcel = optionalParcels.First();
                Init(parcel.Id, drone);
                drone.DStatus = DroneStatus.Delivery;

                dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(DO.Parcel.BelongParcel));
                dal.Update<DO.Parcel>(parcel.Id, drone.Id, nameof(DO.Parcel.DroneId));
            }
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

        private void CompleteMaintenance(DroneToList drone)
        {
            if (!SleepDelayTime()) return; //TODO return?
                                           //TODO what happens when there is no available station
            // Assigning:
            station = bl.ClosestStation(drone.CurrLocation, true);
            distance = Extensions.CalculateDistance(drone.CurrLocation, station.Location);
            updateView();

            //lock (bl)
            {
                // Going toward station
                while (distance > 0.01 && !checkStop())
                {
                    if (!SleepDelayTime()) break;

                    GoAhead(station.Location, drone);
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
                            drone.BatteryStatus = Math.Min(1.0, drone.BatteryStatus + (chargingRate * TIME_STEP));
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

        private void Init(int parcelId, DroneToList drone)
        {
            parcel = dal.GetFromDalById<DO.Parcel>(parcelId);
            pickedUp = parcel.PickingUp.HasValue;
            customer = bl.GetCustomer(pickedUp ? parcel.GetterId : parcel.SenderId);

            distance = Extensions.CalculateDistance(drone.CurrLocation, customer.Location);

            //batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
        }

        private void CompleteDelivery(DroneToList drone)
        {
           /* if (parcel.Equals(default)) */Init((int)drone.DeliveredParcelId, drone);

            while (distance > 0.01 && drone.BatteryStatus > 0 && !checkStop())
            {
                if (!SleepDelayTime()) break;
                lock (bl)
                {
                    GoAhead(customer.Location, drone);
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
                updateView();
            }
        }

        private void GoAhead(Location targetLocation, DroneToList drone)
        {
            double delta = distance < STEP ? distance : STEP;
            distance -= delta;
            double proportion = delta / distance;

            drone.BatteryStatus = Math.Max(0.0, drone.BatteryStatus - (delta * (pickedUp ? PowerConsumptionForParcelWeight() : PowerConsumptionFree)));
            drone.CurrLocation = new()
            {
                Latitude = drone.CurrLocation.Latitude + ((targetLocation.Latitude - drone.CurrLocation.Latitude) * proportion),
                Longitude = drone.CurrLocation.Longitude + ((targetLocation.Longitude - drone.CurrLocation.Longitude) * proportion),
            };
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
