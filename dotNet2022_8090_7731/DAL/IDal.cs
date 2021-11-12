using IDal.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDal
    {
        void AddingBaseStation(BaseStation baseStation);
        void AddingDrone(Drone drone);
        void AddingCustomer(Customer customer);
        void GettingParcelForDelivery(Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int Id);
        void DeliveryPackage(int Id);
        void ChangeDroneStatus(int Id, DroneStatuses newStatus);
        void ChargingDrone(int IdDrone);
        void ReleasingDrone(int dId);
        BaseStation BaseStationDisplay(int id);
        Drone DroneDisplay(int Id);
        Customer CustomerDisplay(string Id);
        Parcel ParcelDisplay(int Id);
        IEnumerable<BaseStation> DisplayingBaseStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<Customer> DisplayingCustomers();
        IEnumerable<Parcel> DisplayingParcels();
        IEnumerable<Parcel> GetUnbelongParcels();
        IEnumerable<BaseStation> AvailableSlots();

        double[] PowerConsumptionRequest();
    }
    //public abstract class IDalGeneric<T>
    //{
    //    List<T> _list;
    //    public IDalGeneric()
    //    {
    //        _list = new List<T>();
    //    }

    //    public abstract void Add {
    //        }
    //    IEnumerable<T> GetList()
    //    {
    //        return _list.Select( item=>new T(item)).ToList();
    //    }
    //}
}