using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    /// <summary>
    /// A class that contains:
    /// rand, DroneList, BaseStationList, CustomerList, ParceList, 
    /// ChargingDroneList, class Config and function-Initialize
    /// </summary>
    internal class DataSource
    {
        const int INITIALIZE_DRONE = 5;
        const int INITIALIZE_CUSTOMER = 10;
        const int INITIALIZE_BASE_STATION = 2;
        const int INITIALIZE_PARCEL = 2;

        //  INITIALIZE_PARCEL always < INITIALIZE_DRONE ( for droneId in parcel initializing )
        //  INITIALIZE_CUSTOMER always >= 2 ( for two difference customers id in parcel )

        /// <summary>
        /// A class Config that contains :
        /// IndexParcel,chargingRate,heavyWeight,mediumWeight,lightWeight,available
        /// </summary>
        internal class Config
        {
            static public double Available = RandBetweenRange(0, 0.001);
            static public double LightWeight = RandBetweenRange(Available, 0.002);
            static public double MediumWeight = RandBetweenRange(LightWeight, 0.003);
            static public double HeavyWeight = RandBetweenRange(MediumWeight, 0.004);
            static public double ChargingRate = 20;//TODO לאתחל
            internal static int IndexParcel = 0;

            private static double RandBetweenRange(double min, double max)
            {
                return (Rand.NextDouble() * (max - min)) + min;
            }
        }

        

        /// <summary>
        /// an object of Random .
        /// </summary>
        internal static Random Rand;

        /// <summary>
        /// A list of Drones.
        /// </summary>
        static internal List<Drone> DroneList;

        /// <summary>
        /// A list of Base Stations.
        /// </summary>
        static internal List<BaseStation> BaseStationList;

        /// <summary>
        /// A list of Customers.
        /// </summary>
        static internal List<Customer> CustomerList;

        /// <summary>
        /// A list of Parcels.
        /// </summary>
        static internal List<Parcel> ParceList;

        /// <summary>
        /// A list of Charging Drones.
        /// </summary>
        static internal List<ChargingDrone> ChargingDroneList;

        /// <summary>
        /// match list by type
        /// </summary>
        static internal Dictionary<Type, IList> Data;

        static DataSource()
        {
            Rand = new Random();
            DroneList = new List<Drone>();
            BaseStationList = new List<BaseStation>();
            CustomerList = new List<Customer>();
            ParceList = new List<Parcel>();
            ChargingDroneList = new List<ChargingDrone>();
            Data = new()
            {
                [typeof(Drone)] = DroneList,
                [typeof(Customer)] = CustomerList,
                [typeof(Parcel)] = ParceList,
                [typeof(BaseStation)] = BaseStationList,
                [typeof(ChargingDrone)] = ChargingDroneList,
            };
        }

        /// <summary>
        /// innitialize drones, customers,base stations, parcels.
        /// </summary>
        internal static void Initialize()
        {
            InitializeDrones();

            InitializeCustomers();

            InitializeBaseStations();

            InitializeParcels();
        }

        private static void InitializeDrones()
        {
            for (int i = 0; i < INITIALIZE_DRONE; ++i)
            {
                DroneList.Add(new Drone()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    Model = $"Boing{Rand.Next(1000, 10000)}",
                    MaxWeight = (WeightCategories)Rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length),
                });
            }
        }

        private static void InitializeCustomers()
        {
            string[] initNames = { "Uria", "Aviad", "Odel", "Natan", "Or", "Keren" };
            string[] initDigitsPhone = { "0556", "0548", "0583", "0533", "0527", "0522", "0505", "0584" };
            for (int i = 0; i < INITIALIZE_CUSTOMER; i++)
            {
                CustomerList.Add(new Customer()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    Name = initNames[Rand.Next(0, initNames.Length)],
                    Phone = initDigitsPhone[Rand.Next(0, initDigitsPhone.Length)] += Rand.Next(100000, 1000000).ToString(),
                    Longitude = Rand.Next(-90, 90) + Rand.NextDouble(),
                    Latitude = Rand.Next(-90, 90) + Rand.NextDouble()
                });
            }
        }

        private static void InitializeBaseStations()
        {
            string[] initNameStation = { "Tel-Tzion", "Tel-Aviv", "Rahanana", "Eilat", "Jerusalem" };
            for (int i = 0; i < INITIALIZE_BASE_STATION; ++i)
            {
                BaseStationList.Add(new BaseStation()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    NameStation = initNameStation[Rand.Next(0, initNameStation.Length)],
                    NumberOfChargingPositions = Rand.Next(0, 50),
                    Longitude = Rand.Next(-90, 90) + Rand.NextDouble(),
                    Latitude = Rand.Next(-90, 90) + Rand.NextDouble()
                });
            }
        }

        private static void InitializeParcels()
        {
            Parcel newParcel;
            for (int i = 0; i < INITIALIZE_PARCEL; ++i)
            {
                newParcel = new Parcel() { Id = ++Config.IndexParcel };
                newParcel.SenderId = CustomerList[Rand.Next(CustomerList.Count)].Id;
                do
                {
                    newParcel.GetterId = CustomerList[Rand.Next(CustomerList.Count)].Id;
                } while (newParcel.GetterId == newParcel.SenderId);
                newParcel.Weight = (WeightCategories)Rand.Next(Enum.GetNames(typeof(WeightCategories)).Length);
                newParcel.MPriority = (UrgencyStatuses)Rand.Next(Enum.GetNames(typeof(UrgencyStatuses)).Length);
                newParcel.CreatedTime = DateTime.Now;
                if (i % 2 == 0)
                {
                    var availableDrone = DroneList.FirstOrDefault(d => d.MaxWeight >= newParcel.Weight
                                                                        && !ParceList.Any(p => p.DroneId == d.Id));
                    if (!availableDrone.Equals(default(Drone)))
                    {
                        newParcel.DroneId = availableDrone.Id;
                        newParcel.BelongParcel = DateTime.Now;
                        //fillParcel.PickingUp = fillParcel.BelongParcel.Value.AddDays(Rand.Next(0, 11));
                        //fillParcel.Arrival = fillParcel.PickingUp.Value.AddDays(Rand.Next(0, 11));
                    }
                }
                ParceList.Add(newParcel);
            }
        }

        //    Parcel fillParcel;
        //        for (int i = 0; i<INITIALIZE_PARCEL; ++i)
        //        {
        //            fillParcel = new Parcel() { ParcelId = ++Config.IndexParcel };
        //    fillParcel.SenderId = CustomerList[Rand.Next(0, CustomerList.Count)].Id.ToString();
        //            do
        //            {
        //                fillParcel.GetterId = CustomerList[Rand.Next(0, CustomerList.Count)].Id.ToString();
        //            } while (fillParcel.GetterId == fillParcel.SenderId);
        //            fillParcel.Weight = (WeightCategories) Rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length);
        //            fillParcel.Status = (UrgencyStatuses) Rand.Next(0, Enum.GetNames(typeof(UrgencyStatuses)).Length);
        //            fillParcel.DroneId = availableDrone();
        //fillParcel.MakingParcel = DateTime.Now;
        //            fillParcel.BelongParcel = fillParcel.DroneId == 0 ? new DateTime() :DateTime.Now;
        //            fillParcel.PickingUp = fillParcel.DroneId == 0 ? new DateTime() : fillParcel.BelongParcel.AddDays(Rand.Next(0, 11));
        //            //fillParcel.Arrival = fillParcel.DroneId == 0 ? new DateTime() : fillParcel.PickingUp.AddDays(rand.Next(0, 11));
        //            ParceList.Add(fillParcel);
        //        }
    }
}

