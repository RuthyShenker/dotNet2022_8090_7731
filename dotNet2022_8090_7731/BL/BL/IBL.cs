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
        void AddingBaseStation(BL_Station baseStation);
        void AddingDrone(BL_Drone drone);
        void AddingCustomer(BL_Customer customer);
        void GettingParcelForDelivery(BL_Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int Id);
        void DeliveryPackage(int Id);
        void ChangeDroneStatus(int Id, DroneStatus newStatus);
        void ChargingDrone(int IdDrone);
        void ReleasingDrone(int dId);
        BL_Station BaseStationDisplay(int id);
        BL_Drone DroneDisplay(int Id);
        BL_Customer CustomerDisplay(string Id);
        BL_Parcel ParcelDisplay(int Id);
        IEnumerable<BL_Station> DisplayingBaseStations();
        IEnumerable<BL_Drone> GetDrones();
        IEnumerable<BL_Customer> DisplayingCustomers();
        IEnumerable<BL_Parcel> DisplayingParcels();
        IEnumerable<BL_Parcel> GetUnbelongParcels();
        IEnumerable<BL_Station> AvailableSlots();

    }
}
