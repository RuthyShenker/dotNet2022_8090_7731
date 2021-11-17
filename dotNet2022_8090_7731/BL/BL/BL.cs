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
    public partial class BL : IBL.IBL
    {
        List<DroneToList> lDroneToList = new List<DroneToList>();
        private Random rand;

        static public double pConsumFree;
        static public double pConsumLight;
        static public double pConsumMedium;
        static public double pConsumHeavy;

        static public double chargingRate;
        IDal.IDal dal;
        List<DroneToList> droneToList = new List<DroneToList>();
        static public double available;
        static public double lightWeight;
        static public double mediumWeight;
        static public double heavyWeight;

        static public double chargingRate;
        IDal.IDal dal;
        public BL()
        {
            dal = new DalObject.DalObjectBaseStation();
            double[] arrPCRequest = dal.PowerConsumptionRequest();
            available = arrPCRequest[0];
            lightWeight = arrPCRequest[1];
            mediumWeight = arrPCRequest[2];
            heavyWeight = arrPCRequest[3];
            chargingRate = arrPCRequest[4];

            IEnumerable<Drone> droneList = dal.GetDrones();
            IEnumerable<Parcel> parcelList = dal.GetParcels();
            foreach (var parcel in parcelList)
            {
                if (parcel.DroneId != 0 && !parcel.Arrival.HasValue)
                {
                    Drone drone = droneList.FirstOrDefault(drone => drone.Id == parcel.DroneId);
                    droneToList.Add;
                }
            }
            //BL_Drone bL_Drone;

            //if (dalObject.GetUnbelongParcels()!=null )
            //{
            //    bL_Drone
            //}


        }

        public void AddingBaseStation(BL_Station bLStation)
        {
            if(dal.ExistsInBaseStation(bLStation.Id))
            {
                throw new Exception("The id is already exists in the base station list!");
            }

            BaseStation station = new BaseStation() { Id=bLStation.Id,Latitude=bLStation.SLocation.Latitude,Longitude=bLStation.SLocation.Longitude,NameStation=bLStation.NameStation,NumberOfChargingPositions=bLStation.NumAvailablePositions};
            dal.AddingBaseStation(station);
        }

        public void AddingDrone(BL_Drone bLDrone,int StationId)
        {
            if(dal.ExistsInDroneList(bLDrone.Id))
            {
                throw new Exception("The id is already exists in the Drone list!");
            }
            if(!dal.ExistsInBaseStation(StationId))
            {
                throw new Exception("this base station doesnt exists!");
            }
            if (!dal.ThereAreFreePositions(StationId))
            {
                throw new Exception("There arent free positions for this base station!");
            }
            DroneToList droneToList = new DroneToList() { Id = bLDrone.Id,
                Model = bLDrone.Model,
                Weight = bLDrone.Weight,
                BatteryStatus = bLDrone.BatteryStatus,
                DStatus = bLDrone.DroneStatus,
                CurrLocation = new Location(dal.GetBaseStation(StationId).Longitude, dal.GetBaseStation(StationId).Latitude),
                NumOfParcel = null };

            Drone drone = new Drone() { Id = bLDrone.Id, MaxWeight = bLDrone.Weight, Model = bLDrone.Model };
            dal.AddingDrone(drone);
        }

        public void AddingCustomer(BL_Customer bLCustomer)
        {
           if(dal.ExistsInCustomerList(bLCustomer.Id))
           {
                throw new Exception("The id is already exists in the Customer List!");
           }
            Customer newCustomer = new Customer() { Id = bLCustomer.Id, Name = bLCustomer.Name, Phone = bLCustomer.Phone, Latitude = bLCustomer.CLocation.Latitude, Longitude = bLCustomer.CLocation.Longitude };
            dal.AddingCustomer(newCustomer);
        }

        public void GettingParcelForDelivery(BL_Parcel newParcel)
        {
            Parcel parcel = new Parcel() { SenderId = newParcel.SenderId,
                GetterId = newParcel.GetterId,
                Weight = (IDal.DO.WeightCategories)newParcel.Weight,
                Status = (UrgencyStatuses)newParcel.MPriority,
                DroneId = 0,
                MakingParcel =newParcel.MakingParcel,
                BelongParcel=newParcel.BelongParcel,
                PickingUp=newParcel.PickingUp,
                Arrival=newParcel.Arrival};

            dal.GettingParcelForDelivery(parcel);
        }

        public void BelongingParcel(int pId)
        {
            throw new NotImplementedException();
        }

        public void PickingUpParcel(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeliveryPackage(int Id)
        {
            throw new NotImplementedException();
        }

        public void ChangeDroneStatus(int Id, DroneStatus newStatus)
        {
            throw new NotImplementedException();
        }

        public void ChargingDrone(int IdDrone)
        {
          
        }

        public void ReleasingDrone(int dId, double timeInCharging)
        {
            throw new NotImplementedException();
        }

        public BL_Station BaseStationDisplay(int id)
        {
            throw new NotImplementedException();
        }

        public BL_Drone DroneDisplay(int Id)
        {
            throw new NotImplementedException();
        }

        public BL_Customer CustomerDisplay(string Id)
        {
            throw new NotImplementedException();
        }

        public BL_Parcel ParcelDisplay(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Station> GetBaseStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Drone> GetDrones()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Parcel> GetParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Parcel> GetUnbelongParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BL_Station> AvailableSlots()
        {
            throw new NotImplementedException();
        }

        public void UpdatingDroneName(int droneId, string newModel)
        {
            if(!dal.ExistsInDroneList(droneId))
            {
                throw new Exception("this id doesnt exist in drone list!");
            }
            Drone drone = dal.GetDrone(droneId);
            drone.Model = newModel;
            dal.UpdateDrone(droneId,drone);
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
            if (amountOfPositions!=default)
            {
                baseStation.NumberOfChargingPositions = amountOfPositions;
            }
            dal.UpdateBaseStation(stationId,baseStation);   
        }

        public void UpdatingCustomerDetails(string customerId, string newName, string newPhone)
        {
            if (!dal.ExistsInCustomerList(customerId))
            {
                throw new Exception("this id doesnt exist in customer list!");
            }
            Customer customer = dal.GetCustomer(customerId);
            if (!string.IsNullOrEmpty(newName))
            {
                customer.Name = newName;
            }
            if (!string.IsNullOrEmpty(newPhone))
            {
                customer.Phone = newPhone;
            }
            dal.UpdateCustomer(customerId,customer);
        }

        IEnumerable<StationToList> IBL.IBL.GetBaseStations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DroneToList> IBL.IBL.GetDrones()
        {
            throw new NotImplementedException();
        }

        IEnumerable<CustomerToList> IBL.IBL.GetCustomers()
        {
            throw new NotImplementedException();
        }
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
        IEnumerable<ParcelToList> IBL.IBL.GetParcels()
        {
            throw new NotImplementedException();
        }

        IEnumerable<ParcelToList> IBL.IBL.GetUnbelongParcels()
        {
            throw new NotImplementedException();
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
        IEnumerable<StationToList> IBL.IBL.AvailableSlots()
        {
            throw new NotImplementedException();
        }
       
    }
}
