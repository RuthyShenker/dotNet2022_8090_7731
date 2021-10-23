using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;

namespace DalObject
{

    /// <summary>
    /// A class that contains:
    /// rand, DroneList, BaseStationList, CustomerList, ParceList, 
    /// ChargingDroneList, class Config and function-Initialize
    /// </summary>
    class DataSource
    {
        /// <summary>
        /// an object of Random .
        /// </summary>
        public static Random rand = new Random();

        /// <summary>
        /// A list of Drones.
        /// </summary>
        static internal List<Drone> DroneList = new List<Drone>();

        /// <summary>
        /// A list of Base Stations.
        /// </summary>
        static internal List<BaseStation> BaseStationList = new List<BaseStation>();

        /// <summary>
        /// A list of Customers.
        /// </summary>
        static internal List<Customer> CustomerList = new List<Customer>();

        /// <summary>
        /// A list of Parcels.
        /// </summary>
        static internal List<Parcel> ParceList = new List<Parcel>();

        /// <summary>
        /// A list of Charging Drones.
        /// </summary>
        static internal List<ChargingDrone> ChargingDroneList = new List<ChargingDrone>();

        /// <summary>
        /// A class Config that contains :
        /// IndexParcel
        /// </summary>
        internal class Config
        {
            internal static int IndexParcel = 0;
        }
        /// <summary>
        /// A function that Initializes the program with
        ///  5 drones, 10 customer, 2 base stations , 11 parcels.
        /// </summary>
        public static void Initialize()
        {
            const int INITIALIZE_DRONE = 5, INITIALIZE_CUSTOMER = 10,
                INITIALIZE_BASE_STATION = 2, INITIALIZE_PARCEL = 2;

            Drone fillDrone;
            for (int i = 0; i < INITIALIZE_DRONE; ++i)
            {
                fillDrone = new Drone() { Id = rand.Next(100000000, 1000000000) };
                fillDrone.Model = rand.Next(1000, 10000).ToString();
                fillDrone.MaxWeight = (WeightCategories)rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length);
                fillDrone.BatteryStatus = rand.Next(0, 101);
                fillDrone.Status = (DroneStatuses)rand.Next(0, Enum.GetNames(typeof(DroneStatuses)).Length-1);
                DroneList.Add(fillDrone);
            }

            Customer fillCustomer;
            string[] initNames = { "Uria", "Aviad", "Odel", "Natan", "Or", "Keren" };
            string[] InitDigitsPhone = { "0556", "0548", "0583", "0533", "0527", "0522", "0505", "0584" };
            for (int i = 0; i < INITIALIZE_CUSTOMER; i++)
            {
                fillCustomer=new Customer() { Id = rand.Next(100000000, 1000000000).ToString()};
                fillCustomer.Name = initNames[rand.Next(0, initNames.Length)];
                fillCustomer.Phone = InitDigitsPhone[rand.Next(0, InitDigitsPhone.Length)];
                fillCustomer.Phone += rand.Next(100000, 1000000).ToString();
                fillCustomer.Longitude = rand.Next(0, 90) + rand.NextDouble();
                fillCustomer.Latitude = rand.Next(0, 180) + rand.NextDouble();
                CustomerList.Add(fillCustomer);
            }

            BaseStation fillBaseStation;
            string[] initNameStation = { "Tel-Tzion", "Tel-Aviv", "Ranana", "Eilat", "Jerusalem" };
            for (int i = 0; i < INITIALIZE_BASE_STATION; ++i)
            {
                fillBaseStation=new BaseStation() { Id = rand.Next(100000000, 1000000000) };
                fillBaseStation.NameStation = initNameStation[rand.Next(0, initNameStation.Length)];
                fillBaseStation.NumAvailablePositions = rand.Next(0, 50);
                fillBaseStation.Longitude = rand.Next(rand.Next(0, 90)) + rand.NextDouble();
                fillBaseStation.Latitude = rand.Next(rand.Next(0, 180)) + rand.NextDouble();
                BaseStationList.Add(fillBaseStation);
            }

            Parcel fillParcel ;
            for (int i = 0; i < INITIALIZE_PARCEL; ++i)
            {
                fillParcel = new Parcel() { ParcelId = ++Config.IndexParcel };
                fillParcel.SenderId = CustomerList[rand.Next(0, CustomerList.Count)].Id.ToString();
                do
                {
                    fillParcel.GetterId = CustomerList[rand.Next(0, CustomerList.Count)].Id.ToString();
                } while (fillParcel.GetterId == fillParcel.SenderId);
                fillParcel.Weight = (WeightCategories)rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length);
                fillParcel.Status = (UrgencyStatuses)rand.Next(0, Enum.GetNames(typeof(UrgencyStatuses)).Length);
                fillParcel.DroneId = availableDrone(); 
                fillParcel.MakingParcel = DateTime.Now;
                fillParcel.BelongParcel = fillParcel.DroneId == 0 ? new DateTime():DateTime.Now;
                fillParcel.PickingUp = fillParcel.DroneId == 0 ?new DateTime(): fillParcel.BelongParcel.AddDays(rand.Next(0, 11));
                fillParcel.Arrival = fillParcel.DroneId == 0 ? new DateTime() : fillParcel.PickingUp.AddDays(rand.Next(0, 11));
                ParceList.Add(fillParcel);
            }
        }

        private static int availableDrone()
        {
            foreach (Drone drone in DroneList)
            {
                if(drone.Status==DroneStatuses.Available)
                {
                    return drone.Id;
                    DalObject dalObject = new DalObject();
                    dalObject.ChangeDroneStatus(drone.Id, DroneStatuses.Delivery);
                }
            }
            return 0;
        }
    }
}
