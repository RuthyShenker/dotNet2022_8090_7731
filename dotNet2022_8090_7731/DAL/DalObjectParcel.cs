using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDal.DO;
using IDAL.DO;
using static DalObject.DataSource;
namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// A function that gets a Parcel and adds it to the list of Parcels.
        /// </summary>
        /// <param name="parcel"></param>
        public void AddingParcel(Parcel parcel)
        {
            ParceList.Add(parcel);
        }

        /// <summary>
        /// A function that gets a id of parcel and belonging that parcel to a drone.
        /// </summary>
        /// <param name="pId"></param>
        public void BelongingParcel(Parcel parcel, int dId)
        {
            parcel.DroneId = dId;
            parcel.BelongParcel = DateTime.Now;
            for (int i = 0; i < ParceList.Count; i++)
            {
                if (ParceList[i].Id == parcel.Id)
                {
                    ParceList.RemoveAt(i);
                    break;
                }
            }
            ParceList.Add(parcel);
        }

        /// <summary>
        /// A function that gets an id of parcel and Picking Up this parcel to the drone.
        /// </summary>
        /// <param name="Id"></param>
        public void PickingUpParcel(int pId)
        {
            updateTimeAction(pId, "pickingUp");
        }

        /// <summary>
        /// A function that gets an id of parcel and the drone that takes this parcel
        /// brings the parcel to the destination.
        /// </summary>
        /// <param name = "pId" ></ param >
        public void ProvidePackage(int pId)
        {
            updateTimeAction(pId, "provide");
        }

        private void updateTimeAction(int pId, string action)
        {
            try
            {
                ParceList.Exists(parcel => parcel.Id == pId);
           
                for (int i = 0; i < ParceList.Count; i++)
                {
                    if (ParceList[i].Id == pId)
                    {// זה טוב או צריך להשתמש ב removeat
                        Parcel tempParcel = ParceList[i];
                        if (action == "pickingUp")
                        {
                            tempParcel.PickingUp = DateTime.Now;
                        }
                        else
                        {
                           tempParcel.Arrival = DateTime.Now;
                        }
                        ParceList[i] = tempParcel;
                        break;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                throw new IdNotExistInTheListException();
            }
        }


        /// <summary>
        /// return copy parcels that aren't belonged to any drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetUnbelongParcels()
        {
            return new List<Parcel>(ParceList.Where(parcel => parcel.DroneId == 0).ToList());
        }

        //// יש פונקציות גנריות
        ///// <summary>
        ///// A function that gets an id of parcel and returns this parcel-copied.
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        public Parcel GetParcel(int Id)
        {
            for (int i = 0; i < ParceList.Count; i++)
            {
                if (ParceList[i].Id == Id)
                {
                    return ParceList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
            //return ParceList.First(parcel => parcel.ParcelId == Id).Clone();


            //}

            ///// <summary>
            ///// A function that returns the list of the parcels
            ///// </summary>
            ///// <returns> parcle list</returns>
            //public IEnumerable<Parcel> GetParcels()
            //{
            //    return ParceList.Select(parcel => new Parcel(parcel)).ToList();
            //}

            ///// <summary>
            ///// A function that returns the list of the parcels
            ///// </summary>
            ///// <returns> parcle list</returns>
            //public IEnumerable<Parcel> GetParcels()
            //{
            //    return ParceList.Select(parcel => new Parcel(parcel)).ToList();
            //}
            public bool ExistsInParcelList(int pId)
        {
            for (int i = 0; i < ParceList.Count; i++)
            {
                if (ParceList[i].Id == pId) return true;
            }
            return false;
        }

    }

}