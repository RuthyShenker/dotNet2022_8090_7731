using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public delegate void DelEventHandler();

    public class Refresh
    {
        public static event DelEventHandler refresh;

        public static event DelEventHandler DronesList;
        public static event DelEventHandler StationsList;
        public static event DelEventHandler CustomersList;
        public static event DelEventHandler ParcelsList;

        public static event DelEventHandler Parcel;
        public static event DelEventHandler Drone;
        public static event DelEventHandler Station;
        public static event DelEventHandler Customer;

        public static void Invoke()
        {
            refresh += new DelEventHandler(India);
            refresh += new DelEventHandler(England);

            //refresh.Invoke();
            DronesList?.Invoke();
            StationsList?.Invoke();
            CustomersList?.Invoke();
            ParcelsList?.Invoke();

            Customer?.Invoke();
            Station?.Invoke();
            Drone?.Invoke();
            Parcel?.Invoke();
        }

        //public static void USA()
        //{
        //    MessageBox.Show("USA");
        //    //Console.WriteLine("USA");
        //}

        public static void India()
        {
            MessageBox.Show("India");

            //Console.WriteLine("India");
        }

        public static void England()
        {
            MessageBox.Show("England");

        }


    }
}

