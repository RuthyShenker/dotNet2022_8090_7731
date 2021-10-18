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
        static internal BaseStation[] BaseStationArr = new BaseStation[5];
        static internal Customer[] CustomerArr = new Customer[100];
        static internal Parcel[] ParcelArr = new Parcel[1000];
        public BaseStation this[string Id]
        {
            get
            {

                foreach (BaseStation Station in BaseStationArr)
                {
                    if (Station.Id == Id)
                    {
                        return Station;
                    }
                }
                throw new Exception();
            }
            set
            {
                foreach (BaseStation Station in BaseStationArr)
                {
                    if (Station.Id == Id)
                    {
                         Station=value as BaseStation;
                    }
                }
                throw new Exception();
            }
        }

        internal class Config
        {
            internal static int IndexDroneArr;
            internal static int IndexBaseStationArr;
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
                fillDrone = DroneArr[Config.IndexDroneArr++];
                fillDrone.Id = rand.Next(100000000, 1000000000).ToString();
                fillDrone.Model = rand.Next(1000, 9999).ToString();
                fillDrone.MaxWeight = (WeightCategories)(rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length));
                fillDrone.BatteryStatus = rand.Next(0, 100);
                fillDrone.Status = (DroneStatuses)rand.Next(0, Enum.GetNames(typeof(DroneStatuses)).Length);
            }


            const int INITIALIZE_CUSTOMER = 10;
            string[] initNames = { "UriA", "Aviad", "Odel", "Natan", "Or", "Keren" };
            string[] InitDigitsPhone = { "0556", "0548", "0583", "0533", "0527", "0522", "0505", "0584" };

            Customer fillCustomer;
            for (int i = 0; i < INITIALIZE_CUSTOMER; i++)
            {
                fillCustomer = CustomerArr[Config.IndexCustomerArr++];
                fillCustomer.Id = rand.Next(100000000, 1000000000).ToString();
                fillCustomer.Name = initNames[rand.Next(0, initNames.Length)];
                fillCustomer.Phone = InitDigitsPhone[rand.Next(0, InitDigitsPhone.Length)] + rand.Next(100000, 1000000).ToString();
                //  fillCustomer.Longitude = /////////////////////////////////
                // fillCustomer.Latitude =///////////////////////////////////
            }


            BaseStation fillBaseStation;
            const int INITIALIZE_BASE_STATION = 2;
            string[] initNameStation = { "Tel-Tzion", "Tel-Aviv", "Ranana", "Eilat", "Jerusalem" };

            for (int i = 0; i < INITIALIZE_BASE_STATION; i++)
            {
                fillBaseStation = BaseStationArr[Config.IndexBaseStationArr++];
                fillBaseStation.Id = rand.Next(100000, 1000000).ToString();
                fillBaseStation.NameStation = initNameStation[rand.Next(0, initNameStation.Length)];
                fillBaseStation.NumberOfChargingStations = rand.Next(2, BaseStationArr.Length);
                fillBaseStation.Longitude = rand.Next(2, BaseStationArr.Length);
                fillBaseStation.Latitude = rand.Next(2, BaseStationArr.Length);

            }


            Parcel fillParcel;
            const int INITIALIZE_PARCEL = 11;

            for (int i = 0; i < INITIALIZE_PARCEL; i++)
            {
                fillParcel = ParcelArr[Config.IndexParcelArr++];
                fillParcel.ParcelId = rand.Next(100000, 1000000).ToString();
                fillParcel.SenderId = CustomerArr[rand.Next(0, Config.IndexCustomerArr)].Id;
                fillParcel.GetterId = CustomerArr[rand.Next(0, Config.IndexCustomerArr)].Id;
                fillParcel.Weight = (WeightCategories)rand.Next(0, Enum.GetNames(typeof(DAL.DO.WeightCategories)).Length);
                fillParcel.Status = (UrgencyStatuses)rand.Next(0, Enum.GetNames(typeof(DAL.DO.UrgencyStatuses)).Length);
                fillParcel.DroneId = rand.Next(0, Config.IndexDroneArr);
                //     fillParcel.MakingParcel = 
                //    fillParcel.PickingUp = 
                //   fillParcel.Arrival =
                //   fillParcel.MatchingParcel =
            }
        }
    }
}
