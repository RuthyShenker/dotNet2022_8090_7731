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
        public void AddingDrone(Drone drone)
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
        public void BelongingParcel(string pId)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == pId);
            foreach (Drone drone in DroneList)
            {
                if (drone.Status == DroneStatuses.Available && drone.MaxWeight >= tempParcel.Weight)
                {
                    tempParcel.DroneId = drone.Id;
                    tempParcel.BelongParcel = DateTime.Now;
                }

                else
                {
                    tempParcel.DroneId = "0";
                }
            }
        }
            //אסיפת חבילה ע"י רחפן
        public void PickingUpParcel(string Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.PickingUp = DateTime.Now;
            ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Delivery);
            ParceList.Add(tempParcel);
        }
        //אספקת חבילה ליעד
        public void DeliveryPackage(string Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.Arrival = DateTime.Now;
            ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Available);
            ParceList.Add(tempParcel);
        }
        /// <summary>
        ///A function that gets an integer that means a new status and Id of drone and 
        ///changes the drone that his Id was given to the new status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus"></param>
        public void ChangeDroneStatus(string Id, DroneStatuses newStatus)
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
            throw new Exception("Not Exist Drone With This Id");
        }
        public void ChargingDrone(string IdDrone)
        {
            foreach (BaseStation baseStation in BaseStationList)
            {
                if (baseStation.NumAvailablePositions != 0)
                {
                    ChangeDroneStatus(IdDrone, DroneStatuses.Maintenance);
                    ChargingDrone newChargingEntity = new ChargingDrone();
                    newChargingEntity.StationId = baseStation.Id;
                    newChargingEntity.DroneId = IdDrone;
                    ChargingDroneList.Add(newChargingEntity);
                    return;
                }
            }
            throw new Exception("There Are No Available Positions");
        }
        public void ReleasingDrone(string dId)
        {
            ChangeDroneStatus(dId, DroneStatuses.Available);
            ChargingDroneList.Remove(ChargingDroneList.First(chargingDrone=>chargingDrone.DroneId==dId));
        }
        //----------------------------------------------------לאחד לפונ אחת
        public void BaseStationDisplay(string id)
        {
            foreach (BaseStation baseStation in BaseStationList)
            {
                if (baseStation.Id == id)
                {
                    baseStation.ToString();
                }
            }
        }
        public void DroneDisplay(string Id)
        {
            foreach (Drone drone in DroneList)
            {
                if (drone.Id == Id)
                {
                    Console.WriteLine(drone);
                }
            }
        }
        public void CustomerDisplay(string Id)
        {
            foreach (Customer customer in CustomerList)
            {
                if (customer.Id == Id)
                {
                    Console.WriteLine(customer);
                }
            }
        }
        public void ParcelDisplay(string Id)
        {
            foreach (Parcel parcel in ParceList)
            {
                if (parcel.ParcelId == Id)
                {
                    Console.WriteLine(parcel);
                }
            }
        }
        //---------------------------------------------------------------------------------

        //----------------------------------------------------לאחד לפונ אחת

        public void DisplayingBaseStations()
        {
            foreach (BaseStation baseStation in BaseStationList)
            {
                baseStation.ToString();
            }
        }
        public void DisplayingDrones()
        {
            foreach (Drone drone in DroneList)
            {
                drone.ToString();
            }
        }
        public void DisplayingCustomers()
        {
            foreach (Customer customer in CustomerList)
            {
                customer.ToString();
            }
        }
        public void DisplayingParcels()
        {
            foreach (Parcel parcel in ParceList)
            {
                parcel.ToString();
            }
        }
        //-------------------------------------------------------------------------
        public void DisplayingUnbelongParcels()
        {
            foreach (Parcel parcel in ParceList)
            {
                if (int.Parse(parcel.DroneId) == 0)
                {
                    parcel.ToString();
                }
            }
        }
        public void DisplayingStationsWithAvailablePositions()
        {
            foreach (BaseStation baseStation in BaseStationList)
            {
                if (baseStation.NumAvailablePositions > 0)
                {
                    baseStation.ToString();
                }
            }
        }
    }
}
