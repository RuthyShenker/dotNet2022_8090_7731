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
        public void AddingBaseStation(object baseStation)
        {
            BaseStationList.Add((BaseStation)baseStation);
        }

        /// <summary>
        /// A function that gets a Drone and adds it to the list of Drones.
        /// </summary>
        /// <param name="drone"></param>
        public void AddingDrone(object drone)
        {
            DroneList.Add((Drone)drone);
        }
        /// <summary>
        /// A function that gets a Customer and adds it to the list of Customers.
        /// </summary>
        /// <param name="customer"></param>
        public void AddingCustomer(object customer)
        {
            CustomerList.Add((Customer)customer);
        }

        /// <summary>
        /// A function that gets a Parcel and adds it to the list of Parcels.
        /// </summary>
        /// <param name="parcel"></param>
        public void GettingParcelForDelivery(object parcel)
        {
            ParceList.Add((Parcel)parcel);
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
            for (int i = 0; i < ParceList.Count; i++)
            {
                if(ParceList[i].ParcelId == Id)
                {
                    Parcel tempParcel = ParceList[i];
                    tempParcel.PickingUp = DateTime.Now;
                    ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Delivery);
                    ParceList[i] = tempParcel;
                    break;
                }
            }
            throw new Exception("Id doesn't exist");
        }

        /// <summary>
        /// A function that gets an id of parcel and the drone that takes this parcel 
        /// brings the parcel to the destination.
        /// </summary>
        /// <param name="Id"></param>
        public void DeliveryPackage(int Id)
        {
            for (int i = 0; i < ParceList.Count; i++)
            {
                if(ParceList[i].ParcelId == Id)
                {
                    Parcel tempParcel = ParceList[i];
                    tempParcel.Arrival = DateTime.Now;
                    ChangeDroneStatus(tempParcel.DroneId, DroneStatuses.Available);
                    ParceList[i] = tempParcel;
                    break;
                }
            }
            throw new Exception("Id doesn't exist");
        }

        /// <summary>
        ///A function that gets an integer that means a new status and Id of drone and 
        ///changes the drone that his Id was given to the new status.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus"></param>
        public void ChangeDroneStatus(int Id, object newStatus)
        /// <param name="newStatus">from the DroneStatuses-integer </param>
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (Id == DroneList[i].Id)
                {
                    Drone changeDrone = DroneList[i];
                    changeDrone.Status = (DroneStatuses)newStatus;
                    DroneList[i] = changeDrone;
                    break;
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

        
        //----------------------------------------------------לאחד לפונ אחת

        /// <summary>
        /// A function that gets an id of droneand releasing this drone from charging.
        /// </summary>
        /// <param name="dId"></param>
        public void ReleasingDrone(int dId)
        {
            ChangeDroneStatus(dId, DroneStatuses.Available);
            for (int i = 0; i < ChargingDroneList.Count; i++)
            {
                if (ChargingDroneList[i].DroneId==dId)
                {
                    ChargingDroneList.RemoveAt(i);
                    break;
                }
            }
            throw new Exception("Id doesnt exist");
        }

        /// <summary>
        /// A function that gets an id of base station and returns this base station-copied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a base station </returns>
        /// 
        public BaseStation BaseStationDisplay(int id)
        {
            for (int i = 0; i < BaseStationList.Count; i++)
            {
                if (BaseStationList[i].Id == id)
                {
                    return BaseStationList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
        }

        /// <summary>
        /// A function that gets an id of drone and returns this drone-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Drone DroneDisplay(int Id)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i].Id == Id)
                {
                    return DroneList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
            //return DroneList.First(drone => drone.Id == Id).Clone();
        }
        /// <summary>
        /// A function that gets an id of customer and returns this customer-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer CustomerDisplay(string Id)
        {
            for (int i = 0; i < CustomerList.Count; i++)
            {
                if (CustomerList[i].Id == Id)
                {
                    return CustomerList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
            //return CustomerList.First(customer => customer.Id == Id).Clone();

        }
        

        /// <summary>
        /// A function that gets an id of parcel and returns this parcel-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Parcel ParcelDisplay(int Id)
        {
            for (int i = 0; i < ParceList.Count; i++)
            {
                if (ParceList[i].ParcelId == Id)
                {
                    return ParceList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
            //return ParceList.First(parcel => parcel.ParcelId == Id).Clone();

        }

        //----------------------------------------------------Connect to one function?

        /// <summary>
        /// A function that returns the list of the base stations 
        /// </summary>
        /// <returns> base station list</returns>
        public IEnumerable<BaseStation> DisplayingBaseStations()
        {
            return new List<BaseStation>(BaseStationList.Select(station => new BaseStation(station)).ToList());
        }
        /// <summary>
        /// A function that returns the list of the drones
        /// </summary>
        /// <returns> drone list</returns>
        public IEnumerable<Drone> DisplayingDrones()
        {
            return new List<Drone>(DroneList.Select(drone => new Drone(drone)).ToList());

        }

        /// <summary>
        /// A function that returns the list of the customers
        /// </summary>
        /// <returns> customer list</returns>
        public IEnumerable<Customer> DisplayingCustomers()
        {
            return new List<Customer>(CustomerList.Select(customer => new Customer(customer)).ToList());
        }

        /// <summary>
        /// A function that returns the list of the parcels
        /// </summary>
        /// <returns> parcle list</returns>
        public IEnumerable<Parcel> DisplayingParcels()
        {
            return new List<Parcel>(ParceList.Select(parcel => new Parcel(parcel)).ToList());
        }
        //-----------------------------------------------------------------------------------

        
        /// <summary>
        /// A function that returns parcels that aren't belonged to any drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplayingUnbelongParcels()
        {
            return new List<Parcel>(ParceList.Where(parcel => parcel.DroneId== 0).ToList());
        }


        /// <summary>
        /// A function that returns base stations that they have available charging positions.
        /// </summary>
        /// <returns>list of base stations that they have available charging positions</returns>
        public IEnumerable<BaseStation> AvailableSlots()
        {
            return new List<BaseStation>(BaseStationList.Where(BaseStation => BaseStation.NumAvailablePositions > 0).ToList());
        }
    }
}
