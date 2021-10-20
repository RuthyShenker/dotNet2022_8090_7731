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
        public void ChangeDroneStatus(string Id, DroneStatuses newStatus)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (Id == DroneList[i].Id)
                {
                    if (newStatus == DroneStatuses.Maintenance)
                    {
                        ChargingDrone(Id);
                    }
                    Drone changeDrone = DroneList[i];
                    changeDrone.Status = newStatus;
                    DroneList[i] = changeDrone;
                    return;
                }
            }
            throw new Exception("Id isnt exist");
        }
        public void ChargingDrone(string IdDrone)
        {
            ChargingDrone newChargingEntity = new ChargingDrone();
            foreach (BaseStation item in BaseStationList)
            {
                if (item.NumAvailablePositions != 0)
                {
                    newChargingEntity.StationId = item.Id;
                    newChargingEntity.DroneId = IdDrone;
                    ChargingDroneList.Add(newChargingEntity);
                    return;
                }
            }
            throw new Exception("There are no available positions");
        }

        public void BaseStationDisplay(string Id)
        {
            foreach (BaseStation item in BaseStationList)
            {
                if (item.Id == Id)
                {
                    item.ToString();
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

        public BaseStation[] DisplayingBaseStations()
        {
            return BaseStationList.ToArray();
        }
        public Drone[] DisplayingDrones()
        {
            return DroneList.ToArray();
        }
        public Customer[] DisplayingCustomers()
        {
            return CustomerList.ToArray();
        }
        public Parcel[] DisplayingParcels()
        {
            return ParceList.ToArray();
        }
        //-------------------------------------------------------------------------
        public Parcel[] DisplayingUnbelongParcels()
        {
            return ParceList.Where(parcel => int.Parse(parcel.DroneId) == 0).ToArray();
        }

        public BaseStation[] AvailableSlots()
        {
            return BaseStationList.Where(BaseStation => BaseStation.NumAvailablePositions > 0).ToArray();
        }
    }
}
