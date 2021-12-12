using DalObject;
using static System.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
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
    public interface IBL
    {
        //Belonging Parcel
        void BelongingParcel(int pId);
        //Picking Up Parcel
        void PickingUpParcel(int dId);
        //Package delivery
        void DeliveryPackage(int Id);

        //void ChangeDroneStatus(int Id, DroneStatus newStatus);

        //Charging Drone
        void SendingDroneToCharge(int IdDrone);

        //void ReleasingDrone(int dId);

        //Releasing Drone from charging
        void ReleasingDrone(int dId, double timeInCharging);

        //Adding
        void AddingBaseStation(Station baseStation);
        void AddingDrone(Drone drone, int numberStation);
        int AddingCustomer(Customer customer);
        int AddingParcel(Parcel parcel);
        //update
        void UpdatingDroneName(int droneId, string newModel);
        void UpdatingStationDetails(int stationId, string stationName, string amountOfPositions);
        void UpdatingCustomerDetails(int customerId, string newName, string newPhone);

        //get lists
        IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDal.DO.IIdentifiable,IDal.DO.IDalObject;
        //IEnumerable<BLToList> GetListToList<DL, BLToList>() where DL : IDal.DO.IIdentifiable, IDal.DO.IDalObject;
        IEnumerable<BLToList> GetListToList<DL, BLToList>(Predicate<DL> predicate=null) where DL : IDal.DO.IIdentifiable, IDal.DO.IDalObject;
        //list of: Available Slots
        //IEnumerable<StationToList> AvailableSlots();

        // get specific
        BL GetBLById<DL, BL>(int Id) where DL: IDal.DO.IIdentifiable, IDal.DO.IDalObject;
        // GetListFromBL יש פונקציה גנרית
        // IEnumerable<StationToList> GetStations();
        // IEnumerable<DroneToList> GetDrones();
        // IEnumerable<CustomerToList> GetCustomers();
        // IEnumerable<ParcelToList> GetParcels();

        //Get Unbelong Parcels 
        //IEnumerable<ParcelToList> GetUnbelongParcels();

        // IEnumerable<StationToList> AvailableSlots();

        //  GetItemFromBLById יש פונקציה גנרית 
        // Station GetStation(int id);
        // Drone GetDrone(int Id);
        // Customer GetCustomer(string Id);
        // Parcel GetParcel(int Id);

    }
}
