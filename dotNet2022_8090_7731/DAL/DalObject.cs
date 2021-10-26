using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using static DalObject.DataSource;
namespace DalObject
{
    public class DalObject : IDal.IDal
    {
        public DalObject()
        {
            Initialize();
        }
        public void AddingBaseStation(object baseStation)
        {
            BaseStationList.Add((BaseStation)baseStation);
        }
        public void AddingDrone(object drone)
        {
            DroneList.Add((Drone)drone);
        }
        public void AddingCustomer(object customer)
        {
            CustomerList.Add((Customer)customer);
        }

        public void GettingParcelForDelivery(object parcel)
        {
            ParceList.Add((Parcel)parcel);
        }

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

        public void PickingUpParcel(string Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.PickingUp = DateTime.Now;
            ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Delivery);
            ParceList.Add(tempParcel);
        }

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
        public void ChangeDroneStatus(string Id, object newStatus)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (Id == DroneList[i].Id)
                {
                    Drone changeDrone = DroneList[i];
                    changeDrone.Status = (DroneStatuses)newStatus;
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
            ChargingDroneList.Remove(ChargingDroneList.First(chargingDrone => chargingDrone.DroneId == dId));

        }
        //----------------------------------------------------לאחד לפונ אחת

        public object BaseStationDisplay(string id)
        {
            return BaseStationList.First(baseStation => baseStation.Id == id);
        }
        public object DroneDisplay(string Id)
        {
            return DroneList.First(drone => drone.Id == Id);
        }
        public object CustomerDisplay(string Id)
        {
            return CustomerList.First(customer => customer.Id == Id);
        }
        public object ParcelDisplay(string Id)
        {
            return ParceList.First(parcel => parcel.ParcelId == Id);

        }
        //---------------------------------------------------------------------------------

        //----------------------------------------------------לאחד לפונ אחת
        public IEnumerable<BaseStation> DisplayingBaseStations()
        {
            return new List<BaseStation>(BaseStationList.Select(baseStation=> new BaseStation(baseStation)).ToList());
        }

        public object[] DisplayingDrones()
        {
            return DroneList.ToArray();

        }

        public object[] DisplayingCustomers()
        {
            return CustomerList.ToArray();

        }

        public object[] DisplayingParcels()
        {
            return ParceList.ToArray();
        }
        //-------------------------------------------------------------------------

        public object[] DisplayingUnbelongParcels()
        {
            return ParceList.Where(parcel => int.Parse(parcel.DroneId) == 0).ToArray();
        }

        public object[] AvailableSlots()
        {
            return BaseStationList.Where(BaseStation => BaseStation.NumAvailablePositions > 0).ToArray();

        }
    }




}
