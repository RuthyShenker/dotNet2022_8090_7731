using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    /// <summary>
    /// A class that contains:
    /// Add
    /// BelongingParcel
    /// PickingUpParcel
    /// DeliveryPackage
    /// ChangeDroneStatus
    /// ChargingDrone
    /// ReleasingDrone
    /// BaseStationDisplay
    /// DroneDisplay
    /// CustomerDisplay
    /// ParcelDisplay
    /// DisplayingBaseStations
    /// DisplayingDrones
    /// DisplayingCustomers
    /// DisplayingParcels
    /// DisplayingUnbelongParcels
    /// AvailableSlots
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// A constructor of DalObject that activates the function Initialize
        /// </summary>
        public DalObject()
        {
            Initialize();
        }

        /// <summary>
        /// A function that gets a base station and adds it to the list of Base Stations.
        /// </summary>
        /// <param name="baseStation"></param>
        public void Add(BaseStation baseStation)
        {
            BaseStationList.Add(baseStation);
        }

        /// <summary>
        /// A function that gets a Drone and adds it to the list of Drones.
        /// </summary>
        /// <param name="drone"></param>
        public void Add(Drone drone)
        {
            DroneList.Add(drone);
        }

        /// <summary>
        /// A function that gets a Customer and adds it to the list of Customers.
        /// </summary>
        /// <param name="customer"></param>
        public void Add(Customer customer)
        {
            CustomerList.Add(customer);
        }

        /// <summary>
        /// A function that gets a Parcel and adds it to the list of Parcels.
        /// </summary>
        /// <param name="parcel"></param>
        public void Add(Parcel parcel)
        {
            ParceList.Add(parcel);
        }

        /// <summary>
        /// A function that gets a id of parcel and belonging that parcel to a drone.
        /// </summary>
        /// <param name="pId"></param>
        public void BelongingParcel(int pId)
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
                    tempParcel.DroneId = 0;
                }
            }
        }
        /// <summary>
        /// A function that gets an id of parcel and Picking Up this parcel to the drone.
        /// </summary>
        /// <param name="Id"></param>
        public void PickingUpParcel(int Id)
        {
            Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == Id);
            ParceList.Remove(ParceList.First(parcel => parcel.ParcelId == Id));
            tempParcel.PickingUp = DateTime.Now;
            ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Delivery);
            ParceList.Add(tempParcel);
        }

        /// <summary>
        /// A function that gets an id of parcel and the drone that takes this parcel 
        /// brings the parcel to the destination.
        /// </summary>
        /// <param name="Id"></param>
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
        ///changes the drone that his Id was given to the new status.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus">from the DroneStatuses-integer </param>
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
        /// <summary>
        /// A function that gets an id of drone and Sends this drone for charging.
        /// </summary>
        /// <param name="IdDrone"></param>
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

        /// <summary>
        /// A function that gets an id of droneand releasing this drone from charging.
        /// </summary>
        /// <param name="dId"></param>
        public void ReleasingDrone(int dId)
        {
            ChangeDroneStatus(dId, DroneStatuses.Available);
            ChargingDroneList.Remove(ChargingDroneList.First(chargingDrone=>chargingDrone.DroneId==dId));
        }

        /// <summary>
        /// A function that gets an id of base station and returns this base station-copied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a base station </returns>
        public BaseStation BaseStationDisplay(int id)
        {
            return BaseStationList.First(baseStation => baseStation.Id == id).Clone();
        }

        /// <summary>
        /// A function that gets an id of drone and returns this drone-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Drone DroneDisplay(int Id)
        {
            return DroneList.First(drone => drone.Id == Id).Clone();
        }
        /// <summary>
        /// A function that gets an id of customer and returns this customer-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer CustomerDisplay(string Id)
        {
            return CustomerList.First(customer => customer.Id == Id).Clone();
        }

        /// <summary>
        /// A function that gets an id of parcel and returns this parcel-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Parcel ParcelDisplay(int Id)
        {
            return ParceList.First(parcel => parcel.ParcelId == Id).Clone();
        }
        //---------------------------------------------------------------------------------

        //----------------------------------------------------Connect to one function?

        /// <summary>
        /// A function that returns the list of the base stations 
        /// </summary>
        /// <returns> base station list</returns>
        public List<BaseStation> DisplayingBaseStations()
        {
            return BaseStationList.Select(station => new BaseStation(station)).ToList();
        }
        /// <summary>
        /// A function that returns the list of the drones
        /// </summary>
        /// <returns> drone list</returns>
        public List<Drone> DisplayingDrones()
        {
            return DroneList.Select(drone => new Drone(drone)).ToList();

        }

        /// <summary>
        /// A function that returns the list of the customers
        /// </summary>
        /// <returns> customer list</returns>
        public List<Customer> DisplayingCustomers()
        {
            return CustomerList.Select(customer => new Customer(customer)).ToList();
        }

        /// <summary>
        /// A function that returns the list of the parcels
        /// </summary>
        /// <returns> parcle list</returns>
        public List<Parcel> DisplayingParcels()
        {
            return ParceList.Select(parcel => new Parcel(parcel)).ToList();
        }
        
        /// <summary>
        /// A function that returns parcels that aren't belonged to any drone.
        /// </summary>
        /// <returns></returns>
        public List<Parcel> DisplayingUnbelongParcels()
        {
            return ParceList.Where(parcel => parcel.DroneId== 0).ToList();
        }

        /// <summary>
        /// A function that returns base stations that they have available charging positions.
        /// </summary>
        /// <returns>list of base stations that they have available charging positions</returns>
        public List<BaseStation> AvailableSlots()
        {
            return BaseStationList.Where(BaseStation => BaseStation.NumAvailablePositions > 0).ToList();
        }
    }
}
