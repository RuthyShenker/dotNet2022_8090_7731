using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using static DalObject.DataSource;

namespace DalObject
{/// <summary>
/// partial class of DalObject:IDal
/// </summary>
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
       /// update parcel is belonged to drone
       /// </summary>
       /// <param name="parcel"></param>
       /// <param name="dId"></param>
        public void UpdateBelongedParcel(Parcel parcel, int dId)
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
            UpdateTimeAction(pId, "pickingUp");
        }

        /// <summary>
        /// A function that gets an id of parcel and the drone that takes this parcel
        /// brings the parcel to the destination.
        /// </summary>
        /// <param name = "pId" ></ param >
        public void ProvidingPackage(int pId)
        {
            UpdateTimeAction(pId, "providing");
        }

        /// <summary>
        /// A function that update Time of Action,the function doesn't return anything.
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="action"></param>
        private void UpdateTimeAction(int pId, string action)
        {
            try
            {
                ParceList.Exists(parcel => parcel.Id == pId);
                for (int i = 0; i < ParceList.Count; i++)
                {
                    if (ParceList[i].Id == pId)
                    {// זה טוב או צריך להשתמש ב removeat
                        Parcel tempParcel = ParceList[i];
                        _ = action switch
                        {
                            var x when x == "pickingUp" => tempParcel.PickingUp = DateTime.Now,
                            _ => tempParcel.Arrival = DateTime.Now,
                        };
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
        /// A function that returns copy parcels that aren't belonged to any drone.
        /// </summary>
        /// <returns> returns copy parcels that aren't belonged to any drone.</returns>
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
    }

}