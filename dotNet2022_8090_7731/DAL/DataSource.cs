using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

namespace DalObject
{

    /// <summary>
    /// A class that contains:
    /// rand, DroneList, BaseStationList, CustomerList, ParceList, 
    /// ChargingDroneList, class Config and function-Initialize
    /// </summary>
    public class DataSource
    {
        const int INITIALIZE_DRONE = 5;
        const int INITIALIZE_CUSTOMER = 10;
        const int INITIALIZE_BASE_STATION = 2;
        const int INITIALIZE_PARCEL = 2;

        //  INITIALIZE_PARCEL always < INITIALIZE_DRONE ( for droneId in parcel initializing )
        //  INITIALIZE_CUSTOMER always >= 2 ( for two difference customers id in parcel )

        /// <summary>
        /// an object of Random .
        /// </summary>
        public static Random Rand = new Random();

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
        /// match list by type
        /// </summary>
        static internal Dictionary<Type, IList> data = new()
        {
            [typeof(Drone)] = DroneList,
            [typeof(Customer)] = CustomerList,
            [typeof(Parcel)] = ParceList,
            [typeof(BaseStation)] = BaseStationList,
            [typeof(ChargingDrone)] = ChargingDroneList,
        };

       
        /// <summary>
        /// A class Config that contains :
        /// IndexParcel,chargingRate,heavyWeight,mediumWeight,lightWeight,available
        /// </summary>
        internal class Config
        {
            public static double available = RandBetweenRange(0, 10);
            public static double lightWeight = RandBetweenRange(available, 20);
            public static double mediumWeight = RandBetweenRange(lightWeight, 30);
            public static double heavyWeight = RandBetweenRange(mediumWeight, 40);

            public static double chargingRate = RandBetweenRange(50, 80);

            internal static int IndexParcel;
        }
        
        public static double RandBetweenRange(double min, double max)
        {
            return Rand.NextDouble() * (max - min) + min;
        }
        
        /// <summary>
        /// innitialize drones, customers,base stations, parcels.
        /// </summary>

        //לשנות את האתחול של ID בכל האוביקטים
        public static void Initialize()
        {
            
            // Drones
            for (int i = 0; i < INITIALIZE_DRONE; ++i)
            {
                DroneList.Add(new Drone()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    Model = Rand.Next(1000, 10000).ToString(),
                    MaxWeight = (WeightCategories)Rand.Next(Enum.GetNames(typeof(WeightCategories)).Length),
                });
            }

            // Customers
            string[] initNames = { "Uria", "Aviad", "Odel", "Natan", "Or", "Keren" };
            string[] InitDigitsPhone = { "0556", "0548", "0583", "0533", "0527", "0522", "0505", "0584" };
            for (int i = 0; i < INITIALIZE_CUSTOMER; i++)
            {
                CustomerList.Add(new Customer()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    Name = initNames[Rand.Next(0, initNames.Length)],
                    Phone = InitDigitsPhone[Rand.Next(InitDigitsPhone.Length)] += Rand.Next(100000, 1000000).ToString(),
                    Longitude = RandBetweenRange(-180, 180),
                    Latitude = RandBetweenRange(-90, 90),
                });
            }

            // BaseStations
            string[] initNameStation = { "Tel-Tzion", "Tel-Aviv", "Ranana", "Eilat", "Jerusalem" };
            for (int i = 0; i < INITIALIZE_BASE_STATION; ++i)
            {
                BaseStationList.Add(new BaseStation()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    NameStation = initNameStation[Rand.Next(initNameStation.Length)],
                    NumberOfChargingPositions = Rand.Next(50),
                    Longitude = RandBetweenRange(-180, 180),
                    Latitude = RandBetweenRange(-90, 90),
                });
            }

            // Parcels
            Parcel parcel;
            for (int i = 0; i < INITIALIZE_PARCEL; ++i)
            {
                parcel = new Parcel() { Id = ++Config.IndexParcel };

                // diffierent Id
                parcel.SenderId = CustomerList[Rand.Next(CustomerList.Count) / 2].Id;
                parcel.GetterId = CustomerList[Rand.Next(CustomerList.Count / 2, CustomerList.Count)].Id;
                
                parcel.Weight = (WeightCategories)Rand.Next(Enum.GetNames(typeof(WeightCategories)).Length);
                parcel.MPriority = (UrgencyStatuses)Rand.Next(Enum.GetNames(typeof(UrgencyStatuses)).Length);
                parcel.MakingParcel = DateTime.Now;
                
                //fillParcel.DroneId = availableDrone();

                // rand assigning parcel
                //bool isAssigned = Rand.Next(2) == 0;
                bool isAssigned = i % 2 == 0;
                parcel.DroneId = isAssigned ? DroneList[i].Id : 0;
                parcel.BelongParcel = parcel.DroneId == 0 ? null : DateTime.Now;

                // rand if do the action, and register with match value
                parcel.PickingUp = !parcel.BelongParcel.HasValue || Rand.Next(2) == 0 ? null : parcel.BelongParcel.Value.AddDays(Rand.Next(0, 11));
                parcel.Arrival = !parcel.PickingUp.HasValue || Rand.Next(2) == 0 ? null : parcel.PickingUp.Value.AddDays(Rand.Next(0, 11));

                ParceList.Add(parcel);
            }
        }
    }
}

