using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleUI_BL.MEnum;
using BO;
namespace ConsoleUI_BL
{
    partial class Program
    {
        public static void AddingOption(BlApi.IBL bL)
        {
            int input = 0;
            tools.PrintEnum(typeof(Adding));
            CheckValids.CheckValid(1, 4, out input);
            switch ((Adding)input)
            {
                case Adding.BaseStation:
                    AddingNewBaseStation();
                    break;

                case Adding.Drone:
                    bL.AddingDrone(GettingNewDrone(out int stationId), stationId);
                    break;

                case Adding.Customer:
                    int id = bL.AddCustomer(GettingNewCustomer());
                    Console.WriteLine("The process of adding a new customer has been completed." +
                         " The adding was completed successfully. The id that was added is: " + id);
                    break;

                case Adding.Parcel:
                    int numberOfOrder = bL.AddingParcel(GettingNewParcel());
                    Console.WriteLine("The process of creating a new package has been completed." +
                        " The order was completed successfully. The order number is: " + numberOfOrder);
                    break;

                default: break;
            }


            //catch (IdIsNotValidException exception)
            //{
            //    Console.WriteLine(exception);
            //}
            //catch(TheStationDoesNotHaveFreePositionsException exception)
            //{
            //    Console.WriteLine(exception);
            //}
        }

        /// <summary>
        /// A function that gets details from the user and create a new parcel of bl and returns it.
        /// </summary>
        /// <returns>parcel ,type=bl</returns>
        private static Parcel GettingNewParcel()
        {
            Console.WriteLine("Enter the id of the sender of the new parcel: ");
            int senderId = CheckValids.InputIdCustomerValidity();
            Console.WriteLine("Enter the id of the getter of the new parcel: ");
            int getterId = CheckValids.InputIdCustomerValidity();
            Console.WriteLine("Enter the weight of the new parcel");
            tools.PrintEnum(typeof(WeightCategories));
            WeightCategories weight = (WeightCategories)CheckValids.InputValidOfEnum(typeof(WeightCategories));
            Console.WriteLine("Enter the priority of the new parcel");
            tools.PrintEnum(typeof(Priority));
            Priority mPriority = (Priority)CheckValids.InputValidOfEnum(typeof(Priority));
            DroneInParcel DInParcel = null;

            return new Parcel(senderId, getterId, weight, mPriority, DInParcel);
        }

        /// <summary>
        /// A function that gets details from the user and create a new customer of bl and returns it.
        /// </summary>
        /// <returns>Customer ,type=bl</returns>
        private static Customer GettingNewCustomer()
        {
            Console.WriteLine("Enter the id of the new customer: ");
            int id = CheckValids.InputIdCustomerValidity();
            Console.WriteLine("Enter the name of the new customer: ");
            string name = CheckValids.InputNameValidity();
            Console.WriteLine("Enter the phone of the new customer: ");
            string phone = CheckValids.InputPhoneValidity();
            Console.WriteLine("Enter the Location of the new customer: ");
            Console.WriteLine("longitude: ");
            double longitude = CheckValids.InputDoubleValidity("longitude");
            Console.WriteLine("latitude: ");
            double latitude = CheckValids.InputDoubleValidity("latitude");
            Location cLocation = new Location(longitude, latitude);
            //List<ParcelInCustomer> lFromCustomer;
            //List<ParcelInCustomer> LForCustomer

            return new Customer(id, name, phone, cLocation);
        }

        /// <summary>
        /// A function that gets details from the user and create a new Station of bl and returns it.
        /// </summary>
        /// <returns>Station ,type=bl</returns>
        private static void AddingNewBaseStation()
        {
            Console.WriteLine("Enter the id of the new base station: ");
            int id = CheckValids.InputNumberValidity("id");
            Console.WriteLine("Enter the name of the new base station: ");
            string nameStation = CheckValids.InputNameValidity();
            Console.WriteLine("Enter the Location of the new base station: ");
            Console.WriteLine("longitude: ");
            double longitude = CheckValids.InputDoubleValidity("longitude");
            Console.WriteLine("latitude: ");
            double latitude = CheckValids.InputDoubleValidity("latitude");
            Console.WriteLine("Enter the number of positions of the new base station: ");
            ///●	מספר עמדות טעינה (פנויות) - כל העמדות פנויות בהוספה
            int numAvailablePositions = CheckValids.InputNumberValidity("number of positions");

            bl.AddingBaseStation(id, nameStation, longitude, latitude, numAvailablePositions);
        }

        /// <summary>
        /// A function that gets details from the user and create a new Drone of bl and returns it.
        /// </summary>
        /// <returns>Drone ,type=bl</returns>
        private static Drone GettingNewDrone(out int stationId)
        {
            Console.WriteLine("Enter the serial number of the manufacturer of the new drone: ");
            int id = CheckValids.InputNumberValidity("Id");
            Console.WriteLine("Enter Model: ");
            string model = Console.ReadLine();
            Console.WriteLine("Enter the max weight the drone can carry: ");
            tools.PrintEnum(typeof(WeightCategories));
            WeightCategories weight = (WeightCategories)CheckValids.InputValidOfEnum(typeof(WeightCategories));
            Console.WriteLine("Enter the station Id to charge the new drone for first charging:  ");
            stationId = CheckValids.InputNumberValidity("id");
            float batteryStatus = 100;
            DroneStatus droneStatus = default(DroneStatus);
            ParcelInTransfer pInTransfer = null;
            Location currLocation = null;

            return new Drone(id, model, weight, batteryStatus,
                droneStatus, pInTransfer, currLocation);
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

        ///// <summary>
        ///// A function that gets from the user Id and returns it as string
        ///// </summary>
        ///// <param name="obj">a name of object</param>
        ///// <returns>an id as string</returns>
        //private static string GettingIdAsString(string obj)
        //{
        //    Console.WriteLine($"Enter The Id Of The {obj}:");
        //    return Console.ReadLine();
        //}
    }
}


