using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    public class DalObject
    {

        public DalObject()
        {
            DataSource.Initialize();
        }
        public void AddingBaseStation(BaseStation baseStation)
        {
            CheckValids.CheckValid(baseStation.Id);
            baseStation.Latitude;
            baseStation.Longitude;
            baseStation.NameStation;
            baseStation.NumberOfChargingStations;
            throw new ArgumentException("iukjythgrhyuik");
        }



        public void AddingDrone()
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
        public void AffiliationParcel(string pId)
        {
            for (int i = 0; i < Config.IndexParcelArr; i++)
            {
                if (pId == ParcelArr[i].ParcelId)
                {
                    ParcelArr[i].DroneId = DroneArr[ rand.Next(0, Config.IndexDroneArr)].Id;
                    return;
                }
            }
            //  throw ("Id isnt exist ");
        }

        public void ChangeDroneStatus(string dId,int newStatus)
        {
            for (int i = 0; i < Config.IndexDroneArr; i++)
            {
                if (dId == DroneArr[i].Id)
                {
                    DroneArr[i].Status = (DroneStatuses)newStatus;
                    return;
                }
            }
            //  throw ("Id isnt exist ");
        }
       
       
        public void ReleasingDroneFromChargingAtBaseStation()
        {

        }
        public void BaseStationDisplay()
        {

        }
        public void DroneDisplay()
        {

        }
        public void CustomerDisplay()
        {

        }
        public void ParcelDisplay()
        {

        }

        public void DisplayingListOfBaseStations()
        {

        }
        public void DisplayingListOfDrones()
        {

        }
        public void DisplayingListOfCustomers()
        {

        }
        public void DisplayingListOfParcels()
        {

        }
        public void DisplayingListOfParcelsNotYetAssociatedToDrone()
        {

        }
        public void DisplayingListOfBaseStationsWithAvailableChargingStation()
        {

        }

    }

}
