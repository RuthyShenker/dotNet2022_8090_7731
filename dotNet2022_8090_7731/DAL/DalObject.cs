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
        public void Add(BaseStation baseStation)
        {
            BaseStationList.Add(baseStation);
        }
        public void Add(Drone drone)
        {
            DroneList.Add(drone);
        }
        public void Add(Customer customer)
        {
            CustomerList.Add(customer);
        }
        public void Add(Parcel parcel)
        {
            ParceList.Add(parcel);
        }
        //שיוך חבילה לרחפן
        public void BelongingParcel(int pId)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == pId);
            foreach (Drone drone in DroneList)
            {
                if (drone.Status == DroneStatuses.Available && drone.MaxWeight >= tempParcel.Weight)
                {
                    tempParcel.DroneId = drone.Id;
                    tempParcel.BelongParcel = DateTime.Now;
                    int x;
                }

                else
                {
                    tempParcel.DroneId = 0;
                }
            }
        }
            //אסיפת חבילה ע"י רחפן
        public void PickingUpParcel(int Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.PickingUp = DateTime.Now;
            ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Delivery);
            ParceList.Add(tempParcel);
        }
        //אספקת חבילה ליעד
        public void DeliveryPackage(int Id)
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
        public void ChangeDroneStatus(int Id, DroneStatuses newStatus)
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
        public void ChargingDrone(int IdDrone)
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
        public void ReleasingDrone(int dId)
        {
            ChangeDroneStatus(dId, DroneStatuses.Available);
            ChargingDroneList.Remove(ChargingDroneList.First(chargingDrone=>chargingDrone.DroneId==dId));
        }
        //----------------------------------------------------לאחד לפונ אחת
        public BaseStation BaseStationDisplay(int id)
        {
            return BaseStationList.First(baseStation => baseStation.Id == id);
        }
        public Drone DroneDisplay(int Id)
        {
            return DroneList.First(drone => drone.Id == Id);
        }
        public Customer CustomerDisplay(string Id)
        {
            return CustomerList.First(customer => customer.Id == Id);
        }
        public Parcel ParcelDisplay(int Id)
        {
            return ParceList.First(parcel => parcel.ParcelId == Id);
        }
        //---------------------------------------------------------------------------------

        //----------------------------------------------------לאחד לפונ אחת

        public List<BaseStation> DisplayingBaseStations()
        {
            return BaseStationList.Select(station => new BaseStation(station)).ToList();
        }
        public List<Drone> DisplayingDrones()
        {
            return DroneList.Select(drone => new Drone(drone)).ToList();

        }
        public List<Customer> DisplayingCustomers()
        {
            return CustomerList.Select(customer => new Customer(customer)).ToList();
        }
        public List<Parcel> DisplayingParcels()
        {
            return ParceList.Select(parcel => new Parcel(parcel)).ToList();
        }
        //-------------------------------------------------------------------------
        public Parcel[] DisplayingUnbelongParcels()
        {
            return ParceList.Where(parcel => parcel.DroneId== 0).ToArray();
        }

        public BaseStation[] AvailableSlots()
        {
            return BaseStationList.Where(BaseStation => BaseStation.NumAvailablePositions > 0).ToArray();
        }
    }
}
