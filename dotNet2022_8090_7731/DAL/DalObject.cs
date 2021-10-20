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
        //שיוך חבילה לרחפן
        public void BelongParcel(string pId)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == pId);
            if (tempParcel == null) throw new Exception("NOT EXIST PARCEL WITH THIS ID");
            Drone tempDrone=DroneList.First(drone => drone.Status == DroneStatuses.Available && drone.MaxWeight>=tempParcel.Weight);
            if (tempDrone == null) tempParcel.DroneId = "0";
            tempParcel.DroneId = tempDrone.Id;
            tempDrone.Status = DroneStatuses.Delivery;
            tempParcel.BelongParcel = DateTime.Now;
        }
        //אסיפת חבילה ע"י רחפן
        public void PickingUpParcelByDrone(string Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            if (tempParcel == null) throw new Exception("NOT EXIST PARCEL WITH THIS ID");
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.PickingUp = DateTime.Now;
            ParceList.Add(tempParcel);
        }
        public void DeliveryPackageToDestination(string Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            tempParcel.Arrival = DateTime.Now;
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
