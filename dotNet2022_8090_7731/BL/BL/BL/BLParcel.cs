using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// A function that gets a parcel and adds it to the data base,the function
        /// doesn't return anything.
        /// </summary>
        /// <param name="parcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddingParcel(Parcel parcel)
        {
            DO.Customer sender;
            DO.Customer getter;
            try
            {
                sender = dal.GetFromDalById<DO.Customer>(parcel.Sender.Id);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(DO.Customer), parcel.Sender.Id);
            }

            try
            {
                getter = dal.GetFromDalById<DO.Customer>(parcel.Getter.Id);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(DO.Customer), parcel.Getter.Id);
            }

            int parcelId = dal.GetIndexParcel();
            if (!dal.IsIdExistInList<DO.Parcel>(parcelId))
            {
                dal.Add(new DO.Parcel()
                {
                    Id = parcelId,
                    SenderId = parcel.Sender.Id,
                    GetterId = parcel.Getter.Id,
                    Weight = (DO.WeightCategories)parcel.Weight,
                    MPriority = (DO.UrgencyStatuses)parcel.MPriority,
                    DroneId = null,
                    CreatedTime = DateTime.Now,
                    BelongParcel = null,
                    PickingUp = null,
                    Arrival = null,
                });
            }

            return parcelId;
        }

        /// <summary>
        /// A function that gets an id of drone and belonging to it a parcel.
        /// </summary>
        /// <param name="dId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BelongingParcel(int dId)
        {
            DroneToList droneToList = FindDroneInList(dId);
            if (droneToList.DStatus != DroneStatus.Free)
            {
                string dStatus = droneToList.DStatus.ToString();
                throw new BO.InValidActionException(typeof(Drone), dId, $"status of drone is {dStatus} ");
            }

            if (!dal.GetListFromDal<DO.Parcel>().Any())
            {
                throw new BO.ListIsEmptyException(typeof(DO.Parcel));
            }

            var optionParcels = OptionalParcelsForSpecificDrone(droneToList.BatteryStatus, droneToList.Weight, droneToList.CurrLocation).ToList();
            var parcel = optionParcels.FirstOrDefault(parcel => !parcel.BelongParcel.HasValue);

            if (!optionParcels.Any() || parcel.Equals(default))
            {
                throw new ThereIsNoMatchObjectInListException(typeof(DO.Parcel), $"There is no match parcels to drone with id {dId} in");
            }

            droneToList.DStatus = DroneStatus.Delivery;

            dal.Update<DO.Parcel>(parcel.Id, dId, nameof(parcel.DroneId));
            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.BelongParcel));
        }

        //TODO what happens if the list is empty
        internal IOrderedEnumerable<DO.Parcel> OptionalParcelsForSpecificDrone(double batteryStatus, WeightCategories weight, Location currLocation)
        {
            //MinBattery(CalculateDistance(currLocation, destination), (WeightCategories)parcel.Weight)
            //    + MinBattery(CalculateDistance(destination, nearestDestinationStation))

            return dal.GetDalListByCondition<DO.Parcel>
                                (parcel => parcel.Weight <= (DO.WeightCategories)weight
                                &&
                                batteryStatus >=
                                MinBattery(
                                    Extensions.CalculateDistance(currLocation, GetCustomer(parcel.SenderId).Location)
                                    )
                                + MinBattery(
                                    Extensions.CalculateDistance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.GetterId).Location), (WeightCategories)parcel.Weight
                                    ) +
                                MinBattery(
                                    Extensions.CalculateDistance(GetCustomer(parcel.GetterId).Location, ClosestStation(GetCustomer(parcel.GetterId).Location).Location)
                                    )
                                )
                                ?.OrderByDescending(parcel => parcel.MPriority)
                                .ThenByDescending(parcel => parcel.Weight)
                                .ThenBy(parcel => GetDistance(currLocation, parcel));
        }

        /// <summary>
        /// A function that gets an id of drone and
        /// causes the drone to pick up the parcel that 
        /// it needs to take to the destination,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickingUpParcel(int dId)
        {
            var drone = FindDroneInList(dId);

            if (drone.DStatus != DroneStatus.Delivery)
            {
                throw new InValidActionException(typeof(DO.Drone), dId, $"status of drone is {drone.DStatus} ");
            }
            if (!drone.DeliveredParcelId.HasValue)
            {
                throw new InValidActionException(typeof(DO.Drone), dId, $"there is no assined parcel ");
            }

            DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<DO.Parcel>(drone.DeliveredParcelId.Value);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), drone.DeliveredParcelId.Value);
            }

            if (parcel.PickingUp != null)
            {
                throw new InValidActionException("Parcel assigned to drone was already picked up");
            }

            Location senderLocation = GetCustomer(parcel.SenderId).Location;
            drone.BatteryStatus -= MinBattery(Extensions.CalculateDistance(drone.CurrLocation, senderLocation));
            drone.CurrLocation = senderLocation;
            // לא היה כתוב לשנות status
            drone.DStatus = DroneStatus.Delivery;
            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.PickingUp));
        }

        /// <summary>
        /// A function that gets an id of drone
        /// and causes this drone to Delivery the Package
        /// that it needs to take 
        /// to the destination,the function doesn't return anything.
        /// </summary>
        /// <param name="dId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryPackage(int dId)
        {
            var drone = FindDroneInList(dId);

            if (drone.DeliveredParcelId == null)
            {
                throw new InValidActionException(typeof(Drone), dId, $"There is no assigned parcel ");
            }
            if (drone.DStatus != DroneStatus.Delivery)
            {
                throw new InValidActionException(typeof(Drone), dId, $"status of drone is {drone.DStatus} ");
            }

            DO.Parcel parcel;
            try
            {
                parcel = dal.GetFromDalById<DO.Parcel>(drone.DeliveredParcelId.Value);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), drone.DeliveredParcelId.Value);
            }

            if (parcel.PickingUp == null)
            {
                throw new InValidActionException("Parcel assigned to drone had not been picked up yet");
            }
            if (parcel.Arrival != null)
            {
                throw new InValidActionException("Parcel assigned has already supplied to destination");
            }

            Location getterLocation = GetCustomer(parcel.GetterId).Location;
            drone.BatteryStatus -= MinBattery(Extensions.CalculateDistance(drone.CurrLocation, getterLocation));
            drone.CurrLocation = getterLocation;
            drone.DStatus = DroneStatus.Free;
            drone.DeliveredParcelId = null;
            dal.Update<DO.Parcel>(parcel.Id, DateTime.Now, nameof(parcel.Arrival));
            dal.Update<DO.Parcel>(parcel.Id, null, nameof(parcel.DroneId));

        }

        /// <summary>
        /// A function that gets IDAL.DO.Parcel object and returns its status
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelStatus of specific parcel </returns>
        private ParcelStatus GetParcelStatus(DO.Parcel parcel)
        {
            if (parcel.Arrival.HasValue)
            {
                return ParcelStatus.InDestination;
            }
            else if (parcel.PickingUp.HasValue)
            {
                return ParcelStatus.collected;
            }
            else if (parcel.BelongParcel.HasValue)
            {
                return ParcelStatus.belonged;
            }
            else
            {
                return ParcelStatus.made;
            }
        }

        //------Get-------------------------------------------------------

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcels()
        {
            return dal.GetListFromDal<DO.Parcel>().Select(s => ConvertToList(s));
        }

        /// <summary>
        /// A function that returns a list of unbelong parcels.
        /// </summary>
        /// <returns>returns a list of unbelong parcels.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            try
            {
                return dal.GetDalListByCondition<DO.Parcel>(parcel => parcel.DroneId == 0)
                    .Select(parcel => ConvertToList(parcel));
            }

            catch (DO.InValidActionException)
            {
                throw new InValidActionException("There is no match object in the list ");
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Parcel
        /// and Expands it to a new object of 
        /// ParcelToList and returns it.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>returns ParcelToList object</returns>
        private ParcelToList ConvertToList(DO.Parcel parcel)
        {
            string senderName = dal.GetFromDalById<DO.Customer>(parcel.SenderId).Name;
            string getterName = dal.GetFromDalById<DO.Customer>(parcel.GetterId).Name;

            ParcelToList nParcel = new()
            {
                Id = parcel.Id,
                SenderName = senderName,
                GetterName = getterName,
                Weight = (WeightCategories)parcel.Weight,
                MyPriority = (Priority)parcel.MPriority,
                Status = GetParcelStatus(parcel)
            };
            return nParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                var dParcel = dal.GetFromDalById<DO.Parcel>(parcelId);
                return ConvertToBL(dParcel);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Parcel), parcelId);
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Parcel
        /// and Expands it to a new object of 
        /// Parcel and returns it.
        /// <param name="parcel"></param>
        /// <returns>returns Parcel object</returns>
        private Parcel ConvertToBL(DO.Parcel parcel)
        {
            CustomerInParcel sender = NewCustomerInParcel(parcel.SenderId);
            CustomerInParcel getter = NewCustomerInParcel(parcel.GetterId);
            DroneInParcel dInParcel = ConvertDroneInParcel(parcel.DroneId);
            return new Parcel()
            {
                Id = parcel.Id,
                Sender = sender,
                Getter = getter,
                Weight = (WeightCategories)parcel.Weight,
                MPriority = (Priority)parcel.MPriority,
                DInParcel = dInParcel,
                MakingParcel = parcel.CreatedTime,
                BelongParcel = parcel.BelongParcel,
                PickingUp = parcel.PickingUp,
                Arrival = parcel.Arrival
            };
        }

        /// <summary>
        /// A function that gets id of drone and bulids from it an object of 
        /// DroneInParcel and of course considering logic and returns 
        /// the new object of DroneInParcel.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>returns 
        /// the new object of DroneInParcel</returns>
        private DroneInParcel ConvertDroneInParcel(int? droneId)
        {
            if (!droneId.HasValue)
                return null;
            if (lDroneToList.FirstOrDefault(d => d.Id == droneId) != null)
            {
                var drone = lDroneToList.FirstOrDefault(drone => drone.Id == droneId);
                return new() { Id = droneId.Value, BatteryStatus = drone.BatteryStatus, CurrLocation = drone.CurrLocation };
            }
            else
            {
                return null;
            }
        }

        public string DeleteParcel(int parcelId)
        {
            try
            {
                dal.Remove(dal.GetFromDalById<DO.Parcel>(parcelId));
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Drone), parcelId);
            }
            return $"The Parcel with Id: {parcelId} was successfully removed from the system";
        }
    }
}


