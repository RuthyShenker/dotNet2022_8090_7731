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
                droneToList = copyCommon(drone);
                // מה קורה אם יש יותר מחבילה אחת לרחפן
                IDal.DO.Parcel parcel = parcelList.FirstOrDefault(parcel => parcel.DroneId == drone.Id && !parcel.Arrival.HasValue);
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
                    droneToList.BatteryStatus = Rand.Next(MinBattery(distance, parcel.Weight), 100);

                    droneToList.NumOfParcel = 1;

                    lDroneToList.Add(droneToList);
                }
                else
                {
                    newUndeliveringDroneToList(droneToList);
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

        // לשנות שם לפונקציה אם לא ברור
        // מקבלת רחפן שלא בסטטוס שלייחה ומעדכנת את שאר השדות לפי ההוראות
        private void newUndeliveringDroneToList(DroneToList droneToList)
        {
            droneToList.DStatus = (DroneStatus)DataSource.Rand.Next((int)Free, (int)Maintenance);
            if (droneToList.DStatus == Maintenance)
            {
                var stationDalList = dal.GetListFromDal<IDal.Do.BaseStation>();
                BaseStation station = stationDalList.ElementAt(DataSource.Rand.Next(0, stationDalList.Count()));
                droneToList.CurrLocation = new Location(station.Longitude, station.Latitude);
                droneToList.BatteryStatus = DataSource.Rand.Next(21);
            }
            else /*if (droneToList.DStatus == Free)*/
            {
                var customersList = CustomersWithProvidedParcels();
                droneToList.CurrLocation = customersList[DataSource.Rand.Next(customersList.Count())].CLocation;
                IDal.DO.Location closetStation = closestStation(droneToList.CurrLocation);
                double distance = CalculateDistance(closetStation, droneToList.CurrLocation);
                droneToList.BatteryStatus = rand.Next(MinBattery(distance), 100);
            }
            droneToList.NumOfParcel = 1;
            lDroneToList.Add(droneToList);
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

        // מחזירה את רשימת הלקוחות שיש חבילות שסופקו להם
        private IList<Customer> CustomersWithProvidedParcels()
        {
            IDal.DO.Customer customer;
            var wantedCustomersList = new List<Customer>():
            var customersDalList = dal.GetListFromDal<IDal.DO.Customer>();
            var parcelsDalList = dal.GetListFromDal<IDal.DO.Parcel>();
            foreach (var parcel in parcelsDalList)
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    wantedCustomersList.Add(customer);
                }
            }
            return wantedCustomersList;
        }

       
        BLL GetItemFromBLById<DL>(int Id)
        {
            var wanted = dal.GetFromDalById<DL>(Id);
            return convertToBL(wanted);
        }

       

        /// <summary>
        /// A function that initalizes: pConsumFree
        ///pConsumLight
        ///pConsumMedium
        ///pConsumHeavy
        ///chargingRate
        /// </summary>
        /// 
        

     /*    /// <summary>
        /// rand location of customer which his parcel had provided him
        /// </summary>
        /// <param name="parcelDalList"></param>
        /// <param name="customerDalList"></param>
        /// <param name="droneToList"></param>
        /// <returns></returns>
       // private Location locaProvidedParcels(IEnumerable<IDal.DO.Parcel> parcelDalList, IEnumerable<IDal.DO.Customer> customerDalList, DroneToList droneToList)
        //{
          //  IDal.DO.Customer customer;
            //List<Location> optionalLocation = new List<Location>();
           foreach (var parcel in parcelDalList)
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customerDalList.First(customer => customer.Id == parcel.GetterId);
                    optionalLocation.Add(new Location(customer.Longitude, customer.Latitude));
                }
            }
            return droneToList.CurrLocation = optionalLocation[DataSource.Rand.Next(optionalLocation.Count)];
        }*/

         private CustomerInParcel NewCustomerInParcel(int Id)
        {
            return new CustomerInParcel(Id, GetFromDalById<Customer>(Id).Name );
        }

        private DroneInParcel NewDroneInParcel(int Id)
        {
            var drone = lDroneToList.FirstOrDefault(drone=>drone.Id==Id);
            return new DroneInParcel(Id, drone.BatteryStatus, drone.CurrLocation);
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