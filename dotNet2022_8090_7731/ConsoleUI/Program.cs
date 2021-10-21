using System;
using DAL.DO;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();
            int input = 0;
            while (input != 5)
            {
                Console.WriteLine
                  ($"\nTap the desired option:\n" +
                  $"1 - Adding\n" +
                  $"2 - Updating\n" +
                  $"3 - Displaying a specific item\n" +
                  $"4 - Displaying a specific List\n" +
                  $"5 - Exit");
                CheckValids.CheckValid(1, 5, out input);
                switch (input)
                {
                    case 1:
                        Console.WriteLine(
                            $"1 - Adding a base station to the stations list.\n" +
                            $"2 - Adding a drone to the drones list.\n" +
                            $"3 - Admission of a new customer to the customer list.\n" +
                            $"4 - Receipt of package for shipment.");
                        CheckValids.CheckValid(1, 4, out input);
                        switch (input)
                        {
                            case 1:
                                dalObject.AddingBaseStation(InputObj.GettingBaseStation());
                                break;
                            case 2:
                                dalObject.AddingDrone(InputObj.GettingDrone());
                                break;
                            case 3:
                                dalObject.AddingCustomer(InputObj.GettingCustomer());
                                break;
                            case 4:
                                dalObject.GettingParcelForDelivery(InputObj.GettingParcel());
                                break;
                            default:
                                break;

                        }
                        break;


                    case 2:
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
                    case 3:
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
                                Customer customer = dalObject.CustomerDisplay(GettingId("Customer"));
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
                    case 4:
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
                                BaseStation[] StationArr = dalObject.DisplayingBaseStations();
                                foreach (BaseStation baseStation in StationArr)
                                {
                                    Console.WriteLine(baseStation);
                                }
                                break;
                            case 2:
                                Drone[] DroneArr = dalObject.DisplayingDrones();
                                foreach (Drone drone in DroneArr)
                                {
                                    Console.WriteLine(drone);
                                }
                                break;
                            case 3:
                                Customer[] CustomerArr = dalObject.DisplayingCustomers();
                                foreach (Customer customer in CustomerArr)
                                {
                                    Console.WriteLine(customer);
                                }
                                break;
                            case 4:
                                Parcel[] ParcelArr = dalObject.DisplayingParcels();
                                foreach (Parcel parcel in ParcelArr)
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case 5:
                                Parcel[] UnbelongParcelsArr = dalObject.DisplayingUnbelongParcels();
                                foreach (Parcel parcel in UnbelongParcelsArr)
                                {
                                    Console.WriteLine(parcel);
                                }
                                input = 0;
                                break;
                            case 6:
                                BaseStation[] AvailableSlotsArr = dalObject.AvailableSlots();
                                foreach (BaseStation baseStation in AvailableSlotsArr)
                                {
                                    Console.WriteLine(baseStation);
                                }
                                break;
                            default:
                                break;

                        }
                        break;
                    default:
                        break;
                }

            }
            Console.WriteLine("good bye!");
        }
        private static string GettingId(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return Console.ReadLine();
        }


    }
}
