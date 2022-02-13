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
        int parcelId;
        int stationId;
        DalApi.IDal dal;
        public enum Maintenance { Assigning, GoingTowardStation, Charging };

        public Simulator(BL blInstance, int droneId, Action updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            dal = blInstance.dal;
            Drone drone = bl.GetDrone(droneId);
            updateView = updateViewAction;
            checkStop = checkStopFunc;

            do
            {
                switch (drone.DroneStatus)
                {
                    case DroneStatus.Free:
                        TryAssumingParcel(droneId);
                        break;
                    case DroneStatus.Maintenance:
                        CompleteCharging(drone);
                        break;
                    case DroneStatus.Delivery:
                        CompleteDelivery(droneId);
                        break;
                    default:
                        break;
                }
            } while (!checkStop());
        }

        private void TryAssumingParcel(Drone drone)
        {
            ////    if (!SleepDelayTime())
            ////        break;

            //lock (bl)
            //{
            //    bl.BelongingParcel(drone.Id);

            //    // כאשר אין באפשרות הרחפן לקחת חבילה
            //    bl.SendingDroneToCharge(droneId);






            //}

            if (!SleepDelayTime()) break;

            lock (bl)
            {
                var optionalParcels = bl.OptionalParcelsForSpecificDrone(drone.BatteryStatus, drone.Weight, drone.CurrLocation);
                if (optionalParcels.Any())
                {
                    var parcelId = optionalParcels.First().Id;

                    switch (parcelId, drone.BatteryStatus)
                    {

                        // if there is no parcel to assign and drone's battery is full.  
                        case (0, 1.0):
                            break;
                        // if the is no parcel to assign and the the drone's battery is not full.
                        case (0, _):
                            var station = bl.ClosestStation(drone.CurrLocation, true);
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
                            try
                            {
                                dal.Update<DO.Parcel>(parcelId, DateTime.Now, nameof(DO.Parcel.BelongParcel));
                                dal.Update<DO.Parcel>(parcelId, drone.Id, nameof(DO.Parcel.DroneId));
                                var requiredDrone= bl.lDroneToList.Find(d => d.Id == drone.Id);
                                requiredDrone.DeliveredParcelId = parcelId;
                                initDelivery((int)parcelId);
                                requiredDrone.DStatus = DroneStatus.Delivery;
                            }
                            catch (DO.ExistIdException ex) { throw new BadStatusException("Internal error getting parcel", ex); }
                            break;
                    }
                }
            }
            break;





        }
        private void CompleteCharging(Drone drone)
        {
            switch (maintenance)
            {
                case Maintenance.Assigning:
                    Station station = bl.ClosestStation(drone.CurrLocation, true);
                    break;
                case Maintenance.GoingTowardStation:
                    break;
                case Maintenance.Charging:
                    break;
                default:
                    break;
            }
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
        }

        private void CompleteDelivery(int droneId)
        {
            throw new NotImplementedException();
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

