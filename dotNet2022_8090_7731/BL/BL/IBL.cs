using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
   public interface IBL
    {
        //Adding
        void AddingBaseStation(BL_Station baseStation);
        void AddingDrone(BL_Drone drone,int numberStation);
        void AddingCustomer(BL_Customer customer);
        void GettingParcelForDelivery(BL_Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int Id);
        void DeliveryPackage(int Id);
        void ChangeDroneStatus(int Id, DroneStatus newStatus);
        void ChargingDrone(int IdDrone);
        void ReleasingDrone(int dId);
        Station BaseStationDisplay(int id);
        Drone GetDrone(int Id);
        Customer CustomerDisplay(string Id);
        Parcel ParcelDisplay(int Id);
        IEnumerable<Station> DisplayingBaseStations();
        IEnumerable<DroneToList> GetDrones();
        IEnumerable<Customer> DisplayingCustomers();
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<Parcel> GetUnbelongParcels();
        IEnumerable<Station> AvailableSlots();

        void ReleasingDrone(int dId,double timeInCharging);
        BL_Station BaseStationDisplay(int id);
        BL_Drone DroneDisplay(int Id);
        BL_Customer CustomerDisplay(string Id);
        BL_Parcel ParcelDisplay(int Id);
        IEnumerable<StationToList> GetBaseStations();
        IEnumerable<DroneToList> GetDrones();
        IEnumerable<CustomerToList> GetCustomers();
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetUnbelongParcels();
        IEnumerable<StationToList> AvailableSlots();
        //צריך להיות בממשק או רק
        //BLב
        void UpdatingDroneName(int droneId, string newModel);
        void UpdatingStationDetails(int stationId,string stationName,int amountOfPositions);
      
        void  UpdatingCustomerDetails(string customerId,string newName,string newPhone);
       
    }
}
