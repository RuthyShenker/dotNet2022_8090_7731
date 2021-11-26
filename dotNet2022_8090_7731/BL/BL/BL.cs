using DalObject;
using IBL.BO;
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
            foreach (var drone in dal.GetListFromDal<IDal.DO.Drone>())
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
            var parcel = dal.GetListFromDal<IDal.DO.Parcel>().FirstOrDefault(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
            if (!parcel.Equals(default(IDal.DO.Parcel)))
            {
                if (lDroneToList.Count != 0 && dal.IsExistInList(lDroneToList, drone => drone.Id == parcel.DroneId))
                {
                   lDroneToList.First(drone => drone.Id == parcel.DroneId).NumOfParcel++;
                    return ; 
                }
                else
                {
                    return CalculateDroneInDelivery(nDrone, parcel);
                }
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
            nDroneToList.Weight = (IBL.BO.WeightCategories)source.MaxWeight;
            return nDroneToList;
        }

        private DroneToList CalculateDroneInDelivery(DroneToList nDrone, IDal.DO.Parcel parcel)
        {
            nDrone.DStatus = DroneStatus.Delivery;
            //location
            var sender = dal.GetFromDalByCondition<IDal.DO.Customer>(customer => customer.Id == parcel.SenderId);
            if (parcel.BelongParcel.HasValue && !parcel.PickingUp.HasValue)
            {
                nDrone.CurrLocation = ClosestStation(new Location(sender.Longitude, sender.Latitude)).SLocation;
            }
            else
            {
                nDrone.CurrLocation = new Location(sender.Longitude, sender.Latitude);
                //IEnumerable bL_Customer = dal.GetSpecificItem(typeof(Customer), parcel.SenderId);
                //droneToList.CurrLocation = bL_Customer.CLocation;
            }
            // battery Status
            var getter = dal.GetFromDalByCondition<IDal.DO.Customer>(customer => customer.Id == parcel.GetterId);
            Location destination = new Location(getter.Longitude, getter.Latitude);
            Location closestStation = ClosestStation(destination).SLocation;
            double distance = CalculateDistance(nDrone.CurrLocation, destination, closestStation);
            nDrone.BatteryStatus = Rand.Next(MinBattery(distance,(WeightCategories)parcel.Weight), 100);
            nDrone.NumOfParcel++;
            return nDrone;
        }

        private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
        {
            nDrone.DStatus = (DroneStatus)DataSource.Rand.Next((int)DroneStatus.Free, (int)DroneStatus.Maintenance);
            if (nDrone.DStatus == DroneStatus.Maintenance)
            {
                var stationDalList = dal.GetListFromDal<IDal.DO.BaseStation>();
                var station = stationDalList.ElementAt(DataSource.Rand.Next(0, stationDalList.Count()));
                nDrone.CurrLocation = new Location(station.Longitude, station.Latitude);
                nDrone.BatteryStatus = DataSource.Rand.Next(21);
            }
            else /*if (droneToList.DStatus == Free)*/
            {
                var customersList = CustomersWithProvidedParcels();
                nDrone.CurrLocation = customersList[DataSource.Rand.Next(customersList.Count())].CLocation;
                var closetStation = ClosestStation(nDrone.CurrLocation);
                double distance = CalculateDistance(closetStation.SLocation, nDrone.CurrLocation);
                nDrone.BatteryStatus = Rand.NextDouble() * (100 - MinBattery(distance)) + MinBattery(distance);
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
        private double MinBattery(double distance, IBL.BO.WeightCategories weight = 0)
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
            return new CustomerInParcel(Id, dal.GetFromDalById<IDal.DO.Customer>(Id).Name);
        }

        private DroneInParcel NewDroneInParcel(int Id)
        {
            var drone = lDroneToList.FirstOrDefault(drone => drone.Id == Id);
            return new DroneInParcel(Id, drone.BatteryStatus, drone.CurrLocation);
        }

        public BL GetBLById<DL, BL>(int Id) where DL : IDal.DO.IIdentifiable
        {
            dynamic wanted = dal.GetFromDalById<DL>(Id);
            return convertToBL(wanted);
        }
        
        // מחזיר רשימה BL
        //  BLל DAL משתמש בפונקציה שממירה 
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
        // להחליף את שם הפונקציה למשהו ברור פליז
        // פונקציה גנרית
        // מקבל סוג אוביקט DL
        // מחזיר רשימה מסוג מתאים BLToList
        // ממיר לכל אוביקט עי פונקציה שממירה - ConvertToList
        public IEnumerable<BLToList> GetListToList<DL, BLToList>() where DL : IDal.DO.IIdentifiable
        {
            if (typeof(BLToList) == typeof(Drone))
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