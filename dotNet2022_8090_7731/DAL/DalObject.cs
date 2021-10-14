using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;

namespace DalObject
{
    public class DalObject
    {
        
        public DalObject()
        {
            DataSource.Initialize();
        }
        public void AddingBaseStation()
        {

            Console.WriteLine("Enter The Id Of The Station:");
            CheckValids.CheckValid(9);
            DataSource.BaseStationArr[DataSource.indexBaseStationArr].Id = CheckValids.strInput;
            Console.WriteLine("Enter The Name Of The Station:");
            Console.WriteLine();
            Console.WriteLine("Enter The Number Of The Charging Stations:");

            Console.WriteLine("Enter The Longitude:");

            Console.WriteLine("Enter the Latitude:");

        }
        public void  AddingDrone()
        {
            Console.WriteLine("Enter The Id Of The Drone:");
            Console.WriteLine("Enter The Model Of The Drone:");
            Console.WriteLine("Enter The MaxWeight of the Drone:");
            Console.WriteLine("Enter The BatteryStatus Of The Drone:");
            Console.WriteLine("Enter The Status Of The Drone:");

        }
        public void AddingCustomer()
        {
            Console.WriteLine("Enter The Id Of The Customer:");
            Console.WriteLine("Enter The Name Of The Customer:");
            Console.WriteLine("Enter The Phone Of The Customer:");
            Console.WriteLine("Enter The Longitude:");
            Console.WriteLine("Enter the Latitude:");
        }
        public void GettingParcelForDelivery()
        {
            Console.WriteLine("Enter The Id Of The Parcel:");
            Console.WriteLine("Enter The Id Of The Sender:");
            Console.WriteLine("Enter The Id Of The Getter:");
            Console.WriteLine("Enter The Weight Of The Parcel:");
            Console.WriteLine("Enter The Status Of The Parcel:");
            Console.WriteLine("Enter The DroneId Of The Parcel:");
            ///i didnt finish!
        }

    }

}
