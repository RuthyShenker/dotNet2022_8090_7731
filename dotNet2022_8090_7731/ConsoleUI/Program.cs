using System;
using DAL.DO;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
           
            DalObject.DalObject dalObject = new DalObject.DalObject();
            int input; 

            Console.WriteLine
                ($"Options:\n" +
                $"1 - Adding\n" +
                $"2 - Updating\n" +
                $"3 - Display\n" +
                $"4 - List view options\n" +
                $"5 - Exit");
            CheckValids.CheckValid(1, 5,out input);

            switch (input)
            {
                case 1:
                    Console.WriteLine(
                        $"1 - Add a base station to the list of stations.\n" +
                        $"2 - Add a drone to the list of existing drones.\n" +
                        $"3 - Admission of a new customer to the customer list.\n" +
                        $"4 - Receipt of package for shipment.");
                    CheckValids.CheckValid(1, 4,out input);
                    switch (input)
                    {
                        case 1:
                            dalObject.AddingBaseStation(InputObj.GettingBaseStation());
                            break;
                        case 2:
                            dalObject.AddingDrone(InputObj.GettingDrone());
                            break;
                        case 3:
                            dalObject.AddingCustomer( InputObj.GettingCustomer());
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
                        $"1 - ● Assigning a package to a skimmer\n" +
                        $"2 - ● Skimmer package assembly\n" +
                        $"3 - ● Delivery of a package to the destination\n" +
                        $"4 - ● Sending a skimmer for charging at a base station\n" +
                        $"5 - ● Release skimmer from charging at base station\n");
                    CheckValids.CheckValid(1, 5,out input);

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
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    Console.WriteLine(
                        $"1 - ● Base station view\n" +
                        $"2 - ● Skimmer display\n" +
                        $"3 - ● Customer view\n" +
                        $"4 - ● Package view\n");
                    CheckValids.CheckValid(1, 4,out input);

                    switch (input)
                    {
                        case 1:
                            dalObject.BaseStationDisplay(GettingId("Base Station"));
                            break;
                        case 2:
                            dalObject.DroneDisplay(GettingId("Drone"));
                            break;
                        case 3:
                            dalObject.CustomerDisplay(GettingId("Customer"));
                            break;
                        case 4:
                            dalObject.ParcelDisplay(GettingId("Parcel"));
                            break;
                        default:
                            break;

                    }
                    break;
                case 4:
                    Console.WriteLine(
                        $"1 - ● Displays a list of base stations\n" +
                        $"2 - ● Displays the list of skimmers\n" +
                        $"3 - ● View the customer list\n" +
                        $"4 - ● Displays the list of packages\n" +
                        $"5 - ● Displays a list of packages that have not yet been assigned to the glider\n" +
                        $"6 - ● Display base stations with available charging stations\n");
                    CheckValids.CheckValid(1, 6,out input);

                    switch (input)
                    {
                        case 1:
                            dalObject.DisplayingBaseStations();
                            break;
                        case 2:
                            dalObject.DisplayingDrones();
                            break;
                        case 3:
                            dalObject.DisplayingCustomers();
                            break;
                        case 4:
                            dalObject.DisplayingParcels();
                            break;
                        case 5:
                            dalObject.DisplayingUnbelongParcels();
                            break;
                        case 6:
                            dalObject.DisplayingStationsWithAvailablePositions();
                            break;
                        default:
                            break;

                    }
                    break;
                case 5:

                    break;

                default:

                    break;
            }

        }

        private static string GettingId(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return Console.ReadLine();
        }

        
    }
}
