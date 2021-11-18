using DalObject
using IBL.BO;
using IDal.DO;
using static IBL.BO.Enum.DroneStatus;
using static IBL.BO.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum.WeightCategories;
using System.Device.Location;

namespace BL
{
    partial class BL : IBL.IBL
    {
        IDal.IDal dal;
        List<DroneToList> lDroneToList;
        private Random rand;
        static double pConsumFree;
        static double pConsumLight;
        static double pConsumMedium;
        static double pConsumHeavy;
        static double chargingRate;
        public BL()
        {
            dal = new DalObject.DalObject();
            lDroneToList = new List<DroneToList>();
            rand = new Random();
            UpdatePConsumption();

            IEnumerable<IDal.DO.Drone> droneList = dal.GetDrones();
            IEnumerable<IDal.DO.Parcel> parcelList = dal.GetParcels();
            IEnumerable<IDal.DO.Customer> customerDalList = dal.GetCustomers();
            IEnumerable<BaseStation> stationDalList = dal.GetBaseStations();
            DroneToList droneToList;
            foreach (var drone in droneList)
            {
                droneToList = copyCommon(drone);
                // מה קורה אם יש יותר מחבילה אחת לרחפן
                var parcel = parcelList.FirstOrDefault(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
                if (parcel==null)
                {
                    droneToList.DStatus = Delivery;

                    //location
                    IDal.DO.Customer sender = customerDalList.First(customer => customer.Id == parcel.SenderId);
                    if (parcel.BelongParcel.HasValue && !parcel.PickingUp.HasValue)
                    {
                        droneToList.CurrLocation = closestStation(sender, stationDalList);
                    }
                    else
                    {
                        droneToList.CurrLocation = new Location(sender.Longitude, sender.Latitude);
                        //IEnumerable bL_Customer = dal.GetSpecificItem(typeof(Customer), parcel.SenderId);
                        //droneToList.CurrLocation = bL_Customer.CLocation;
                    }

                    // battery Status
                    IDal.DO.Customer destination = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    Location closetStation = closestStation(destination, stationDalList);
                    double distance = calDistance(closetStation, droneToList.CurrLocation, destination);
                    droneToList.BatteryStatus = rand.Next(MinButtery(distance, parcel.Weight), 100);

                    droneToList.NumOfParcel = 1;

                    lDroneToList.Add(droneToList);
                }
                else
                {
                    newUndeliveringDroneToList(stationDalList, droneToList);
                }
            }
        }
        private void UpdatePConsumption()
        {
            double[] arrPCRequest = dal.PowerConsumptionRequest();
            pConsumFree = arrPCRequest[0];
            pConsumLight = arrPCRequest[1];
            pConsumMedium = arrPCRequest[2];
            pConsumHeavy = arrPCRequest[3];
            chargingRate = arrPCRequest[4];
        }

        private void newUndeliveringDroneToList(IEnumerable<BaseStation> stationDalList, DroneToList droneToList)
        {
            droneToList.DStatus = (DroneStatus)DataSource.Rand.Next((int)Free, (int)Maintenance);
            if (droneToList.DStatus == Maintenance)
            {
                BaseStation station = stationDalList.ElementAt(DataSource.Rand.Next(0, stationDalList.Count()));
                droneToList.CurrLocation = new Location(station.Longitude, station.Latitude);
                droneToList.BatteryStatus = DataSource.Rand.Next(21);
            }
            else /*if (droneToList.DStatus == Free)*/
            {
                droneToList.CurrLocation = locaProvidedParcels(parcelList, customerDalList, droneToList);
                Location closetStation = closestStation(droneToList.CurrLocation, stationDalList);
                double distance = calDistance(closetStation, droneToList.CurrLocation);
                droneToList.BatteryStatus = rand.Next(MinButtery(distance), 100);
            }
            droneToList.NumOfParcel = 1;
            lDroneToList.Add(droneToList);
        }

        private double calDistance(Location closestStation, Location droneLocation, IDal.DO.Customer destination = default(IDal.DO.Customer))
        {
            var droneLocationCoords = geoCoordinate(droneLocation);
            var closetStationCoord = geoCoordinate(closestStation);
            if (!destination.Equals(default(IDal.DO.Customer)))
            {
                Location ldestination = new Location(destination.Longitude, destination.Latitude);
                var destinationCoord = geoCoordinate(ldestination);
                double distance = droneLocationCoords.GetDistanceTo(destinationCoord);
                return distance += destinationCoord.GetDistanceTo(closetStationCoord);
            }
            return droneLocationCoords.GetDistanceTo(closetStationCoord);
        }

        private GeoCoordinate geoCoordinate(Location location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }

        /// <summary>
        /// rand location of customer which his parcel had provided him
        /// </summary>
        /// <param name="parcelDalList"></param>
        /// <param name="customerDalList"></param>
        /// <param name="droneToList"></param>
        /// <returns></returns>
        private Location locaProvidedParcels(IEnumerable<IDal.DO.Parcel> parcelDalList, IEnumerable<IDal.DO.Customer> customerDalList, DroneToList droneToList)
        {
            IDal.DO.Customer customer;
            List<Location> optionalLocation = new List<Location>();
            foreach (var parcel in parcelDalList)
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    optionalLocation.Add(new Location(customer.Longitude, customer.Latitude));
                }
            }
            return droneToList.CurrLocation = optionalLocation[DataSource.Rand.Next(optionalLocation.Count)];
        }

        public void AddingBaseStation(BL_Station bLStation)
        {
            if (dal.ExistsInBaseStation(bLStation.Id))
            {
                throw new Exception("The id is already exists in the base station list!");
            }

            BaseStation station = new BaseStation() { Id = bLStation.Id, Latitude = bLStation.SLocation.Latitude, Longitude = bLStation.SLocation.Longitude, NameStation = bLStation.NameStation, NumberOfChargingPositions = bLStation.NumAvailablePositions };
            dal.AddingBaseStation(station);
        }

        public void AddingDrone(IBL.BO.Drone bLDrone, int StationId)
        {
            if (dal.ExistsInDroneList(bLDrone.Id))
            {
                throw new Exception("The id is already exists in the Drone list!");
            }
            if (!dal.ExistsInBaseStation(StationId))
            {
                throw new Exception("this base station doesnt exists!");
            }
            if (!dal.ThereAreFreePositions(StationId))
            {
                throw new Exception("There arent free positions for this base station!");
            }
            DroneToList droneToList = new DroneToList()
            {
                Id = bLDrone.Id,
                Model = bLDrone.Model,
                Weight = bLDrone.Weight,
                BatteryStatus = bLDrone.BatteryStatus,
                DStatus = bLDrone.DroneStatus,
                CurrLocation = new Location(dal.GetBaseStation(StationId).Longitude, dal.GetBaseStation(StationId).Latitude),
                NumOfParcel = null
            };

            IDal.DO.Drone drone = new IDal.DO.Drone() { Id = bLDrone.Id, MaxWeight = bLDrone.Weight, Model = bLDrone.Model };
            dal.AddingDrone(drone);
        }

        public void AddingCustomer(IBL.BO.Customer bLCustomer)
        {
            if (dal.ExistsInCustomerList(bLCustomer.Id))
            {
                throw new Exception("The id is already exists in the Customer List!");
            }
            IDal.DO.Customer newCustomer = new IDal.DO.Customer() { Id = bLCustomer.Id, Name = bLCustomer.Name, Phone = bLCustomer.Phone, Latitude = bLCustomer.CLocation.Latitude, Longitude = bLCustomer.CLocation.Longitude };
            dal.AddingCustomer(newCustomer);
        }

        public void GettingParcelForDelivery(IBL.BO.Parcel newParcel)
        {
            IDal.DO.Parcel parcel = new IDal.DO.Parcel()
            {
                SenderId = newParcel.SenderId,
                GetterId = newParcel.GetterId,
                Weight = (IDal.DO.WeightCategories)newParcel.Weight,
                Status = (UrgencyStatuses)newParcel.MPriority,
                DroneId = 0,
                MakingParcel = newParcel.MakingParcel,
                BelongParcel = newParcel.BelongParcel,
                PickingUp = newParcel.PickingUp,
                Arrival = newParcel.Arrival
            };

            dal.GettingParcelForDelivery(parcel);
        }


        public void UpdatingDroneName(int droneId, string newModel)
        {
            if (!dal.ExistsInDroneList(droneId))
            {
                throw new Exception("this id doesnt exist in drone list!");
            }
            IDal.DO.Drone drone = dal.GetDrone(droneId);
            drone.Model = newModel;
            dal.UpdateDrone(droneId, drone);
        }

        public void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions)
        {
            if (!dal.ExistsInBaseStation(stationId))
            {
                throw new Exception("this id doesnt exist in base station list!");
            }
            BaseStation baseStation = dal.GetBaseStation(stationId);
            if (!string.IsNullOrEmpty(stationName))
            {
                baseStation.NameStation = stationName;
            }
            if (amountOfPositions != default)
            {
                baseStation.NumberOfChargingPositions = amountOfPositions;
            }
            dal.UpdateBaseStation(stationId, baseStation);
        }

        public void UpdatingCustomerDetails(string customerId, string newName, string newPhone)
        {
            if (!dal.ExistsInCustomerList(customerId))
            {
                throw new Exception("this id doesnt exist in customer list!");
            }
            IDal.DO.Customer customer = dal.GetCustomer(customerId);
            if (!string.IsNullOrEmpty(newName))
            {
                customer.Name = newName;
            }
            if (!string.IsNullOrEmpty(newPhone))
            {
                customer.Phone = newPhone;
            }
            dal.UpdateCustomer(customerId, customer);
        }
    }
}