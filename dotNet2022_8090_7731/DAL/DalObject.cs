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
        public void BelongParcel(string pId)
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
       
       
       
        public void BaseStationDisplay(int Id)
        {
            Console.WriteLine(BaseStationArr[Id]);
        }
        public void DroneDisplay(int Id)
        {

        }
        public void CustomerDisplay(int Id)
        {

        }
        public void ParcelDisplay(int Id)
        {

        }
        //----------------------------------------------------לאחד לפונ אחת

        public void DisplayingBaseStations()
        {
            foreach (BaseStation item in BaseStationArr)
            {
                item.ToString();
            }
        }
        public void DisplayingDrones()
         {
            foreach (Drone item in DroneArr)
            {
                item.ToString();
            }
        }
        public void DisplayingCustomers()
        {
            foreach (Customer item in CustomerArr)
            {
                item.ToString();
            }
        }
        public void DisplayingParcels()
        {
            foreach (Parcel item in ParcelArr)
            {
                item.ToString();
            }
        }
        //-------------------------------------------------------------------------
        public void DisplayingUnbelongParcels()
        {
            foreach (Parcel item in ParcelArr)
            {
                
                if (int.Parse( item.DroneId)!=0)
                {
                    item.ToString();
                }
            }
        }
        public void DisplayingStationsWithAvailablePositions()
        {
            foreach (BaseStation item in BaseStationArr)
            {
                for (int i = 0; i < item.NumChargingStations; i++)
                {
                    if (true)
                    {

                    } item.
                }
                if (item.NumChargingStations)
                {

                }
                item.ToString();
            }
        }

    }

}
