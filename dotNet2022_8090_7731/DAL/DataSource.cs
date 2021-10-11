using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;


namespace DalObject
{

    class DataSource
    {
        public static Random rand = new Random();

        static internal Drone[] DroneArr = new Drone[10];
        static internal BasicStation[] BasicStationArr = new BasicStation[5];
        static internal Customer[] CustomerArr = new Customer[100];
        static internal Parcel[] ParcelArr = new Parcel[1000];


        internal class Config
        {
            internal static int IndexDroneArr;
            internal static int IndexBasicStationArr;
            internal static int IndexCustomerArr;
            internal static int IndexParcelArr;
            //there is another field!!
        }
        public static void Initialize()
        {
            DroneArr[ rand.Next(DroneArr.Length)]=(64964,h9h);
        }
    }
}
