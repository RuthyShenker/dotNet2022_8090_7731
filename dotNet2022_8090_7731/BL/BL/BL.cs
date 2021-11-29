﻿using DalObject;
using IBL.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using System.Device.Location;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        IDal.IDal dal;

        List<DroneToList> lDroneToList;

        static double powerConsumptionFree;
        static double powerConsumptionLight;
        static double powerConsumptionMedium;
        static double powerConsumptionHeavy;
        /// <summary>
        /// Charging rate per hour
        /// </summary>
        static double chargingRate;

        public BL()
        {
            dal = new DalObject.DalObject();
            lDroneToList = new List<DroneToList>();
            UpdatePConsumption();
            foreach (var drone in dal.GetListFromDal<IDal.DO.Drone>())
            {
                if (lDroneToList.Count != 0 && lDroneToList.Exists(droneInList => droneInList.Id == drone.Id))
                {
                    lDroneToList.First(droneInList => droneInList.Id == drone.Id).NumOfParcel++;
                }
                lDroneToList.Add(ConvertToList(drone));
            }
        }


        /// <summary>
        /// A function that Pulls out from that data base the data of the fields:
        /// powerConsumptionFree
        //powerConsumptionLight
        // powerConsumptionMedium
        //powerConsumptionHeavy
        //chargingRate
        //this function doesn't return any thing.
        /// </summary>
        private void UpdatePConsumption()
        {
            double[] arrPCRequest = dal.PowerConsumptionRequest();
            powerConsumptionFree = arrPCRequest[0];
            powerConsumptionLight = arrPCRequest[1];
            powerConsumptionMedium = arrPCRequest[2];
            powerConsumptionHeavy = arrPCRequest[3];
            chargingRate = arrPCRequest[4];
        }

        /// <summary>
        /// A function that gets an object of IDal.DO.Drone and Expands it to object of 
        /// IBL.BO.DroneToList Considering of course with logic.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private DroneToList ConvertToList(IDal.DO.Drone drone)
        {
            DroneToList nDrone = copyCommon(drone);
            var parcel = dal.GetListFromDal<IDal.DO.Parcel>().FirstOrDefault(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
            if (!parcel.Equals(default(IDal.DO.Parcel)))
            {
               return CalculateDroneInDelivery(nDrone, parcel);
            }
            else 
            {
                nDrone.NumOfParcel = null; // אולי לא צריך שורה זו
                return CalculateUnDeliveryingDrone(nDrone);
            }
        }



        /// <summary>
        /// A function that builds new DroneToList object and gets an object of IDal.DO.Drone
        /// and copies from the object-IDal.DO.Drone the common fields.
        /// </summary>
        private DroneToList copyCommon(IDal.DO.Drone source)
        {
            DroneToList nDroneToList = new DroneToList(
            source.Id,
           source.Model,
           (IBL.BO.WeightCategories)source.MaxWeight);
            return nDroneToList;
        }

        /// <summary>
        /// ???????????????????????????????????????????????
        /// </summary>
        /// <param name="nDrone"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private DroneToList CalculateDroneInDelivery(DroneToList nDrone, IDal.DO.Parcel parcel)
        {
            nDrone.DStatus = DroneStatus.Delivery;
            //location
            var sender = dal.GetFromDalByCondition<IDal.DO.Customer>(customer => customer.Id == parcel.SenderId);
            if (parcel.BelongParcel != null && parcel.PickingUp == null)
            {
                nDrone.CurrLocation = ClosestStation(new Location(sender.Longitude, sender.Latitude)).SLocation;
            }
            else if (parcel.Arrival == null && parcel.PickingUp != null)
            {
                nDrone.CurrLocation = new Location(sender.Longitude, sender.Latitude);
            }
            // battery Status
            Location destination = GetBLById<IDal.DO.Customer, Customer>(parcel.GetterId).CLocation;
            Location closestStation = ClosestStation(destination).SLocation;
            double distance = CalculateDistance(nDrone.CurrLocation, destination, closestStation);
            double minBattery = MinBattery(distance, (WeightCategories)parcel.Weight);
            nDrone.BatteryStatus = Rand.NextDouble() * (100 - minBattery) + minBattery;
            nDrone.NumOfParcel++;
            return nDrone;
        }

        /// <summary>
        /// ?????????????????????????????????????????????
        /// </summary>
        /// <param name="nDrone"></param>
        /// <returns></returns>
        private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
        {
            nDrone.DStatus = (DroneStatus)Rand.Next((int)DroneStatus.Free, (int)DroneStatus.Maintenance);
            if (nDrone.DStatus == DroneStatus.Maintenance)
            {
                var stationDalList = dal.GetListFromDal<IDal.DO.BaseStation>();
                var station = stationDalList.ElementAt(Rand.Next(stationDalList.Count()));
                nDrone.CurrLocation = new Location(station.Longitude, station.Latitude);
                nDrone.BatteryStatus = Rand.NextDouble() * 20;
            }
            else // droneToList.DStatus == Free
            {
                var customersList = CustomersWithProvidedParcels();
                nDrone.CurrLocation = customersList[Rand.Next(customersList.Count)].CLocation;
                var closetStation = ClosestStation(nDrone.CurrLocation);
                double distance = CalculateDistance( nDrone.CurrLocation, closetStation.SLocation);
                nDrone.BatteryStatus = Rand.NextDouble() * (100 - MinBattery(distance)) + MinBattery(distance);
            }
            return nDrone;
        }


        /// <summary>
        /// A function that gets weight of drone
        /// and distance and returns the minimum battery that 
        /// the drone needs in order to flight.
        /// Default value of weight=0.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>the minimum battery in double</returns>
        private double MinBattery(double distance, IBL.BO.WeightCategories weight = 0)
        {
            switch (weight)
            {
                case IBL.BO.WeightCategories.Light:
                    return powerConsumptionLight * distance;
                case IBL.BO.WeightCategories.Medium:
                    return powerConsumptionMedium * distance;
                case IBL.BO.WeightCategories.Heavy:
                    return powerConsumptionHeavy * distance;
                default:
                    return powerConsumptionFree * distance;
            }
        }

        /// <summary>
        /// A function that Calculates distance between all places in the array
        /// Places to be sent to function in the order of the flight
        /// This is a fishing plane that takes a package that travels from a customer to a destination to a charging station
        /// </summary>
        /// <param name="locations"></param>
        ///<returns>the function gets an array of locations and returns
        /// the sum of the distance between them
        /// in double.</returns>
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
        /// A function that gets an object of Location and bulids from it an object of  
        /// GeoCoordinate and returns it.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>returns an object of GeoCoordinate</returns>
        private GeoCoordinate geoCoordinate(Location location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }

        /// <summary>
        /// A function that gets id of customer and builds from it an object
        /// of CustomerInParcel and Of course considering logic and returns 
        /// the new object of CustomerInParcel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>eturns 
        /// the new object of CustomerInParcel</returns>
        private CustomerInParcel NewCustomerInParcel(int Id)
        {
            return new CustomerInParcel(Id, dal.GetFromDalById<IDal.DO.Customer>(Id).Name);
        }

        /// <summary>
        /// A function that gets id of drone and bulids from it an object of 
        /// DroneInParcel and of course considering logic and returns 
        /// the new object of DroneInParcel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>returns 
        /// the new object of DroneInParcel</returns>
        private DroneInParcel NewDroneInParcel(int Id)
        {
            var drone = lDroneToList.FirstOrDefault(drone => drone.Id == Id);
            return new DroneInParcel(Id, drone.BatteryStatus, drone.CurrLocation);
        }

        /// <summary>
        /// A generic function that gets two types: DL, BL and id and 
        /// pulls out from the dal the object that its type DL and its id is the
        /// same as the id that the function gets and Expands this object to new object
        /// of BL type and returns this object.
        /// </summary>
        /// <typeparam name="DL"></typeparam>
        /// <typeparam name="BL"></typeparam>
        /// <param name="Id"></param>
        /// <returns>returns an object of BL type</returns>
        public BL GetBLById<DL, BL>(int Id) where DL : IDal.DO.IIdentifiable
        {
            dynamic wantedDal = dal.GetFromDalById<DL>(Id);
            BL wantedBl= ConvertToBL(wantedDal);
            return wantedBl;
        }

        /// <summary>
        /// A function that gets two locations and returns if they are the same or not.
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns>returns if they are the same or not</returns>
        private bool equalLocations(Location location1, Location location2)
        {
            return location1.Longitude == location2.Longitude && location1.Latitude == location2.Latitude;
        }


        /// <summary>
        /// A function that gets two types: DL, BL and 
        /// pulls out the list from dal with DL type and 
        /// creates list Of BL type and converts every object in the
        /// list to DL type,to BL type and adds it to BL list and returns this List.
        /// </summary>
        /// <typeparam name="DL"></typeparam>
        /// <typeparam name="BL"></typeparam>
        /// <returns>returns list of data with type of BL. </returns>
        public IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDal.DO.IIdentifiable
        {
            var bLList = new List<BL>();
            var dalList = dal.GetListFromDal<DL>();
            foreach (dynamic dlItem in dalList)
            {
                var blItem = ConvertToBL(dlItem);
                bLList.Add(blItem);
            }
            return bLList;
        }

        /// <summary>
        /// A generic function that gets two types:DL, BLToList
        /// and pulls out the list from dal with DL type and 
        /// creates list Of BLToList type and converts every object in the
        /// list to BLToList type and adds it to BLToList list and returns this List.
        /// </summary>
        /// <typeparam name="DL"></typeparam>
        /// <typeparam name="BLToList"></typeparam>
        /// <returns>returns list of data with the BLToList</returns>
        public IEnumerable<BLToList> GetListToList<DL, BLToList>() where DL : IDal.DO.IIdentifiable
        {
            if (typeof(BLToList) == typeof(DroneToList))
            {
                return (IEnumerable<BLToList>)lDroneToList;
            }
            var dalList = dal.GetListFromDal<DL>();
            var listToList = new List<BLToList>();
            foreach (dynamic dalItem in dalList)
            {
                var blToListItem = ConvertToList(dalItem);
                listToList.Add(blToListItem);
            }
            return listToList;
        }
    }
}