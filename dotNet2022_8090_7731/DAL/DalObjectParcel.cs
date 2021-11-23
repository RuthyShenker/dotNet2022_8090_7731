using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using static DalObject.DataSource;
namespace DalObject
{
    public partial class DalObjectBaseStation
    {
        /// <summary>
        /// A function that gets a Parcel and adds it to the list of Parcels.
        /// </summary>
        /// <param name="parcel"></param>
        public void AddingParcel(Parcel parcel)
        {
            parcel.ParcelId =
            ParceList.Add(parcel);
        }

        ///// <summary>
        ///// A function that gets a id of parcel and belonging that parcel to a drone.
        ///// </summary>
        ///// <param name="pId"></param>
        //public void BelongingParcel(int pId)
        //{
        //    Parcel tempParcel = ParceList.First(parcel => parcel.ParcelId == pId);
        //    foreach (Drone drone in DroneList)
        //    {
        //        if (drone.Status == DroneStatus.Available && drone.MaxWeight >= tempParcel.Weight)
        //        {
        //            tempParcel.DroneId = drone.Id;
        //            tempParcel.BelongParcel = DateTime.Now;
        //        }

        //        else
        //        {
        //            tempParcel.DroneId = 0;
        //        }
        //    }
        //}

        ///// <summary>
        ///// A function that gets an id of parcel and Picking Up this parcel to the drone.
        ///// </summary>
        ///// <param name="Id"></param>
        //public void PickingUpParcel(int Id)
        //{
        //    for (int i = 0; i < ParceList.Count; i++)
        //    {
        //        if (ParceList[i].ParcelId == Id)
        //        {
        //            Parcel tempParcel = ParceList[i];
        //            tempParcel.PickingUp = DateTime.Now;
        //            ChangeDroneStatus(tempParcel.DroneId, DroneStatus.Delivery);
        //            ParceList[i] = tempParcel;
        //            break;
        //        }
        //    }
        //    throw new Exception("Id doesn't exist");
        //}

        ///// <summary>
        ///// A function that gets an id of parcel and the drone that takes this parcel 
        ///// brings the parcel to the destination.
        ///// </summary>
        ///// <param name="Id"></param>
        //public void DeliveryPackage(int Id)
        //{
        //    for (int i = 0; i < ParceList.Count; i++)
        //    {
        //        if (ParceList[i].ParcelId == Id)
        //        {
        //            Parcel tempParcel = ParceList[i];
        //            tempParcel.Arrival = DateTime.Now;
        //            ChangeDroneStatus(tempParcel.DroneId, DroneStatus.Available);
        //            ParceList[i] = tempParcel;
        //            break;
        //        }
        //    }
        //    throw new Exception("Id doesn't exist");
        //}

        /// <summary>
        /// return copy parcels that aren't belonged to any drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetUnbelongParcels()
        {
            return ParceList.Where(parcel => parcel.DroneId == 0).ToList();
        }

        //// יש פונקציות גנריות
        ///// <summary>
        ///// A function that gets an id of parcel and returns this parcel-copied.
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public Parcel GetParcel(int Id)
        //{
        //    for (int i = 0; i < ParceList.Count; i++)
        //    {
        //        if (ParceList[i].Id == Id)
        //        {
        //            return ParceList[i].Clone();
        //        }
        //    }
        //    throw new Exception("id doesnt exist");
        //    //return ParceList.First(parcel => parcel.ParcelId == Id).Clone();

<<<<<<< HEAD
        //}

        ///// <summary>
        ///// A function that returns the list of the parcels
        ///// </summary>
        ///// <returns> parcle list</returns>
        //public IEnumerable<Parcel> GetParcels()
        //{
        //    return ParceList.Select(parcel => new Parcel(parcel)).ToList();
        //}
=======
        /// <summary>
        /// A function that returns the list of the parcels
        /// </summary>
        /// <returns> parcle list</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            return ParceList.Select(parcel => new Parcel(parcel)).ToList();
        }
        public bool ExistsInParcelList(int pId)
        {
            for (int i = 0; i < ParceList.Count; i++)
            {
                if (ParceList[i].ParcelId == pId) return true;
            }
            return false;
        }
>>>>>>> b0b8e234a79098bf1379a2a177505eff6cf6f12b
    }
}
