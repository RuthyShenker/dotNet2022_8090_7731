using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;



namespace ConsoleUI_BL
{
    partial class Program
    {
        public static void AddingOption()
        {

            int input = 0;
            Console.WriteLine($"1 - Adding a base station to the stations list.\n" +
                              $"2 - Adding a drone to the drones list.\n" +
                              $"3 - Admission of a new customer to the customer list.\n" +
                              $"4 - Receipt of package for shipment.");
            CheckValids.CheckValid(1, 4, out input);
            switch (input)
            {
                case 1:
                    bL.AddingBaseStation(GettingNewBaseStation());
                    break;
                case 2:
                    int stationId;
                    bL.AddingDrone(GettingNewDrone(out stationId), stationId);
                    break;
                case 3:
                    bL.AddingCustomer(GettingNewCustomer());
                    break;
                case 4:
                    bL.AddingParcel(GettingNewParcel());
                    break;
                default:
                    break;

            }
        }

        private static Parcel GettingNewParcel()
        {
            Console.WriteLine("Enter the id of the sender of the new parcel: ");
            int senderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the id of the getter of the new parcel: ");
            int getterId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the weight of the new parcel: 0-Light, 1-Medium, 2-Heavy ");
            WeightCategories weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the priority of the new parcel: 0-Normal, 1-Fast, 2-Emergency");
            Priority mPriority = (Priority)int.Parse(Console.ReadLine());
            DroneInParcel DInParcel = null;

            return new Parcel(senderId, getterId, weight, mPriority, DInParcel, MakingParcel, BelongParcel, PickingUp, Arrival);
        }

        private static Customer GettingNewCustomer()
        {
            Console.WriteLine("Enter the id of the new customer: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the new customer: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the phone of the new customer: ");
            string phone = Console.ReadLine();
            //Console.WriteLine("Enter the location of new customer: ");
            //Console.WriteLine("longitude: ");
            //double longitude = double.Parse(Console.ReadLine());
            //Console.WriteLine("latitude: ");
            //double latitude = double.Parse(Console.ReadLine());
            Location cLocation = null;
            return new Customer(id, name, phone, cLocation);

        }


        private static Station GettingNewBaseStation()
        {
            Console.WriteLine("Enter the id of the new base station: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the new base station: ");
            string nameStation = Console.ReadLine();
            Console.WriteLine("Enter the Location of the new base station: ");
            Console.WriteLine("longitude: ");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("latitude: ");
            double latitude = double.Parse(Console.ReadLine());
            Location sLocation = new Location(longitude, latitude);
            Console.WriteLine("Enter the number of positions of the new base station: ");
            int numAvailablePositions = int.Parse(Console.ReadLine());
            List<BL_ChargingDrone> lBL_ChargingDrone = new List<BL_ChargingDrone>();
            return new Station(id, nameStation, sLocation, numAvailablePositions, lBL_ChargingDrone);
        }

        private static Drone GettingNewDrone(out int stationId)
        {
            Console.WriteLine("Enter the serial number of the manufacturer of the new drone: ");
            int id = int.Parse(Console.ReadLine());
            //Console.WriteLine("enter the model of the new drone: ");
            //int model = int.Parse(Console.ReadLine());
            int model = 0;
            Console.WriteLine("enter the weight of the new drone: ");
            WeightCategories weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of the station to charge the new drone for first charging:  ");
            stationId = int.Parse(Console.ReadLine());
            float batteryStatus = DalObject.DataSource.rand.Next(20, 41);
            DroneStatus droneStatus = DroneStatus.Maintenance;
            ParcelInTransfer pInTransfer = null;
            Location currLocation = null;
            return new Drone(id, model, weight, batteryStatus, droneStatus, pInTransfer, currLocation);
        }

        //public static BaseStation GettingBaseStation()
        //{

        //    int iInput;
        //    double dInput;
        //    Console.WriteLine("Enter The Id Of The Station:");
        //    int id = int.Parse(Console.ReadLine());
        //    Console.WriteLine("Enter The Name Of The Station:");
        //    string nameStation = Console.ReadLine();

        //    Console.WriteLine("Enter The Location. Longitude:");
        //    CheckValids.InputValiDoubleNum(out dInput, 90);
        //    double longitude = (double)dInput;

        //    Console.WriteLine("Latitude:");
        //    CheckValids.InputValiDoubleNum(out dInput, 180);
        //    double latitude = (double)dInput;

        //    Console.WriteLine("Enter The Number Of The Charging Stations:");
        //    CheckValids.CheckValid(1, 50, out iInput);
        //    int numberOfChargingStations = iInput;

        //    Location location = new Location(longitude, latitude);
        //    List<BL_ChargingDrone> lBL_ChargingDrone = new List<BL_ChargingDrone>();

        //    return new BL_Station(id, nameStation, location, numberOfChargingStations, lBL_ChargingDrone);
        //}

        /// <summary>
        /// A function that gets from the user Id and returns it as string
        /// </summary>
        /// <param name="obj">a name of object</param>
        /// <returns>an id as string</returns>
        private static string GettingIdAsString(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return Console.ReadLine();
        }
    }
}


