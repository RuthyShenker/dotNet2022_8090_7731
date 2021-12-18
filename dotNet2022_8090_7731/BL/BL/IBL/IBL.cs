using DalObject;
using static System.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace IBL
{
    /// <summary>
    ///An interface of IBL that contains:
    ///BelongingParcel
    ///PickingUpParcel
    ///DeliveryPackage
    ///SendingDroneToCharge
    ///ReleasingDrone
    ///AddingBaseStation
    ///AddingDrone
    ///AddingCustomer
    ///AddingParcel
    ///UpdatingDroneName
    ///UpdatingStationDetails
    ///UpdatingCustomerDetails
    ///GetListOfBL
    ///GetListToList
    ///AvailableSlots
    ///GetBLById
    ///GetUnbelongParcels
    /// </summary>
    public interface IBL : IBLCustomer, IBLDrone, IBLParcel, IBLStation
    {

        //IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject;
        //IEnumerable<BLToList> GetListToList<DL, BLToList>(Predicate<DL> predicate = null) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject;
        //BL GetBLById<DL, BL>(int Id) where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject;




        //Belonging Parcel

        //Picking Up Parcel

        //Package delivery


        //void ChangeDroneStatus(int Id, DroneStatus newStatus);

        //Charging Drone

        //void ReleasingDrone(int dId);

        //Releasing Drone from charging


        //Adding



        //update



        //get lists

        //IEnumerable<BLToList> GetListToList<DL, BLToList>() where DL : IDAL.DO.IIdentifiable, IDAL.DO.IDalObject;


        //list of: Available Slots


        // get specific

        // GetListFromBL יש פונקציה גנרית
        // IEnumerable<StationToList> GetStations();
        // IEnumerable<DroneToList> GetDrones();
        // IEnumerable<CustomerToList> GetCustomers();
        // IEnumerable<ParcelToList> GetParcels();

        //Get Unbelong Parcels 


        //IEnumerable<StationToList> AvailableSlots();
        //  GetItemFromBLById יש פונקציה גנרית 
        // Station GetStation(int id);
        // Drone GetDrone(int Id);
        // Customer GetCustomer(string Id);
        // Parcel GetParcel(int Id);
    }
}
