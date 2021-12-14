//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using IDAL.DO;
//using DalObject;

//namespace ConsoleUI
//{
//    /// <summary>
//    /// A class contains:
//    /// GettingParcel
//    ///GettingCustomer
//    ///GettingDrone
//    ///GettingBaseStation
//    /// </summary>
//    class InputObj
//    {
//        /// <summary>
//        /// A function that gets from the user details of new Parcel
//        /// </summary>
//        /// <returns>The new parcel</returns>
//        public static Parcel GettingParcel()
//        {
//            Console.WriteLine("Enter The Id Of The Sender: ");
//            string senderId = Console.ReadLine();
//            Console.WriteLine("Enter The Id Of The Getter: ");
//            string getterId = Console.ReadLine();

//            Console.WriteLine("Enter The Weight Of The Parcel: ");
//            int input;
//            CheckValids.InputValidWeightCategories(out input);
//            WeightCategories weight = (WeightCategories)input;

//            Console.WriteLine("Enter The Status Of The Parcel:");
//            CheckValids.InputValidUrgencyStatuses(out input);
//            UrgencyStatuses status = (UrgencyStatuses)input;

//            DateTime makingParcel = DateTime.Now;
//            DateTime belongParcel = new DateTime();
//            DateTime pickingUp = new DateTime();
//            DateTime arrival = new DateTime();
//            return new Parcel(senderId, getterId, weight, status, makingParcel, belongParcel, pickingUp, arrival);
//        }
//        /// <summary>
//        /// A function that gets from the user details of new customer
//        /// </summary>
//        /// <returns>The new customer</returns>
//        public static Customer GettingCustomer()
//        {
//            double dInput;
//            Console.WriteLine("Enter The Id Of The Customer:");
//            string id = Console.ReadLine();
//            Console.WriteLine("Enter The Name Of The Customer:");
//            string name = Console.ReadLine();
//            Console.WriteLine("Enter The Phone Of The Customer:");
//            string phone = Console.ReadLine();

//            Console.WriteLine("Enter The Longitude:");
//            CheckValids.InputValiDoubleNum(out dInput, 90);
//            double longitude = (double)dInput;

//            Console.WriteLine("Enter the Latitude:");
//            CheckValids.InputValiDoubleNum(out dInput, 180);
//            double latitude = (double)dInput;

//            return new Customer(id, name, phone, longitude, latitude);
//        }
//        /// <summary>
//        /// A function that  gets from the user details of new drone
//        /// </summary>
//        /// <returns>The new drone</returns>
//        public static Drone GettingDrone()
//        {
//            int iInput;
//            double dInput;
//            Console.WriteLine("Enter The Id Of The Drone:");
//            int id = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter The Model Of The Drone:");
//            string model = Console.ReadLine();

//            Console.WriteLine("Enter The MaxWeight of the Drone:");
//            CheckValids.InputValidWeightCategories(out iInput);
//            WeightCategories maxHeight = (WeightCategories)iInput;

//            Console.WriteLine("Enter The BatteryStatus Of The Drone:");
//            CheckValids.InputValiDoubleNum(out dInput, 100);
//            double batteryStatus = (double)dInput;

//            Console.WriteLine("Enter The Status Of The Drone:");
//            bool check = int.TryParse(Console.ReadLine(), out iInput);
//            while (!check || iInput < 0 || iInput > Enum.GetNames(typeof(UrgencyStatuses)).Length)
//            {
//                Console.WriteLine("Invalid input!, please enter again");
//                check = int.TryParse(Console.ReadLine(), out iInput);
//            }
//            DroneStatuses status = (DroneStatuses)iInput;

//            return new Drone(id, model, maxHeight, batteryStatus, status);
//        }
//        /// <summary>
//        /// A function that gets details from the user of new base station 
//        /// </summary>
//        /// <returns>The new base station</returns>
//        public static BaseStation GettingBaseStation()
//        {
//            int iInput;
//            double dInput;
//            Console.WriteLine("Enter The Id Of The Station:");
//            int id = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter The Name Of The Station:");
//            string nameStation = Console.ReadLine();

//            Console.WriteLine("Enter The Number Of The Charging Stations:");
//            CheckValids.CheckValid(1, 50, out iInput);
//            int numberOfChargingStations = iInput;

//            Console.WriteLine("Enter The Longitude:");
//            CheckValids.InputValiDoubleNum(out dInput, 90);
//            double longitude = (double)dInput;

//            Console.WriteLine("Enter the Latitude:");
//            CheckValids.InputValiDoubleNum(out dInput, 180);
//            double latitude = (double)dInput;

//            return new BaseStation(id, nameStation, numberOfChargingStations, longitude, latitude);
//        }
//    }
//}