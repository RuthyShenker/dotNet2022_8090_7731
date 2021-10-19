using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using static DalObject.DataSource;
using static DalObject.DataSource.Config;
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
            BaseStationArr[IndexBaseStationArr] = baseStation;
        }
        public void  AddingDrone(Drone drone)
        {
            DroneArr[IndexDroneArr] = drone;
        }
        public void AddingCustomer(Customer customer)
        {
            CustomerArr[IndexCustomerArr] = customer;
        }
        public void GettingParcelForDelivery(Parcel parcel)
        {
            ParcelArr[IndexParcelArr] = parcel;
        }
        public void AffiliationParcel(string pId)
        {
            if (AvailableDrones())
            {
                for (int i = 0; i < IndexParcelArr; i++)
                {
                    if (pId == ParcelArr[i].ParcelId)
                    { 
                        do
                        {
                            ParcelArr[i].DroneId = DroneArr[rand.Next(0, IndexDroneArr)].Id;
                        } while (DroneArr[rand.Next(0, IndexDroneArr)].Status != DroneStatuses.Available);
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
            for (int i = 0; i < DroneArr.Length; i++)
                if(DroneArr[i].Status == DroneStatuses.Available)
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
            
            for (int i = 0; i < IndexDroneArr; i++)
            {
                if (Id == DroneArr[i].Id)
                {
                    DroneArr[i].Status = newStatus;
                    return;
                }
            }
            //  throw ("Id isnt exist ");
        }
       
       
        public void ReleasingDroneFromChargingAtBaseStation()
        {

        }
        public void BaseStationDisplay(string Id)
        {
            foreach (var baseStation in BaseStationArr)
            {
                if(baseStation.Id==Id)
                {
                    Console.WriteLine(baseStation);
                }
            }
        }
        public void DroneDisplay(string Id)
        {
            foreach (var drone in DroneArr)
            {
                if (drone.Id == Id)
                {
                    Console.WriteLine(drone);
                }
            }

        }
        public void CustomerDisplay(string Id)
        {
            foreach (var customer in CustomerArr)
            {
                if (customer.Id == Id)
                {
                    Console.WriteLine(customer);
                }
            }
        }
        public void ParcelDisplay(string Id)
        {
            foreach (var parcel in ParcelArr)
            {
                if (parcel.ParcelId == Id)
                {
                    Console.WriteLine(parcel);
                }
            }
        }

        public void AffiliationParcel(object getId)
        {
            throw new NotImplementedException();
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
