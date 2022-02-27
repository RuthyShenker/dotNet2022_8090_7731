using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public delegate void DelEventHandler();

    /// <summary>
    /// Responsible to Refresh all the entities:
    /// </summary>
    public class Refresh
    {
        public static event DelEventHandler DronesList;
        public static event DelEventHandler StationsList;
        public static event DelEventHandler CustomersList;
        public static event DelEventHandler ParcelsList;

        public static event DelEventHandler Parcel;
        public static event DelEventHandler Drone;
        public static event DelEventHandler Station;
        public static event DelEventHandler Customer;

        /// <summary>
        /// Dictionary of all the workers:
        /// </summary>
        public static Dictionary<int, BackgroundWorker> Workers = new();


        public static void Invoke()
        {
            DronesList?.Invoke();
            StationsList?.Invoke();
            CustomersList?.Invoke();
            ParcelsList?.Invoke();

            Customer?.Invoke();
            Station?.Invoke();
            Drone?.Invoke();
            Parcel?.Invoke();
        }

    }
}

