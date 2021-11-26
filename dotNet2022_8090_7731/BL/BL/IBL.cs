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
    public interface IBL
    {
        //BelongingParcel
        void BelongingParcel(int pId);
        //PickingUpParcel
        void PickingUpParcel(int dId);
        //אספקת חבילה
        void DeliveryPackage(int Id);

        //void ChangeDroneStatus(int Id, DroneStatus newStatus);

        //ChargingDrone
        void SendingDroneToCharge(int IdDrone);

        //void ReleasingDrone(int dId);
        //ReleasingDrone
        void ReleasingDrone(int dId, double timeInCharging);

        //Adding
        void AddingBaseStation(Station baseStation);
        void AddingDrone(Drone drone, int numberStation);
        void AddingCustomer(Customer customer);
        void AddingParcel(Parcel parcel);
        //update
        void UpdatingDroneName(int droneId, string newModel);
        void UpdatingStationDetails(int stationId, string stationName, string amountOfPositions);
        void UpdatingCustomerDetails(int customerId, string newName, string newPhone);

        // get lists
        IEnumerable<BL> GetListOfBL<DL, BL>() where DL : IDal.DO.IIdentifiable;
        IEnumerable<BLToList> GetListToList<DL, BLToList>() where DL : IDal.DO.IIdentifiable;
        IEnumerable<Station> AvailableSlots();

        // get specific
        BL GetBLById<DL, BL>(int Id) where DL: IDal.DO.IIdentifiable;
        // GetListFromBL יש פונקציה גנרית
        // IEnumerable<StationToList> GetStations();
        // IEnumerable<DroneToList> GetDrones();
        // IEnumerable<CustomerToList> GetCustomers();
        // IEnumerable<ParcelToList> GetParcels();
        // IEnumerable<ParcelToList> GetUnbelongParcels();
        // IEnumerable<StationToList> AvailableSlots();

        //  GetItemFromBLById יש פונקציה גנרית 
        // Station GetStation(int id);
        // Drone GetDrone(int Id);
        // Customer GetCustomer(string Id);
        // Parcel GetParcel(int Id);

    }
}
