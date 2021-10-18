﻿using System;
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
            CheckValids.CheckValid(1, 5);

            switch (CheckValids.input)
            {
                case 1:
                    Console.WriteLine(
                        $"1 - ● Add a base station to the list of stations.\n" +
                        $"2 - ● Add a drone to the list of existing drones.\n" +
                        $"3 - ● Admission of a new customer to the customer list.\n" +
                        $"4 - ● Receipt of package for shipment.");
                    CheckValids.CheckValid(1, 4);
                    switch (CheckValids.input)
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
                    CheckValids.CheckValid(1, 5);

                    switch (CheckValids.input)
                    {
                        case 1:
                            Console.WriteLine("Enter The Id Of The parcel:");
                            string PId = Console.ReadLine();
                            dalObject.AffiliationParcel(PId);
                            break;
                        case 2:
                            Console.WriteLine("Enter The Id Of The drone:");
                            string dId = Console.ReadLine();
                            dalObject.ChangeDroneStatus(dId,3);
                            break;
                        case 3:
                            Console.WriteLine("Enter The Id Of The drone:");
                            dId = Console.ReadLine();
                            dalObject.ChangeDroneStatus(dId, 3);
                            break;
                        case 4:
                            Console.WriteLine("Enter The Id Of The drone:");
                            dId = Console.ReadLine();
                            dalObject.ChangeDroneStatus(dId, 2);
                            break;
                        case 5:
                            Console.WriteLine("Enter The Id Of The drone:");
                            dId = Console.ReadLine();
                            dalObject.ChangeDroneStatus(dId, 1);
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
                    CheckValids.CheckValid(1, 4);

                    switch (CheckValids.input)
                    {
                        case 1:
                            dalObject.BaseStationDisplay(GetIdToDisplay());
                            break;
                        case 2:
                            dalObject.DroneDisplay(GetIdToDisplay());
                            break;
                        case 3:
                            dalObject.CustomerDisplay(GetIdToDisplay());
                            break;
                        case 4:
                            dalObject.ParcelDisplay(GetIdToDisplay());
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
                            dalObject.DisplayingListOfDrones();
                            break;
                        case 3:
                            dalObject.DisplayingListOfCustomers();
                            break;
                        case 4:
                            dalObject.DisplayingListOfParcels();
                            break;
                        case 5:
                            dalObject.DisplayingListOfParcelsNotYetAssociatedToDrone();
                            break;
                        case 6:
                            dalObject.DisplayingListOfBaseStationsWithAvailableChargingStation();
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

        private static int GetIdToDisplay()
        {
            Console.WriteLine("Enter Id To Display:");
            return int.Parse(Console.ReadLine());
        }

        private static Parcel GetParcel()
        {
            Console.WriteLine("Enter The Id Of The Parcel:");
            string parcelId = Console.ReadLine();
            Console.WriteLine("Enter The Id Of The Sender:");
            string senderId= Console.ReadLine();
            Console.WriteLine("Enter The Id Of The Getter:");
            string getterId = Console.ReadLine();
            Console.WriteLine("Enter The Weight Of The Parcel:");
            WeightCategories weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The Status Of The Parcel:");
            UrgencyStatuses status = (UrgencyStatuses)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter The DroneId Of The Parcel:");
            int droneId = int.Parse(Console.ReadLine());
            DateTime makingParcel = DateTime.Now;
            DateTime pickingUp;
            DateTime arrival;
            DateTime matchingParcel;
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
