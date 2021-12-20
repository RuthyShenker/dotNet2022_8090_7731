using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource.Config;
using IDAL.DO;
using static DalObject.DataSource;
using System.Collections;

namespace DalObject
{/// <summary>
/// partial class of DalObject:IDal
/// </summary>
    public sealed partial class DalObject
    {
        /// <summary>
        /// A function that returnsthe data of : available, lightWeight,
        ///mediumWeight, heavyWeight, chargingRate
        /// </summary>
        /// <returns>returns an array of double that contains:available, lightWeight,
        ///mediumWeight, heavyWeight, chargingRate</returns>
        public double[] PowerConsumptionRequest() => new double[5] { Available, LightWeight,MediumWeight, HeavyWeight, ChargingRate };

        #region canErase?




        //public Drone this[int id]
        //{
        //    //set
        //    //{
        //    //   Drone drone= DroneList.First(drone => drone.Id == id);
        //    //   drone
        //    //}
        //    get
        //    {
        //        return DroneList.First(drone => drone.Id == id);
        //    }
        //}
        /// <summary>
        /// A function that gets a Drone and adds it to the list of Drones,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="drone"></param>
        //public void AddingDrone(Drone drone)
        //{
        //    DroneList.Add(drone);
        //}
        /// <summary>
        /// A function that gets :
        /// drone Id,station Id
        ///and adds a ChargingDrone object contains these fields to 
        ///the list ChargingDroneList,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        /// <param name="sId"></param>
        //public void AddingDroneToCharge(int dId, int sId)
        //{
        //    ChargingDroneList.Add(new ChargingDrone(dId, sId));
        //}

        /// <summary>
        /// A function that returns new list of all Charging Drone.
        /// </summary>
        ///// <returns>returns new list of all the Charging Drone  </returns>
        //public IEnumerable<ChargingDrone> GetChargingDrones()
        //{
        //    return new List<ChargingDrone>(ChargingDroneList);
        //}


        /// <summary>
        ///A function that gets an integer that means a new status and Id of drone and 
        ///changes the drone that his Id was given to the new status.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus"></param>
        //public void ChangeDroneStatus(int Id, DroneStatus newStatus)
        ///// <param name="newStatus">from the DroneStatuses-integer </param>
        //{
        //    for (int i = 0; i < DroneList.Count; i++)
        //    {
        //        if (Id == DroneList[i].Id)
        //        {
        //            Drone changeDrone = DroneList[i];
        //            changeDrone.Status = newStatus;
        //            DroneList[i] = changeDrone;
        //            break;
        //        }
        //    }
        //    throw new Exception("Not Exist Drone With This Id");
        //}


        /// <summary>
        /// A function that gets an id of drone and Sends this drone for charging.
        /// </summary>
        /// <param name="IdDrone"></param>
        //public void ChargingDrone(int IdDrone)
        //{
        //    foreach (BaseStation baseStation in BaseStationList)
        //    {
        //        if (baseStation.NumberOfChargingPositions != 0)
        //        {
        //            ChargingDrone newChargingEntity = new ChargingDrone();
        //            newChargingEntity.StationId = baseStation.Id;
        //            newChargingEntity.DroneId = IdDrone;
        //            ChargingDroneList.Add(newChargingEntity);
        //            return;
        //        }
        //    }
        //    throw new Exception("There Are No Available Positions");
        //}


        ///// <summary>
        ///// A function that gets an id of drone and returns this drone-copied.
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public Drone GetDrone(int Id)
        //{
        //    for (int i = 0; i < DroneList.Count; i++)
        //    {
        //        if (DroneList[i].Id == Id)
        //        {
        //            return DroneList[i].Clone();
        //        }
        //    }
        //    throw new IdNotExistInTheListException();
        //    //return DroneList.First(drone => drone.Id == Id).Clone();
        //}


        ///// <summary>
        ///// A function that returns the list of the drones
        ///// </summary>
        ///// <returns> drone list</returns>
        //public IEnumerable<Drone> GetDrones()
        //{
        //    return DroneList.Select(drone => new Drone(drone)).ToList();

        //}
        //public IEnumerable<Parcel> GetParcels()
        //{
        //    return ParceList.Select(parcel => new Parcel(parcel)).ToList();

        //}
        //public IEnumerable<Customer> GetCustomers()
        //{
        //    return CustomerList.Select(customer => new Customer(customer)).ToList();

        //}
        //public IEnumerable<Station> GetStations()
        //{
        //    return BaseStationList.Select(Station => new Station(Station)).ToList();

        //}
        /// <summary>
        /// A function that gets an id of drone 
        /// and a drone and updates the data base to 
        /// contains this object of this id,the function
        /// doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        /// <param name="drone"></param>
        //public void UpdateDrone(int dId, Drone drone)
        //{
        //    DroneList.Remove(DroneList.Find(drone => drone.Id == dId));
        //    DroneList.Add(drone);
        //}

        #endregion
    }
}
