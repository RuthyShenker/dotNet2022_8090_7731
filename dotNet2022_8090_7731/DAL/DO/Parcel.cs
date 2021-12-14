using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// A struct of Parcel contains:
        /// Id of Parcel, Id of Sender, Id of Getter, Weight, Status, Id of Drone.
        /// </summary>
        public struct Parcel:IIdentifiable,IDalObject
        {
            /// <summary>
            /// A constructor of Parcel that gets parameters 
            /// and initalizes the new instance with this 
            /// parameters. 
            /// </summary>
            /// <param name="senderId"></param>
            /// <param name="getterId"></param>
            /// <param name="weight"></param>
            /// <param name="status"></param>
            /// <param name="makingParcel"></param>
            /// <param name="belongParcel"></param>
            /// <param name="pickingUp"></param>
            /// <param name="arrival"></param>
            public Parcel(int senderId, int getterId, WeightCategories weight, UrgencyStatuses status,
                DateTime makingParcel, DateTime belongParcel, DateTime pickingUp, DateTime arrival)
            {
                Id = ++DataSource.Config.IndexParcel;
                SenderId = senderId;
                GetterId = getterId;
                Weight = weight;
                MPriority = status;
                DroneId = 0;
                CreatedTime = makingParcel;
                BelongParcel = belongParcel;
                PickingUp = pickingUp;
                Arrival = arrival;
            }

            /// <summary>
            /// A constructor of parcel that gets 
            /// an instance parcel and initalizes
            /// the new instance with the parameters of this instance.
            /// </summary>
            /// <param name="parcel">an object</param>
            public Parcel(Parcel parcel)
            {
                Id = parcel.Id;
                SenderId = parcel.SenderId;
                GetterId = parcel.GetterId;
                Weight = parcel.Weight;
                MPriority = parcel.MPriority;
                DroneId = parcel.DroneId;
                CreatedTime = parcel.CreatedTime;
                BelongParcel = parcel.BelongParcel;
                PickingUp = parcel.PickingUp;
                Arrival = parcel.Arrival;
            }

            /// <summary>
            /// this field is init
            /// </summary>
            public int Id { get; init; }

            public int SenderId { get; set; }

            public int GetterId { get; set; }

            public WeightCategories Weight { get; set; }

            public UrgencyStatuses MPriority { get; set; }

            public int DroneId { get; set; }

            /// <summary>
            /// Time of creation a package to delivery
            /// </summary>
            public DateTime CreatedTime { get; set; }

            /// <summary>
            /// Time of belonging A package to drone
            /// </summary>
            public DateTime? BelongParcel { get; set; }

            /// <summary>
            /// Time of collecting the parcel frome the sender
            /// </summary>
            public DateTime? PickingUp { get; set; }

            /// <summary>
            /// Time of arriving the package to the getter
            /// </summary>
            public DateTime? Arrival { get; set; }

            /// <summary>
            /// A function that returns the details of the Parcel
            /// </summary>
            /// <returns>The details</returns>
            public override string ToString()
            {
                return $"Parcel Id: {Id}    SenderId: {SenderId}   " +
                    $" GetterId: {GetterId}  Parcel weight: {Weight} " +
                    $"Priority: {MPriority}    DroneId: {DroneId} " +
                    $"Created Time parcel: {CreatedTime}  Belong parcel:{BelongParcel}   " +
                    $"Picking up: {PickingUp}   Arrival: {Arrival} ";
            }
        }
    }
}
