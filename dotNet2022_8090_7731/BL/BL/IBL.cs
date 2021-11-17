using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    interface IBL
    {
        //Adding
        void AddingBaseStation(Station baseStation);
        void AddingDrone(Drone drone);
        void AddingCustomer(Customer customer);
        void GettingParcelForDelivery(Parcel parcel);
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

    }
}
