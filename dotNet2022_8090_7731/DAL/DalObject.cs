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
            Initialize();
        }
        public void AddingBaseStation(BaseStation baseStation)
        {
            BaseStationList.Add(baseStation);
        }
        public void  AddingDrone(Drone drone)
        {
            DroneList.Add(drone);
        }
        public void AddingCustomer(Customer customer)
        {
            CustomerList.Add(customer);
        }
        public void GettingParcelForDelivery(Parcel parcel)
        {
            ParceList.Add(parcel);
        }
        public void BelongParcel(string pId)
        {
            if (AvailableDrones())
            {
                for (int i = 0; i < DroneList.Count; i++)
                {
                    if (pId == ParceList[i].ParcelId)
                    {
                        Parcel changeParcel = ParceList[i];
                        do
                        {
                            changeParcel.DroneId = DroneList[rand.Next(0, DroneList.Count)].Id;
                            ParceList[i] = changeParcel;
                        } while (DroneList[rand.Next(0, DroneList.Count)].Status != DroneStatuses.Available);
                        return;
                    }
                }
            }
            //Console.WriteLine("There isn't available Drone!");
            //  throw ("There isn't available Drone!");
        }
        /// <summary>
        /// A function that checks if exists drone that his statusDrone is available.
        /// </summary>
        /// <returns>if exists available drone return true else false</returns>
        public bool AvailableDrones()
        {
            for (int i = 0; i < DroneList.Count; i++)
                if(DroneList[i].Status == DroneStatuses.Available)
                    return true;
            return false;
        }
        /// <summary>
        ///A function that gets an integer that means a new status and Id of drone and 
        ///changes the drone that his Id was given to the new status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus"></param>
        public void ChangeDroneStatus(string Id,DroneStatuses newStatus)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (Id == DroneList[i].Id)
                {
                    Drone changeDrone = DroneList[i];
                    changeDrone.Status = newStatus;
                    DroneList[i] = changeDrone;
                    return;
                }
            }
            //  throw ("Id isnt exist ");
        }

        public void BaseStationDisplay(int Id)
        {
            BaseStationList[Id].ToString();
        }
      
        public void BaseStationDisplay(string Id)
        {
            foreach (var baseStation in BaseStationList)
            {
                if(baseStation.Id==Id)
                {
                    baseStation.ToString(); 
                }
            }
        }
        public void DroneDisplay(string Id)
        {
            foreach (Drone item in DroneList)
            {
                if (item.Id == Id)
                {
                    Console.WriteLine(item);
                }
            }
        }
        public void CustomerDisplay(string Id)
        {
            foreach (Customer item in CustomerList)
            {
                if (item.Id == Id)
                {
                    Console.WriteLine(item);
                }
            }
        }
        public void ParcelDisplay(string Id)
        {
            foreach (Parcel item in ParceList)
            {
                if (item.ParcelId == Id)
                {
                    Console.WriteLine(item);
                }
            }
        }

        //----------------------------------------------------לאחד לפונ אחת

        public void DisplayingBaseStations()
        {
            foreach (BaseStation item in BaseStationList)
            {
                item.ToString();
            }
        }
        public void DisplayingDrones()
         {
            foreach (Drone item in DroneList)
            {
                item.ToString();
            }
        }
        public void DisplayingCustomers()
        {
            foreach (Customer item in CustomerList)
            {
                item.ToString();
            }
        }
        public void DisplayingParcels()
        {
            foreach (Parcel item in ParceList)
            {
                item.ToString();
            }
        }
        //-------------------------------------------------------------------------
        public void DisplayingUnbelongParcels()
        {
            foreach (Parcel item in ParceList)
            {
                if (int.Parse( item.DroneId)!=0)
                {
                    item.ToString();
                }
            }
        }
        public void DisplayingStationsWithAvailablePositions()
        {
            foreach (BaseStation item in BaseStationList)
            {
                for (int i = 0; i < item.NumChargingStations; i++)
                {
                    if (true)
                    {

                    } item.
                }
                
                item.ToString();
            }
        }
    }
}
