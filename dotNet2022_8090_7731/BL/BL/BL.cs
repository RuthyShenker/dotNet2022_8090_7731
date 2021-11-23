using DalObject;
using IBL.BO;
using IDal.DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using System.Device.Location;

namespace BL
{
    partial class BL : IBL.IBL
    {
        IDal.IDal dal;
        List<DroneToList> lDroneToList;

        // לשנות שם לכל השדות האלו המשמעות שלהם:
        // עצמת הבטריה שרחפן צריך כאשר הוא פנוי
        // עצמת הבטריה שרחפן צריך כאשר הוא נושא משקל קל וכו
        // powerConsumptionFree, powerConsumptionLight, powerConsumptionMedium......
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
            DroneToList droneToList;
            foreach (var drone in dal.GetDrones())
            {
                lDroneToList.Add(ConvertToList(drone));
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

        private DroneToList ConvertToList(IDal.DO.Drone drone)
        {
            var nDrone = copyCommon(drone);
            var parcel = parcelList.FirstOrDefault(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
            if (!parcel.Equals(default(IDal.DO.Parcel)))
            {
                // first שורות מעל אותו דבר עדיף עם  
                parcel = parcelList.Where(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
                nDrone.NumOfParcel = parcel.Length;
                return CalculateDroneInDelivery(nDrone);
            }
            else // בעצם זהו הcatch של where
            {
                nDrone.NumOfParcel = null; // אולי לא צריך שורה זו
                return CalculateUnDeliveryingDrone(nDrone);
            }
        }

        /// <summary>
        ///  build new DroneToList object and copy from IDal.DO.Drone the common fields.
        /// </summary>
        private DroneToList copyCommon(IDal.DO.Drone source)
        {
            DroneToList nDroneToList = new DroneToList();
            nDroneToList.Id = source.Id;
            nDroneToList.Model = source.Model;
            nDroneToList.Weight = source.MaxWeight;
            return nDroneToList;
        }

        private DroneToList CalculateDroneInDelivery(DroneToList nDrone)
        {
             nDrone.DStatus = Delivery;
            //location
            var sender = customerDalList.First(customer => customer.Id == parcel.SenderId);
            if (parcel.BelongParcel.HasValue && !parcel.PickingUp.HasValue)
            {
                var baseStation = closestStation(new Location(sender.Longitude, sender.Latitude));
                nDrone.CurrLocation = new Location(baseStation.Longitude, baseStation.Latitude);
            }
            else
            {
                nDrone.CurrLocation = new Location(sender.Longitude, sender.Latitude);
                //IEnumerable bL_Customer = dal.GetSpecificItem(typeof(Customer), parcel.SenderId);
                //droneToList.CurrLocation = bL_Customer.CLocation;
            }
            // battery Status
            var destination = customerDalList.First(customer => customer.Id == parcel.GetterId);
            Location closetStation = closestStation(destination, stationDalList);
            double distance = CalculateDistance(closetStation, droneToList.CurrLocation, destination);
            nDrone.BatteryStatus = Rand.Next(MinBattery(distance, parcel.Weight), 100);
            return nDrone;
        }

        private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
        {
            nDrone.DStatus = (DroneStatus)DataSource.Rand.Next((int)Free, (int)Maintenance);
            if (nDrone.DStatus == Maintance)
            {
                var stationDalList = dal.GetListFromDal<IDal.Do.BaseStation>();
                BaseStation station = stationDalList.ElementAt(DataSource.Rand.Next(0, stationDalList.Count()));
                nDrone.CurrLocation = new Location(station.Longitude, station.Latitude);
                nDrone.BatteryStatus = DataSource.Rand.Next(21);
            }
            else /*if (droneToList.DStatus == Free)*/
            {
                var customersList = CustomersWithProvidedParcels();
                droneToList.CurrLocation = customersList[DataSource.Rand.Next(customersList.Count())].CLocation;
                var closetStation = closestStation(nDrone.CurrLocation);
                double distance = CalculateDistance(closetStation, nDrone.CurrLocation);
                nDrone.BatteryStatus = rand.Next(MinBattery(distance), 100);
            }
            return nDrone;
        }
        /// <summary>
        /// A function that gets weight of drone
        /// and distance and returns the minimum battery that 
        /// the drone needs in order to flight.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>the minimum battery in double</returns>
        // weight=0 ערך ברירת מחדל לפונקציה
        private double MinBattery(double distance,  IBL.BO.WeightCategories weight = 0)
        {
            switch (weight)
            {
                case IBL.BO.WeightCategories.Light:
                    return pConsumLight * distance;
                case IBL.BO.WeightCategories.Medium:
                    return pConsumMedium * distance;
                case IBL.BO.WeightCategories.Heavy:
                    return pConsumHeavy * distance;
                default:
                    return pConsumFree * distance;
            }
        }
        // מחשבת מרחק בין כל המקומים במערך 
        // צריכים לשלוח לפוקציה מקומים לפי סדר הטיסה
        // זא לדוג מטוס שלוקח חבילה נוסע מלקוח ליעד לעמדת טעינה
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

        // מקבלת אוביקט מסוג Location
        // מחזירה אוביקט מסוג geoCoordinate
        /// <summary>
        /// get an object of location and return an object of GeoCoordinate
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private GeoCoordinate geoCoordinate(Location location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }

        private CustomerInParcel NewCustomerInParcel(int Id)
        {
            return new CustomerInParcel(Id, GetFromDalById<Customer>(Id).Name );
        }

        private DroneInParcel NewDroneInParcel(int Id)
        {
            var drone = lDroneToList.FirstOrDefault(drone=>drone.Id==Id);
            return new DroneInParcel(Id, drone.BatteryStatus, drone.CurrLocation);
        }

        BLL GetFromBLById<BL>(int Id)
        {
            var wanted = dal.GetFromDalById<DL>(Id);
            return convertToBL(wanted);
        }

        // מחזיר רשימה BL
        //  BLל DAL משתמש בפונקציה שממירה 
        public IEnumerable<BLL> GetListFromBL<DL>()
        {
            var bLList = new List<BLL>();
            var dalList = dal.GetListFromDal<Exstensions.matchBLObject[typeof(DL)]>();
            foreach (DL dlItem in dalList)
            {
                var blItem = ConvertToBL(dlItem);
                bLList.Add(blItem);
            }
            return bLList;
        }

        // להחליף את שם הפונקציה למשהו ברור פליז
        // פונקציה גנרית
        // מקבל סוג אוביקט DL
        // מחזיר רשימה מסוג מתאים BLToList
        // משתמש במילון 
        // ממיר לכל אוביקט עי פונקציה שממירה - ConvertToList
        public IEnumerable<BLToList> GetListToList<DL>()
        {
            if (typeof( BLToList)==typeof( Drone))
	        {
                 return lDroneToList;
	        }
            var dalList = GetListFromDal<Exstensions.matchBLToListObject[typeof(DL)]>();
            var listToList = new List<Exstensions.matchBLToListObject[typeof(DL)]>();
            foreach (DL dalItem in dalList)
            {
                var blToListItem = ConvertToList(dalItem);
                listToList.Add(blToListItem);
            }
            return listToList;
        }
    }
}