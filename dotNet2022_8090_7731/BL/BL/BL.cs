using DalObject;
using IBL.BO;
using IDal.DO;
using static IBL.BO.Enum.DroneStatus;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using System.Device.Location;

namespace BL
{
    partial class BL : IBL.IBL
    {
        List<DroneToList> lDroneToList = new List<DroneToList>();
        static public double available;
        static public double lightWeight;
        static public double mediumWeight;
        static public double heavyWeight;

        static public double chargingRate;
        IDal.IDal dal;
        public BL()
        {
            dal = new DalObject.DalObject();
            double[] arrPCRequest = dal.PowerConsumptionRequest();
            available = arrPCRequest[0];
            lightWeight = arrPCRequest[1];
            mediumWeight = arrPCRequest[2];
            heavyWeight = arrPCRequest[3];
            chargingRate = arrPCRequest[4];

            IEnumerable<Drone> droneList = dal.GetDrones();
            IEnumerable<Parcel> parcelList = dal.GetParcels();
            IEnumerable<Customer> customerDalList = dal.GetCustomers();
            IEnumerable<BaseStation> stationDalList = dal.GetStations();

            foreach (var parcel in parcelList)
            {
                if (parcel.DroneId != 0 && !parcel.Arrival.HasValue)
                {
                    DroneToList droneToList = new DroneToList();
                    Drone drone = droneList.FirstOrDefault(drone => drone.Id == parcel.DroneId);
                    copyCommon(droneToList, drone);

                    droneToList.DStatus = Delivery;

                    //location
                    Customer sender = customerDalList.First(customer => customer.Id == parcel.SenderId);
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


                    // calculate the distance between closest charging station to the destination
                    Customer destination = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    var destinationCoord = new GeoCoordinate(destination.Latitude, destination.Longitude);
                    Location closetStation = closestStation(destination, stationDalList);
                    var closetStationCoord = new GeoCoordinate(closetStation.Latitude, closetStation.Longitude);
                    double distance = destinationCoord.GetDistanceTo(closetStationCoord);

                    var droneCoord = new GeoCoordinate(droneToList.CurrLocation.Latitude, droneToList.CurrLocation.Longitude);
                    distance += destinationCoord.GetDistanceTo(droneCoord);



                    droneToList.BatteryStatus = randBattery(drone);
                    droneToList.Add
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
                    }
                }
                if (!IsDeliverying)
                {
                    DroneToList droneToList = new DroneToList();
                    Drone tempDrone = drone;
                    copyCommon(droneToList, tempDrone);
                    droneToList.DStatus = (DroneStatus)DataSource.rand.Next((int)Free, (int)Maintenance);
                    if (droneToList.DStatus == Maintenance)
                    {
                        BaseStation tempStation = stationDalList.ElementAt(DataSource.rand.Next(0, stationDalList.Count()));
                        droneToList.CurrLocation = new Location(tempStation.Longitude, tempStation.Latitude);
                        droneToList.BatteryStatus = DataSource.rand.Next(0, 21);
                    }
                    else /*if (droneToList.DStatus == Free)*/
                    {





                        droneToList.CurrLocation = locaProvidedParcels(parcelList, customerDalList, droneToList);
                        droneToList.BatteryStatus =
                    }
                }
            }
            //BL_Drone bL_Drone;

            //if (dalObject.GetUnbelongParcels()!=null )
            //{
            //    bL_Drone
            //}


        }

        private float randBattery(Drone drone)
        {


            return
        }

        private void copyCommon(DroneToList destination, Drone source)
        {
            destination.Id = source.Id;
            destination.Model = source.Model;
            destination.Weight = source.MaxWeight;
        }

        /// <summary>
        /// rand location of customer which his parcel had provided him
        /// </summary>
        /// <param name="parcelDalList"></param>
        /// <param name="customerDalList"></param>
        /// <param name="droneToList"></param>
        /// <returns></returns>
        private Location locaProvidedParcels(IEnumerable<Parcel> parcelDalList, IEnumerable<Customer> customerDalList, DroneToList droneToList)
        {
            Customer customer;
            List<Location> optionalLocation = new List<Location>();
            foreach (var parcel in parcelDalList)
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    optionalLocation.Add(new Location(customer.Longitude, customer.Latitude));
                }
            }
            return droneToList.CurrLocation = optionalLocation[DataSource.rand.Next(0, optionalLocation.Count)];
        }


        private Location closestStation(Customer customer, IEnumerable<BaseStation> stationDalList)
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
