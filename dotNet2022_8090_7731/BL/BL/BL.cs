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
using static DalObject.DataSource;
using System.Device.Location;

namespace BL
{
    partial class BL : IBL.IBL
    {
        IDal.IDal dal;
        List<DroneToList> lDroneToList;
        static double pConsumFree;
        static double pConsumLight;
        static double pConsumMedium;
        static double pConsumHeavy;
        /// <summary>
        /// לשעה קצב טעינה
        /// </summary>
        static double chargingRate;
        public BL()
        {
            dal = new DalObject.DalObject();
            lDroneToList = new List<DroneToList>();
            UpdatePConsumption();

            IEnumerable<IDal.DO.Drone> droneList = dal.GetDrones();
            IEnumerable<IDal.DO.Parcel> parcelList = dal.GetParcels();
            IEnumerable<IDal.DO.Customer> customerDalList = dal.GetCustomers();
            IEnumerable<BaseStation> stationDalList = dal.GetBaseStations();
            DroneToList droneToList;

            foreach (var drone in dal.GetDrones())
            {
                droneToList = copyCommon(drone);
                // מה קורה אם יש יותר מחבילה אחת לרחפן
                IDal.DO.Parcel parcel = parcelList.FirstOrDefault(p => p.DroneId == drone.Id && !p.Arrival.HasValue);
                if (!parcel.Equals(default(IDal.DO.Parcel)))
                {
                    droneToList.DStatus = Delivery;

                    //location
                    IDal.DO.Customer sender = customerDalList.First(customer => customer.Id == parcel.SenderId);
                    if (parcel.BelongParcel.HasValue && !parcel.PickingUp.HasValue)
                    {
                        IDal.DO.BaseStation baseStation = closestStation(new Location(sender.Longitude, sender.Latitude));
                        droneToList.CurrLocation = new Location(baseStation.Longitude, baseStation.Latitude);
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
                    double distance = CalculateDistance(closetStation, droneToList.CurrLocation, destination);
                    droneToList.BatteryStatus = Rand.Next(MinButtery(distance, parcel.Weight), 100);

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
                var optionalLocations = locaProvidedParcels();
                droneToList.CurrLocation = optionalLocations[DataSource.Rand.Next(optionalLocations.Count())];
               IDal.DO.Location closetStation = closestStation(droneToList.CurrLocation);
                double distance = CalculateDistance(closetStation, droneToList.CurrLocation);
                droneToList.BatteryStatus = rand.Next(MinBattery(distance), 100);
            }
            droneToList.NumOfParcel = 1;
            lDroneToList.Add(droneToList);
        }

        /// <summary>
        /// the function gets an array of locations and return the sum of the distance between them
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        private double CalculateDistance(params Location[] locations)
        {
            var locationCoords1 = geoCoordinate(locations[0]);
            var locationCoords2 = locationCoords1;
            double distance = 0;
            for (int i = 1; i < locations.Length; i++)
            {
                locationCoords2 = geoCoordinate(locations[i]);
                distance += locationCoords1.GetDistanceTo(locationCoords2);
                locationCoords1 = locationCoords2;
            }
            return distance;
        }

        /// <summary>
        /// get an object of location and return an object of GeoCoordinate
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private GeoCoordinate geoCoordinate(Location location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }

        /// <summary>
        /// returns the locations of people which their parcels had provided them
        /// </summary>
        /// <param name="parcelDalList"></param>
        /// <param name="customerDalList"></param>
        /// <param name="droneToList"></param>
        /// <returns></returns>
        private IList<Location> locaProvidedParcels()
        {
            IDal.DO.Customer customer;
            var locationsList = new List<Location>();
            var customerDalList = dal.GetListFromDal<IDal.DO.Customer>();
            foreach (var parcel in dal.GetListFromDal<IDal.DO.Parcel>())
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    locationsList.Add(new Location(customer.Longitude, customer.Latitude));
                }
            }
            return locationsList;
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
            //if (!dal.ExistsInDroneList(droneId))
            //{
            //    
            //}
            try
            {
                IDal.DO.Drone drone = dal.GetDrone(droneId);
                drone.Model = newModel;
                dal.UpdateDrone(droneId, drone);
            }
            catch (DAL.IdNotExistInAListException)
            {
                //bl exception-new
                throw new Exception("this id doesnt exist in drone list!");
            }
        }

        /// <summary>
        /// A function that initalizes: pConsumFree
        ///pConsumLight
        ///pConsumMedium
        ///pConsumHeavy
        ///chargingRate
        /// </summary>
        /// 
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

        public IEnumerable<BL> GetBList<BL, DL>(Converter<DL, BL> map)
        {
            var bLList = new List<BL>();
            IEnumerable<DL> dalList = dal.GetListFromDal<DL>();
            foreach (DL dlItem in dalList)
            {
                var blItem = map(dlItem);
                bLList.Add(blItem);
            }
            return bLList;
        }
    }
}