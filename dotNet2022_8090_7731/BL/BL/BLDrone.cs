using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    partial class BL
    {
        IEnumerable<DroneToList> GetDrones()
        {
            IEnumerable<DroneToList> ldroneToList = new List<DroneToList>();
            IEnumerable<IDal.DO.Drone> lDrones = dal.GetDrones();
            DroneToList droneToList = new DroneToList(); 
            foreach (var drone in lDrones)
            {
                droneToList.Id = drone.Id;
                droneToList.Model = drone.Model;
                droneToList.Weight = (IBL.BO.Enum.WeightCategories)drone.MaxWeight;
               
                droneToList.BatteryStatus =ldal.
                droneToList.DStatus =ldal.
                droneToList.CurrLocation =ldal.
                droneToList.NumOfParcel =ldal.

            }

        }


    }
}
