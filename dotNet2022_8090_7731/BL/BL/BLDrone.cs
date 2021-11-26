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

        /*  // יש גנרי
          IEnumerable<DroneToList> GetDrones()
          {
              return lDroneToList;
          }*/

        private Drone ConvertToBL(IDal.DO.Drone drone)
        {
            ParcelInTransfer parcelInTransfer = CalculateParcelInTransfer(drone.Id);
            try
            {
               var wantedDrone = lDroneToList.FirstOrDefault(droneToList => droneToList.Id == drone.Id);
                return new Drone(wantedDrone.Id, wantedDrone.Model, wantedDrone.Weight, wantedDrone.BatteryStatus, wantedDrone.DStatus,
                                     parcelInTransfer, wantedDrone.CurrLocation);
            }
            catch (ArgumentNullException)
            {

                throw;
            }
        }

        private ParcelInTransfer CalculateParcelInTransfer(int droneId)
        {
            var parcelsDalList = dal.GetListFromDal<IDal.DO.Parcel>();
            var dalParcel = parcelsDalList.First(parcel => parcel.DroneId == droneId);
            var parcel = convertToBL(dalParcel);
            var sender = dal.GetFromDalById<IDal.DO.Customer>(parcel.Sender.Id);
            Location senderLocation = new Location(sender.Longitude, sender.Latitude);
            var getter = dal.GetFromDalById<IDal.DO.Customer>(parcel.Getter.Id);
            Location getterLocation = new Location(getter.Longitude, getter.Latitude);
            double distance = CalculateDistance(senderLocation, getterLocation);
            return new ParcelInTransfer(parcel.Id, parcel.PickingUp.HasValue, //  האם זו הכוונה IsInWay?
            parcel.MPriority, parcel.Weight,
            parcel.Sender, parcel.Getter, senderLocation, getterLocation, distance);
        }
        /// <summary>
        /// A function that gets an id od drone and sending it to charging.
        /// </summary>
        /// <param name="IdDrone"></param>
        public void SendingDroneToCharge(int IdDrone)
        {

            try { 
                DroneToList drone = lDroneToList.Find(drone => drone.Id == IdDrone);
                if (drone.DStatus != IBL.BO.DroneStatus.Free)
                {
                    if (drone.DStatus == IBL.BO.DroneStatus.Maintenance)
                        throw new SendingDroneToCharge("this drone in maintenance,it cant go to charge");
                    throw new SendingDroneToCharge("this drone in delivery ,it cant go to charge");
                }
                Station closetdStation = ClosestStation(drone.CurrLocation);
                //Station closebdStation = ConvertToBL(closetdStation);
                if (closetdStation.NumAvailablePositions == 0)
                {
                    throw new StationDoesntHaveAvailablePositionsException("The closet Station doesnt have available positions!");
                }
                double distanceFromDroneToStation = CalculateDistance(closetdStation.SLocation, drone.CurrLocation);
                double minBattery = MinBattery(distanceFromDroneToStation, drone.Weight);
                if (drone.BatteryStatus - minBattery < 0)
                {
                    throw new ThereIsntEnoughBatteryToTheDrone("There isnt enough battery to the drone in order to go to the closet station to be charging");
                }

            drone.BatteryStatus = minBattery;
            drone.CurrLocation = closetdStation.SLocation;
            drone.DStatus = IBL.BO.DroneStatus.Maintenance;
            //--closetBaseStation.NumAvailablePositions;
            //closetBaseStation.LBL_ChargingDrone.Add(new BL_ChargingDrone(drone.Id, closetBaseStation.Id));
            dal.AddDroneToCharge(drone.Id, closetdStation.Id);
        }
            catch (ArgumentNullException exception)
            {
                throw new IdIsNotValidException("This Id Not Exists in list of Drone To List");
    }
}

/// <summary>
/// A function that gets an id of drone and releasing it from charging.
/// </summary>
/// <param name="dId"></param>
/// <param name="timeInCharging"></param>
public void ReleasingDrone(int dId, double timeInCharging)
{
    if (!dal.ExistsInDroneList(dId))
    {
        throw new Exception("this id doesnt exist in the drone list!");
    }
    DroneToList drone = lDroneToList.Find(drone => drone.Id == dId);

    switch (drone.DStatus)
    {
        case DroneStatus.Free:
            throw new Exception("this drone in free state, it cant relese from charging!");
        case DroneStatus.Delivery:
            throw new Exception("this drone in delivery state,it cant relese from charging!");
        default:

            drone.DStatus = DroneStatus.Free;
            drone.BatteryStatus += timeInCharging * chargingRate;
            dal.ReleasingDrone(drone.Id);
            break;
    }
}

        /// <summary>
        /// A function that gets an id of drone and belonging to it a parcel.
        /// </summary>
        /// <param name="dId"></param>
        public void BelongingParcel(int dId)
        {
            if (!dal.IsIdExistInList<IDal.DO.Drone>(dId))
            {
                //throw
            }
            DroneToList droneToList = lDroneToList.Find(drone => drone.Id == dId);
            if (droneToList.DStatus != DroneStatus.Free)
            {
                string dStatus = droneToList.DStatus.ToString();
                throw new Exception($"this drone cant be belonging to parcel because it is in {dStatus} status!");
            }
            // כשעושים מיון לפי thenby הסדר ממיון לפי המיון הסופי או שהוא מבין למין כל קבוצה שוב?
            var optionParcels = dal.GetDalListByCondition<IDal.DO.Parcel>(parcel => parcel.Weight <= (IDal.DO.WeightCategories)droneToList.Weight)
                .OrderByDescending(parcel => parcel.MPriority).ThenByDescending(parcel => parcel.Weight).ThenBy(parcel => GetDistance(droneToList.CurrLocation, parcel));
            bool belonged = false;
            foreach (var parcel in optionParcels)
            {
                if (droneToList.BatteryStatus >= MinBattery(GetDistance(droneToList.CurrLocation, parcel), (IBL.BO.WeightCategories)parcel.Weight))
                {
                    droneToList.DStatus = DroneStatus.Delivery;
                    dal.BelongingParcel(parcel, droneToList.Id);
                    belonged = true;
                    break;
                }
            }
            if (!belonged)
            {
                throw new Exception(); 
            }
}

private double GetDistance(Location droneLocation, IDal.DO.Parcel parcel)
{
    Location senderLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.SenderId).CLocation;
    Location getterLocation = GetBLById<IDal.DO.Customer, Customer>(parcel.SenderId).CLocation;
    return CalculateDistance(droneLocation, senderLocation, getterLocation, ClosestStation(getterLocation).SLocation);
}

        public void AddingDrone(Drone bLDrone, int StationId)
        {
            if (dal.ExistsInDroneList(bLDrone.Id))
            {
                throw new IdIsNotValidException("The id is already exists in the Drone list!");
            }
            if (!dal.ExistsInBaseStation(StationId))
            {
                throw new IdIsNotValidException("This base station doesnt exists!");
            }
            if (!dal.ThereAreFreePositions(StationId))
            {
                throw new TheStationDoesNotHaveFreePositions("There arent free positions for this base station!");
            }
            bLDrone.BatteryStatus = DalObject.DataSource.Rand.Next(20, 41);
            bLDrone.DroneStatus = IBL.BO.DroneStatus.Maintenance;
            IDal.DO.BaseStation station = dal.GetFromDalById<IDal.DO.BaseStation>(StationId);
            DroneToList droneToList = new DroneToList()
            {
                Id = bLDrone.Id,
                Model = bLDrone.Model,
                Weight = bLDrone.Weight,
                BatteryStatus = bLDrone.BatteryStatus,
                DStatus = bLDrone.DroneStatus,
                CurrLocation = new Location(station.Longitude, station.Latitude),
                NumOfParcel = null
            };
            lDroneToList.Add(droneToList);
            IDal.DO.Drone drone = new IDal.DO.Drone()
            {
                Id = bLDrone.Id,
                MaxWeight = (IDal.DO.WeightCategories)bLDrone.Weight,
                Model = bLDrone.Model
            };
            dal.AddingDrone(drone);
        }

        public void UpdatingDroneName(int droneId, string newModel)
        {
            try
            {
                DroneToList droneToList = lDroneToList.Find(drone => drone.Id == droneId);
                droneToList.Model = newModel;
                IDal.DO.Drone dalDrone = new IDal.DO.Drone()
                {
                    Id = droneToList.Id,
                    Model = droneToList.Model,
                    MaxWeight = (IDal.DO.WeightCategories)droneToList.Weight
                };
               dal.UpdateDrone(droneId, dalDrone);
            }
            catch(ArgumentNullException )
            {
                throw new UpdatingFailedIdNotExistsException("this id doesnt exist in list of drone to list!");
            }
            catch (DAL.IdNotExistInTheListException )
            {
                //bl exception-new
                throw new UpdatingFailedIdNotExistsException("this id doesnt exist in drone list!");
            }
        }
    }
}
