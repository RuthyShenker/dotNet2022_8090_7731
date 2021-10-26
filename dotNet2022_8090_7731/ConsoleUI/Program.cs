using System;
using System.Collections.Generic;
using DAL.DO;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IDal dalObject = new DalObject.DalObject();
            int input = 0;
            while ((ProgramOptions)input != ProgramOptions.Exit)
            {
                Console.WriteLine
                  ($"\nTap the desired option:\n" +
                  $"1 - Adding\n" +
                  $"2 - Updating\n" +
                  $"3 - Displaying a specific item\n" +
                  $"4 - Displaying a specific List\n" +
                  $"5 - Exit");
                CheckValids.CheckValid(1, 5, out input);
                switch ((ProgramOptions)input)
                {
                    case ProgramOptions.Add:
                        Console.WriteLine(
                            $"1 - Adding a base station to the stations list.\n" +
                            $"2 - Adding a drone to the drones list.\n" +
                            $"3 - Admission of a new customer to the customer list.\n" +
                            $"4 - Receipt of package for shipment.");
                        CheckValids.CheckValid(1, 4, out input);
                        switch (input)
                        {
                            case 1:
                                dalObject.Add(InputObj.GettingBaseStation());
                                break;
                            case 2:
                                dalObject.Add(InputObj.GettingDrone());
                                break;
                            case 3:
                                dalObject.Add(InputObj.GettingCustomer());
                                break;
                            case 4:
                                dalObject.Add(InputObj.GettingParcel());
                                break;
                            default:
                                break;

                        }
                        break;


                    case ProgramOptions.Update:
                        Console.WriteLine(
                            $"1 - Belong a package to a drone\n" +
                            $"2 - Raising package by drone\n" +
                            $"3 - Delivery of a package to the destination\n" +
                            $"4 - Sending drone for charging at the base station\n" +
                            $"5 - Release drone from charging at the base station");
                        CheckValids.CheckValid(1, 5, out input);

                        switch (input)
                        {
                            case 1:
                                dalObject.BelongingParcel(GettingId("Parcel"));
                                break;
                            case 2:
                                dalObject.PickingUpParcel(GettingId("Parcel"));
                                break;
                            case 3:
                                dalObject.DeliveryPackage(GettingId("Parcel"));
                                break;
                            case 4:
                                dalObject.ChargingDrone(GettingId("Drone"));
                                break;
                            case 5:
                                dalObject.ReleasingDrone(GettingId("Drone"));
                                input = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    case ProgramOptions.DisplaySpecific:
                        Console.WriteLine(
                            $"1 - Base station view\n" +
                            $"2 - Drone view\n" +
                            $"3 - Customer view\n" +
                            $"4 - Package view");
                        CheckValids.CheckValid(1, 4, out input);

                        switch (input)
                        {
                            case 1:
                                BaseStation baseStation = dalObject.BaseStationDisplay(GettingId("Base Station"));
                                Console.WriteLine(baseStation);
                                break;
                            case 2:
                                Drone drone = dalObject.DroneDisplay(GettingId("Drone"));
                                Console.WriteLine(drone);
                                break;
                            case 3:
                                Customer customer = dalObject.CustomerDisplay(GettingIdAsString("Customer"));
                                Console.WriteLine(customer);
                                break;
                            case 4:
                                Parcel parcel = dalObject.ParcelDisplay(GettingId("Parcel"));
                                Console.WriteLine(parcel);
                                break;
                            default:
                                break;

                        }
                        break;
                    case ProgramOptions.DisplayList:
                        Console.WriteLine(
                            $"1 - View base stations list\n" +
                            $"2 - View drones list\n" +
                            $"3 - View customers list\n" +
                            $"4 - View packages list\n" +
                            $"5 - View of packages which are unbelong to drones\n" +
                            $"6 - View base stations with available charging positions");
                        CheckValids.CheckValid(1, 6, out input);

                        switch (input)
                        {
                            case 1:
                                IEnumerable<BaseStation> StationList = dalObject.DisplayingBaseStations();
                                foreach (BaseStation baseStation in StationList)
                                {
                                    Console.WriteLine(baseStation);
                                }
                                break;
                            case 2:
                                IEnumerable<Drone> DroneList = dalObject.DisplayingDrones();
                                foreach (Drone drone in DroneList)
                                {
                                    Console.WriteLine(drone);
                                }
                                break;
                            case 3:
                                IEnumerable<Customer> CustomerList = dalObject.DisplayingCustomers();
                                foreach (Customer customer in CustomerList)
                                {
                                    Console.WriteLine(customer);
                                }
                                break;
                            case 4:
                                IEnumerable<Parcel> ParcelList = dalObject.DisplayingParcels();
                                foreach (Parcel parcel in ParcelList)
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case 5:
                                IEnumerable<Parcel> UnbelongParcelsList = dalObject.DisplayingUnbelongParcels();
                                foreach (Parcel parcel in UnbelongParcelsList)
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case 6:
                                IEnumerable<BaseStation> AvailableSlotsList = dalObject.AvailableSlots();
                                foreach (BaseStation baseStation in AvailableSlotsList)
                                {
                                    Console.WriteLine(baseStation);
                                }
                                break;
                            default:
                                break;

                        }
                        break;
                    case ProgramOptions.Exit:
                        {
                            Console.WriteLine("good bye!");
                            break;
                        }
                    default:
                        break;
                }

            }
        }
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

        /// <summary>
        /// A function that gets from the user Id and returns it as int
        /// </summary>
        /// <param name="obj">a name of object</param>
        /// <returns>an id as int</returns>
        private static int GettingId(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return int.Parse(Console.ReadLine());
        }


    }
}
