﻿using BO;
using Singleton;
using System;
using System.Collections.Generic;

namespace BL
{
    /// <summary>
    /// An internal sealed partial class BL inherits from Singleton<BL>,and impliments BlApi.IBL,
    /// </summary>
    internal sealed partial class BL : Singleton<BL>, BlApi.IBL
    {

        /// <summary>
        /// instance of dal.
        /// </summary>
        internal DalApi.IDal dal = DalApi.DalFactory.GetDal();

        /// <summary>
        /// list of drones of type DroneToList.
        /// </summary>
        internal List<DroneToList> lDroneToList;

        /// <summary>
        /// an instance of class Random.
        /// </summary>
        readonly Random rand;

        //internal static double powerConsumptionFree;

        /// <summary>
        ///data of Power Consumptions.
        /// </summary>
        internal static double powerConsumptionLight { get; set; }
        internal static double powerConsumptionMedium {get; set; }
        internal static double powerConsumptionHeavy {get; set; }
        internal static double PowerConsumptionFree { get; set; }

        /// <summary>
        /// Charging rate per hour
        /// </summary>
        //static double chargingRate; שינית לinternal צריך גם בsimulator
        internal static double chargingRate { get; set; }

        /// <summary>
        /// A private constructor of BL that Initialize Power Consumptions, Initialize Drone List,initialize an instance of dal and rand.
        /// </summary>
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
        /// powerConsumptionLight
        /// powerConsumptionMedium
        /// powerConsumptionHeavy
        /// chargingRate
        /// this function doesn't return any thing.
        /// </summary>
        private void InitializePowerConsumption()
        {
            (
                PowerConsumptionFree,
                powerConsumptionLight,
                powerConsumptionMedium,
                powerConsumptionHeavy,
                chargingRate
            ) = dal.PowerConsumptionRequest();
        }

    }
}
#region Erase?
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
//public BL GetBLById<DL, BL>(int Id) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalDo
//{
//    try
//    {
//        dynamic wantedDal = dal.GetFromDalById<DL>(Id);
//        BL wantedBl = ConvertToBL(wantedDal);
//        return wantedBl;
//    }
//    catch (DalObject.IdDoesNotExistException)
//    {
//        throw new IdDoesNotExistException(typeof(DL), Id);
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
//public IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalDo
//{
//    var bLList = new List<BL>();
//    var dalList = dal.GetListFromDal<DL>();
//    foreach (dynamic dlItem in dalList)
//    {
//        var blItem = ConvertToBL(dlItem);
//        bLList.AddCustomer(blItem);
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
//public IEnumerable<BLToList> GetListToList<DL, BLToList>(Predicate<DL> predicate = null) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalDo
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
//            listToList.AddCustomer(blToListItem);
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
//            listToList.AddCustomer(blToListItem);
//        }
//        return listToList;
//    }
//    catch (DalObject.InValidActionException)
//    {
//        throw new InValidActionException("There is no match object in the list ");
//    }
//}
#endregion





