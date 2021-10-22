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

        static internal List<Drone> DroneList = new List<Drone>();
        static internal List<BaseStation> BaseStationList = new List<BaseStation>();
        static internal List<Customer> CustomerList = new List<Customer>();
        static internal List<Parcel> ParceList = new List<Parcel>();
        static internal List<ChargingDrone> ChargingDroneList = new List<ChargingDrone>();


        //public BaseStation this[string Id]
        //{
        //    get
        //    {



        //        public BaseStation this[string Id]
        //{
        //    get
        //    {
        //        foreach (BaseStation baseStation in BaseStationArr)
        //        {
        //            if (baseStation.Id == Id)
        //            {
        //                return baseStation;
        //            }
        //        }
        //        throw new Exception("There isn't baseStation with this Id");
        //    }
        //    set
        //    {
        //        for (int i = 0; i < BaseStationArr.Length; i++)
        //        {
        //            if (BaseStationArr[i].Id == Id)
        //            {
        //                BaseStationArr[i] = value;
        //            }
        //        }
        //    }
        //}
        //public Drone this[string Id]
        //{
        //    get
        //    {
        //        foreach (Drone drone in DroneArr)
        //        {
        //            if (drone.Id == Id)
        //            {
        //                return drone;
        //            }
        //        }
        //        throw new Exception("There isn't Drone with this Id");
        //    }
        //    set
        //    {
        //        for (int i = 0; i < DroneArr.Length; i++)
        //        {
        //            if (DroneArr[i].Id == Id)
        //            {
        //                DroneArr[i] = value;
        //            }
        //        }
        //    }
        //}
        //        {
        //            get
        //            {

        //                foreach (BaseStation Station in BaseStationArr)
        //                {
        //                    if (Station.Id == Id)
        //                    {
        //                        return Station;
        //                    }
        //                }
        //                throw new Exception();
        //            }
        //            set
        //            {
        //                foreach (BaseStation Station in BaseStationArr)
        //                {
        //                    if (Station.Id == Id)
        //                    {
        //                        Station = value as BaseStation;
        //                    }
        //                }
        //                throw new Exception();
        //            }
        //        }

        internal class Config
        {
            internal static int IndexParcel = 0;
        }
        public static void Initialize()
        {
            const int INITIALIZE_DRONE = 5, INITIALIZE_CUSTOMER = 10, INITIALIZE_BASE_STATION = 2, INITIALIZE_PARCEL = 11;

            Drone fillDrone;
            for (int i = 0; i < INITIALIZE_DRONE; ++i)
            {
                fillDrone = new Drone() { Id = rand.Next(100000000, 1000000000) };
                fillDrone.Model = rand.Next(1000, 10000).ToString();
                fillDrone.MaxWeight = (WeightCategories)rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length);
                fillDrone.BatteryStatus = rand.Next(0, 101);
                fillDrone.Status = (DroneStatuses)rand.Next(0, Enum.GetNames(typeof(DroneStatuses)).Length);
                DroneList.Add(fillDrone);
            }

            Customer fillCustomer;
            string[] initNames = { "UriA", "Aviad", "Odel", "Natan", "Or", "Keren" };
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
                fillParcel.DroneId = DroneList[rand.Next(0, DroneList.Count)].Id;
                fillParcel.MakingParcel = DateTime.Now;
                fillParcel.BelongParcel = fillParcel.MakingParcel.AddDays(rand.Next(0, 4));
                fillParcel.PickingUp = fillParcel.BelongParcel.AddDays(rand.Next(0, 11));
                fillParcel.Arrival = fillParcel.PickingUp.AddDays(rand.Next(0, 11));
                
                ParceList.Add(fillParcel);

            }
        }
    }
}
