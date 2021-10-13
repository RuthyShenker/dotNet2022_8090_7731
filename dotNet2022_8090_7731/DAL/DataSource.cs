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
        static internal BaseStation[] BasicStationArr = new BaseStation[5];
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
            const int INITIALIZE_DRONE = 5;
            Drone fillDrone;
            for (int i = 0; i < INITIALIZE_DRONE; i++)
            {
                fillDrone = DroneArr[Config.IndexDroneArr];
                fillDrone.Id = rand.Next(100000000, 999999999).ToString();
                fillDrone.Model = rand.Next(1000, 9999).ToString();
                fillDrone.MaxWeight = (WeightCategories)(rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length));
                fillDrone.BatteryStatus = rand.Next(0, 100);
                fillDrone.Status = (DroneStatuses)(rand.Next(0, Enum.GetNames(typeof(DroneStatuses)).Length));
                Config.IndexDroneArr++;
            }


            const int INITIALIZE_CUSTOMER = 10;
            string[] nameForInitialize = {"UriA","Aviad","Ariel",};
            Customer fillCustomer;
            for (int i = 0; i < INITIALIZE_CUSTOMER; i++)
            {
                fillCustomer = CustomerArr[Config.IndexCustomerArr];
                fillCustomer.Id = rand.Next(100000000, 999999999).ToString();
                fillCustomer.Model = rand.Next(1000, 9999).ToString();
                fillCustomer.MaxWeight = (WeightCategories)(rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length));
                fillCustomer.BatteryStatus = rand.Next(0, 100);
                fillCustomer.Status = (DroneStatuses)(rand.Next(0, Enum.GetNames(typeof(DroneStatuses)).Length));
            }

             public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
            //DroneArr[ rand.Next(DroneArr.Length)]=(64964,h9h);
        }
    }
}
