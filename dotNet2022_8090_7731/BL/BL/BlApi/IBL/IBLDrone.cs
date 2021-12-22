using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBLDrone
    {
        void AddingDrone(Drone drone, int numberStation);
        void UpdatingDroneName(int droneId, string newModel);
        void SendingDroneToCharge(int IdDrone);
        void ReleasingDrone(int dId);
        IEnumerable<DroneToList> GetDrones(Func<DroneToList, bool> predicate = null);
        Drone GetDrone(int droneId);
        bool IsDroneExist(int id);
    }
}