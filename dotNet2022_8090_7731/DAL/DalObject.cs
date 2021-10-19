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
