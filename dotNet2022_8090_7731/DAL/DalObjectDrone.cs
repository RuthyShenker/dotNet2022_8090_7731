using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource.Config;
using IDal.DO;
using static DalObject.DataSource;
namespace DalObject
{
    public partial class DalObjectBaseStation
    {
        public Drone this[int id]
        {
            //set
            //{
            //   Drone drone= DroneList.First(drone => drone.Id == id);
            //   drone
            //}
            get
            {
                return DroneList.First(drone => drone.Id == id);
            }
        }
        /// <summary>
        /// A function that gets a Drone and adds it to the list of Drones.
        /// </summary>
        /// <param name="drone"></param>
        public void AddingDrone(Drone drone)
        {
            DroneList.Add(drone);
        }

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

        /// <summary>
        /// A function that gets an id of droneand releasing this drone from charging.
        /// </summary>
        /// <param name="dId"></param>
        public void ReleasingDrone(int dId)
        {
            for (int i = 0; i < ChargingDroneList.Count; i++)
            {
                if (ChargingDroneList[i].DroneId == dId)
                {
                    ChargingDroneList.RemoveAt(i);
                    break;
                }
            }
            throw new Exception("Id doesnt exist");
        }

        /// <summary>
        /// A function that gets an id of drone and returns this drone-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Drone GetDrone(int Id)
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
        /// A function that returns the list of the drones
        /// </summary>
        /// <returns> drone list</returns>
        public IEnumerable<Drone> GetDrones()
        {
            return DroneList.Select(drone => new Drone(drone)).ToList();

        }

        public double[] PowerConsumptionRequest() => new double[5] { available, lightWeight, mediumWeight, heavyWeight, chargingRate };
        public bool ExistsInDroneList(int id)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i].Id == id)
                    return true;
            }
            return false;
        }
        public int SumOfDronesInSpecificStation(int sId)
        {
            int count = 0;
            for (int i = 0; i < ChargingDroneList.Count; i++)
            {
                if (ChargingDroneList[i].StationId == sId) count++;
            }
            return count;
        }
        void UpdateDrone(int dId, Drone drone)
        {
            DroneList.Remove(DroneList.Find(drone => drone.Id == dId));
            DroneList.Add(drone);
        }
    }
}
