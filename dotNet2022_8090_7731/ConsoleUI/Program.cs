using System;
using DAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine
                ($"Options:\n" +
                $"1 - Adding\n" +
                $"2 - Updating\n" +
                $"3 - Display\n" +
                $"4 - List view options\n" +
                $"5 - Exit");
            DalObject.DalObject dalObject = new DalObject.DalObject();
            BaseStation baseStation;
            Drone drone;
            Customer customer;
            Parcel parcel;
            int input;
            CheckValids.CheckValid(1, 5,out input);

            switch (input)
            {
                case 1:
                    Console.WriteLine(
                        $"1 - ● Add a base station to the list of stations.\n" +
                        $"2 - ● Add a drone to the list of existing drones.\n" +
                        $"3 - ● Admission of a new customer to the customer list.\n" +
                        $"4 - ● Receipt of package for shipment.");
                    CheckValids.CheckValid(1, 4,out input);
                    switch (input)
                    {
                        case 1:
                            baseStation = GetBaseStation();
                            dalObject.AddingBaseStation(baseStation);
                            break;
                        case 2:
                            drone = GetDrone();
                            dalObject.AddingDrone(drone);
                            break;
                        case 3:
                            customer = GetCustomer();
                            dalObject.AddingCustomer(customer);
                            break;
                        case 4:
                            ///I didnt finish!
                            parcel = GetParcel();
                            dalObject.GettingParcelForDelivery(parcel);
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
                            dalObject.BelongParcel(GettingId("Parcel"));
                            break;
                        case 2:
                            dalObject.ChangeDroneStatus(GettingId("Drone"),DroneStatuses.Delivery);
                            break;
                        case 3:
                            dalObject.ChangeDroneStatus(GettingId("Drone"),DroneStatuses.Available);
                            break;
                        case 4:
                            dalObject.ChangeDroneStatus(GettingId("Drone"), DroneStatuses.Maintenance);
                            break;
                        case 5:
                            dalObject.ChangeDroneStatus(GettingId("Drone"), DroneStatuses.Available);
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
                    CheckValids.CheckValid(1, 6);

                    switch (CheckValids.input)
                    {
                        case 1:
                            dalObject.DisplayingListOfBaseStations();
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

        private static Parcel GetParcel()
        {
            Console.WriteLine("Enter The Id Of The Parcel: ");
            string parcelId = Console.ReadLine();
            Console.WriteLine("Enter The Id Of The Sender: ");
            string senderId= Console.ReadLine();
            Console.WriteLine("Enter The Id Of The Getter: ");
            string getterId = Console.ReadLine();
            Console.WriteLine("Enter The Weight Of The Parcel: ");
            WeightCategories weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The Status Of The Parcel:");
            UrgencyStatuses status = (UrgencyStatuses)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The DroneId Of The Parcel: ");
            string droneId = Console.ReadLine();
            DateTime makingParcel = DateTime.Now;
            /////מה צריך להיות במשתנים הללו?
            DateTime pickingUp = DateTime.Now;
            DateTime arrival = DateTime.Now;
            DateTime matchingParcel = DateTime.Now;
            //i didnt finish!
            return new Parcel(parcelId, senderId, getterId, weight, status, droneId, makingParcel, pickingUp, arrival, matchingParcel);

        }

        private static Customer GetCustomer()
        {
            Console.WriteLine("Enter The Id Of The Customer:");
            string id = Console.ReadLine();
            Console.WriteLine("Enter The Name Of The Customer:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter The Phone Of The Customer:");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter The Longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Latitude:");
            double latitude= double.Parse(Console.ReadLine());
            return new Customer(id, name, phone, longitude, latitude);
        }

        private static Drone GetDrone()
        {
            Console.WriteLine("Enter The Id Of The Drone:");
            string id = Console.ReadLine();
            Console.WriteLine("Enter The Model Of The Drone:");
            string model=Console.ReadLine();
            Console.WriteLine("Enter The MaxWeight of the Drone:");
            WeightCategories maxHeight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The BatteryStatus Of The Drone:");
            double batteryStatus = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter The Status Of The Drone:");
            DroneStatuses status = (DroneStatuses)int.Parse(Console.ReadLine());
            return new Drone(id, model, maxHeight, batteryStatus, status);
        }

        private static BaseStation GetBaseStation()
        {
            Console.WriteLine("Enter The Id Of The Station:");
            string id = Console.ReadLine();
            Console.WriteLine("Enter The Name Of The Station:");
            string nameStation = Console.ReadLine();
            Console.WriteLine("Enter The Number Of The Charging Stations:");
            int numberOfChargingStations = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The Longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Latitude:");
            double latitude = double.Parse(Console.ReadLine()); ;
            return new BaseStation(id, nameStation, numberOfChargingStations, longitude, latitude);
        }
    }
}
