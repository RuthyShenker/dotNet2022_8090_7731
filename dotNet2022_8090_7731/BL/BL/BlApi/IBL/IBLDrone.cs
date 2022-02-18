﻿using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    /// <summary>
    /// interface IBLDrone includes:
    /// AddingDrone
    /// UpdatingDroneName
    /// SendingDroneToCharge
    /// ReleasingDrone
    /// GetDrones
    /// GetDrone
    /// DeleteDrone
    /// StartSimulator
    /// GetPowerConsumption
    /// </summary>
    public interface IBLDrone
    {
        //ADD:
        void AddingDrone(Drone drone, int numberStation);

        //UPDATE:
        void UpdatingDroneName(int droneId, string newModel);
        void SendingDroneToCharge(int IdDrone);
        void ReleasingDrone(int dId);

        //GET:
        IEnumerable<DroneToList> GetDrones(Func<DroneToList, bool> predicate = null);
        Drone GetDrone(int droneId);

        //DELETE:
        string DeleteDrone(int customerId);


        //bool IsDroneExist(int id);
        void StartSimulator(int droneId, Action<object> updateView, Func<bool> checkStop);

        IEnumerable<double> GetPowerConsumption();
        

    }
}