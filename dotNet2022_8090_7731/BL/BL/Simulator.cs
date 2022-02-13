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
        public enum Maintenance { Assigning, GoingTowardStation, Charging };

        public Simulator(BL blInstance, int droneId, Action updateViewAction, Func<bool> checkStopFunc)
        {
            bl = blInstance;
            var dal = blInstance.dal;
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
            } while (true);
        }

        private void TryAssumingParcel(int droneId)
        {

            // כאשר אין באפשרות הרחפן לקחת חבילה
            bl.SendingDroneToCharge(droneId);

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
