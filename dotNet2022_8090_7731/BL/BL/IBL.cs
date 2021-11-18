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
        void GettingParcelForDelivery(IDal.DO.Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int Id);
        void DeliveryPackage(int Id);
        void ChangeDroneStatus(int Id, DroneStatus newStatus);
        void ChargingDrone(int IdDrone);
        void ReleasingDrone(int dId);
        void ReleasingDrone(int dId, double timeInCharging);

        //Adding
        void AddingBaseStation(IDal.DO.BaseStation baseStation);
        void AddingDrone(IDal.DO.Drone drone, int numberStation);
        void AddingCustomer(IDal.DO.Customer customer);



       


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





        //צריך להיות בממשק או רק
        //BLב
        void UpdatingDroneName(int droneId, string newModel);
        void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions);

        void UpdatingCustomerDetails(string customerId, string newName, string newPhone);

    }
}
