using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using DalObject;

namespace ConsoleUI
{
    class InputObj
    {
        public static Parcel GettingParcel()
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
            DateTime belongParcel =makingParcel.AddDays(rand.Next(0, 4));
            DateTime pickingUp = belongParcel.AddDays(rand.Next(0, 11));
            DateTime arrival = pickingUp.AddDays(rand.Next(0, 11));
            return new Parcel(parcelId, senderId, getterId, weight, status, droneId, makingParcel,belongParcel ,pickingUp, arrival);

        }

        public static Customer GettingCustomer()
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

        public static Drone GettingDrone()
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

        public static BaseStation GettingBaseStation()
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
