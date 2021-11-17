using DalObject;
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
        List<DroneToList> lDroneToList = new List<DroneToList>();
        private Random rand;

        static public double pConsumFree;
        static public double pConsumLight;
        static public double pConsumMedium;
        static public double pConsumHeavy;

        static public double chargingRate;
        IDal.IDal dal;
        public BL()
        {
            dal = new DalObject.DalObject();
            rand = new Random(); 
            double[] arrPCRequest = dal.PowerConsumptionRequest();
            pConsumFree = arrPCRequest[0];
            pConsumLight = arrPCRequest[1];
            pConsumMedium = arrPCRequest[2];
            pConsumHeavy = arrPCRequest[3];
            chargingRate = arrPCRequest[4];

            IEnumerable<IDal.DO.Drone> droneList = dal.GetDrones();
            IEnumerable<IDal.DO.Parcel> parcelList = dal.GetParcels();
            IEnumerable<IDal.DO.Customer> customerDalList = dal.GetCustomers();
            IEnumerable<BaseStation> stationDalList = dal.GetStations();

            foreach (var drone in dal.GetDrones())
            {

                var parcel = parcelList.FirstOrDefault(p => p.DroneId == drone.Id && !p.Arrival.HasValue);

            }
            foreach (var parcel in parcelList)
            {
                if (parcel.DroneId != 0 && !parcel.Arrival.HasValue)
                {
                    IDal.DO.Drone drone = droneList.FirstOrDefault(drone => drone.Id == parcel.DroneId);

                    /// מה קורה אם הרחפן כבר באמצע לשללוח זא טופל באיטרציות קודמות
                    DroneToList droneToList = copyCommon(drone);

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
            }
            bool IsDeliverying = false;
            foreach (var drone in droneList)
            {
                foreach (var parcel in parcelList)
                {
                    if (parcel.DroneId == drone.Id)
                    {
                        IsDeliverying = true;
                        break;
                    }
                }
                if (!IsDeliverying)
                {
                    //lDroneToList.Add(droneToList);

                    newUndeliveringDroneToList(stationDalList, drone);
                }
                IsDeliverying = false;
            }
        }

        private void newUndeliveringDroneToList(IEnumerable<BaseStation> stationDalList, IDal.DO.Drone drone)
        {
            DroneToList droneToList = copyCommon(drone);

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

        // weight=0 ערך ברירת מחדל לפונקציה
        private double MinButtery(double distance, IDal.DO.WeightCategories weight = 0)
        {
            switch (weight)
            {
                case IDal.DO.WeightCategories.Light:
                    return pConsumLight * distance;
                case IDal.DO.WeightCategories.Medium:
                    return pConsumMedium * distance;
                case IDal.DO.WeightCategories.Heavy:
                    return pConsumHeavy * distance;
                default:
                    return pConsumFree * distance;
            }
        }
        private DroneToList copyCommon(IDal.DO.Drone source)
        {
            DroneToList nDroneToList = new DroneToList();
            nDroneToList.Id = source.Id;
            nDroneToList.Model = source.Model;
            nDroneToList.Weight = source.MaxWeight;
            return nDroneToList;
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
            return droneToList.CurrLocation = optionalLocation[DataSource.Rand.Next(0, optionalLocation.Count)];
        }


        private Location closestStation(IDal.DO.Customer customer, IEnumerable<BaseStation> stationDalList)
        {
            var cCoord = new GeoCoordinate(customer.Latitude, customer.Longitude);
            var sCoord = new GeoCoordinate(stationDalList.ElementAt(0).Latitude, stationDalList.ElementAt(0).Longitude);
            double currDistance, distance = sCoord.GetDistanceTo(cCoord);
            int index = 0;
            for (int i = 1; i < stationDalList.Count(); i++)
            {
                sCoord = new GeoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
                currDistance = sCoord.GetDistanceTo(cCoord);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    index = i;
                }
            }
            return new Location(stationDalList.ElementAt(index).Latitude, stationDalList.ElementAt(index).Longitude);
        }
    }
}
