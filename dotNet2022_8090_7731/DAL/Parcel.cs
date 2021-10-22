using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        /// <summary>
        /// A struct of Parcel
        /// </summary>
        public struct Parcel
        {
            
            /// <summary>
            /// A constructor of Parcel that gets params 
            /// </summary>
            /// <param name="senderId"></param>
            /// <param name="getterId"></param>
            /// <param name="weight"></param>
            /// <param name="status"></param>
            /// <param name="makingParcel"></param>
            /// <param name="belongParcel"></param>
            /// <param name="pickingUp"></param>
            /// <param name="arrival"></param>
            public Parcel(string senderId, string getterId, WeightCategories weight, UrgencyStatuses status, 
                DateTime makingParcel,DateTime belongParcel, DateTime pickingUp, DateTime arrival)
            {
                ParcelId = ++DataSource.Config.IndexParcel;
                SenderId = senderId;
                GetterId = getterId;
                Weight = weight;
                Status = status;
                DroneId = 0;
                MakingParcel = makingParcel;
                BelongParcel = belongParcel;
                PickingUp = pickingUp;
                Arrival = arrival;
            }
            /// <summary>
            /// A constructor of parcel that gets a parcel and copies the fields
            /// </summary>
            /// <param name="parcel">an object</param>
            public Parcel(Parcel parcel)
            {
                ParcelId = parcel.ParcelId;
                SenderId = parcel.SenderId;
                GetterId = parcel.GetterId;
                Weight = parcel.Weight;
                Status = parcel.Status;
                DroneId = parcel.DroneId;
                MakingParcel = parcel.MakingParcel;
                BelongParcel = parcel.BelongParcel;
                PickingUp = parcel.PickingUp;
                Arrival = parcel.Arrival;
            }
          
            /// <summary>
            /// this field is init
            /// </summary>
            public int ParcelId { get; init; }
            public string SenderId { get; set; }
            public string GetterId { get; set; }
            public WeightCategories Weight { get; set; }
            public UrgencyStatuses Status { get; set; }
            public int DroneId { get; set; }

            /// <summary>
            /// Time of creation a package to delivery
            /// </summary>
            public DateTime MakingParcel { get; set; }
            /// <summary>
            /// Time of belonging A package to drone
            /// </summary>
            public DateTime BelongParcel { get; set; }
            /// <summary>
            /// Time of collecting the parcel frome the sender
            /// </summary>
            public DateTime PickingUp { get; set; }
            //Time of arriving the package to the getter
            public DateTime Arrival { get; set; }
           /// <summary>
           /// A function that prints all the fields of the package
           /// </summary>
           /// <returns></returns>
            public override string ToString()
            {
                return $"Parcel Id: {ParcelId}    SenderId: {SenderId}    GetterId: {GetterId}    " +
                    $"Parcel weight: {Weight}    Priority: {Status}    DroneId: {DroneId}    " +
                    $"Making parcel: {MakingParcel}    Arrival: {Arrival}";
                //    Belong parcel:{BelongParcel}    $"Picking up: {PickingUp}" +
            }
        }
    }
}
