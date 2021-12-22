using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Collections.ObjectModel;
using BO;
using Singleton;

namespace BL
{
    public sealed partial class BL : Singleton<BL>, BlApi.IBL
    {
        DalApi.IDal dal;
        List<DroneToList> lDroneToList;
        Random rand;
        static double powerConsumptionFree;
        static double powerConsumptionLight;
        static double powerConsumptionMedium;
        static double powerConsumptionHeavy;
        /// <summary>
        /// Charging rate per hour
        /// </summary>
        static double chargingRate;

        private BL()
        {
            rand = new Random();
            dal = DalApi.DalFactory.GetDal();
            InitializePowerConsumption();
            InitializeDroneList();
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
        private void InitializePowerConsumption()
        {
            (
                powerConsumptionFree,
                powerConsumptionLight,
                powerConsumptionMedium,
                powerConsumptionHeavy,
                chargingRate
            ) = dal.PowerConsumptionRequest();
        }
    }
}
//#region Erase?
///// <summary>
///// Get drone which his status is not 'Delivery'
///// Calculate his fields and returns it.
///// </summary>
///// <param name="nDrone"></param>
///// <returns></returns>
//private DroneToList CalculateUnDeliveryingDrone(DroneToList nDrone)
//{
//    nDrone.DStatus = (DroneStatus)rand.Next((int)DroneStatus.Free, (int)DroneStatus.Maintenance);
//    if (nDrone.DStatus == DroneStatus.Maintenance)
//    {
//        var stationDalList = dal.GetListFromDal<IDAL.DO.BaseStation>();
//        var station = stationDalList.ElementAt(rand.Next(stationDalList.Count()));
//        nDrone.CurrLocation = new Location(station.Longitude, station.Latitude);
//        nDrone.BatteryStatus = rand.NextDouble() * 20;
//    }
//    else // droneToList.DStatus == Free
//    {
//        var customersList = CustomersWithProvidedParcels();
//        nDrone.CurrLocation = customersList[rand.Next(customersList.Count)].Location;
//        var closetStation = ClosestStation(nDrone.CurrLocation);
//        double distance = CalculateDistance(nDrone.CurrLocation, closetStation.Location);
//        nDrone.BatteryStatus = rand.NextDouble() * (100 - MinBattery(distance)) + MinBattery(distance);
//    }
//    return nDrone;
////}
///// <summary>
///// A generic function that gets two types: DL, BL and id and 
///// pulls out from the dal the object that its type DL and its id is the
///// same as the id that the function gets and Expands this object to new object
///// of BL type and returns this object.
///// </summary>
///// <typeparam name="DL"></typeparam>
///// <typeparam name="BL"></typeparam>
///// <param name="Id"></param>
///// <returns>returns an object of BL type</returns>
//public BL GetBLById<DL, BL>(int Id) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject
//{
//    try
//    {
//        dynamic wantedDal = dal.GetFromDalById<DL>(Id);
//        BL wantedBl = ConvertToBL(wantedDal);
//        return wantedBl;
//    }
//    catch (DalObject.IdIsNotExistException)
//    {
//        throw new IdIsNotExistException(typeof(DL), Id);
//    }
//}
///// <summary>
///// A function that gets two types: DL, BL and 
///// pulls out the list from dal with DL type and 
///// creates list Of BL type and converts every object in the
///// list to DL type,to BL type and adds it to BL list and returns this List.
///// </summary>
///// <typeparam name="DL"></typeparam>
///// <typeparam name="BL"></typeparam>
///// <returns>returns list of data with type of BL. </returns>
//public IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject
//{
//    var bLList = new List<BL>();
//    var dalList = dal.GetListFromDal<DL>();
//    foreach (dynamic dlItem in dalList)
//    {
//        var blItem = ConvertToBL(dlItem);
//        bLList.Add(blItem);
//    }
//    return bLList;
//}
///// <summary>
///// A generic function that gets two types:DL, BLToList
///// and pulls out the list from dal with DL type and 
///// creates list Of BLToList type and converts every object in the
///// list to BLToList type and adds it to BLToList list and returns this List.
///// </summary>
///// <typeparam name="DL"></typeparam>
///// <typeparam name="BLToList"></typeparam>
///// <returns>returns list of data with the BLToList</returns>
//public IEnumerable<BLToList> GetListToList<DL, BLToList>(Predicate<DL> predicate = null) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject
//{
//    try
//    {
//        if (predicate == null && typeof(BLToList) == typeof(DroneToList))
//        {
//            return (IEnumerable<BLToList>)lDroneToList;
//        }
//        List<DL> dalList;
//        _ = predicate == null ? dalList = (List<DL>)dal.GetListFromDal<DL>() : dalList = (List<DL>)dal.GetDalListByCondition<DL>(predicate);
//        var listToList = new List<BLToList>();
//        foreach (dynamic dalItem in dalList)
//        {
//            var blToListItem = ConvertToList(dalItem);
//            listToList.Add(blToListItem);
//        }
//        return listToList;
//    }
//    catch (DalObject.InValidActionException)
//    {
//        throw new InValidActionException("There is no match object in the list ");
//    }
//}
//public IEnumerable<BLToList> GetListToList<BLToList>() 
//{
//    try
//    {
//        if (typeof(BLToList) == typeof(DroneToList))
//        {
//            return (IEnumerable<BLToList>)lDroneToList;
//        }
//        Type type = Extensions.matchType[typeof(BLToList)];
//        List<type> dalList = (List<type>)dal.GetListFromDal<type>();
//        var listToList = new List<BLToList>();
//        foreach (dynamic dalItem in dalList)
//        {
//            var blToListItem = ConvertToList(dalItem);
//            listToList.Add(blToListItem);
//        }
//        return listToList;
//    }
//    catch (DalObject.InValidActionException)
//    {
//        throw new InValidActionException("There is no match object in the list ");
//    }
//}
//#endregion





