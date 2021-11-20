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
        void PickingUpParcel(int Id);
        //אספקת חבילה
        void DeliveryPackage(int Id);

        //void ChangeDroneStatus(int Id, DroneStatus newStatus);

        //ChargingDrone
        void ChargingDrone(int IdDrone);

        //void ReleasingDrone(int dId);
        //ReleasingDrone
        void ReleasingDrone(int dId, double timeInCharging);

        //Adding
        void AddingBaseStation(Station baseStation);
        void AddingDrone(Drone drone, int numberStation);
        void AddingCustomer(Customer customer);
        void AddingParcel(Parcel parcel);
        //update
        void UpdatingDroneName(int droneId, int newModel);
        void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions);
        void UpdatingCustomerDetails(string customerId, string newName, string newPhone);


        // get lists
        IEnumerable<StationToList> GetStations();
        IEnumerable<DroneToList> GetDrones();
        IEnumerable<CustomerToList> GetCustomers();
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetUnbelongParcels();
        IEnumerable<StationToList> AvailableSlots();

        // get specific
        Station GetStation(int id);
        Drone GetDrone(int Id);
        Customer GetCustomer(string Id);
        Parcel GetParcel(int Id);


    }
}
