using DalObject;
using IDal.DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDal
    {
        IEnumerable GetSpecificItem(Type type, object Id)
        {
            return DataSource.data[type].Cast<IIdentifiable>().Where(item => item.Id == Id);
        }


        IEnumerable GetList(Type type)
        {
            return DataSource.data[type];
        }

        public object DisplayItem<T>(List<T> list, object Id)
        {

            return list[Id].Clone();
        }

        public object DisplayItem<T>(List<T> list)
        {

            return list.Clone();
        }
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
        BaseStation GetBaseStation(int id);
        Drone GetDrone(int Id);
        Customer GetCustomer(string Id);
        Parcel GetParcel(int Id);
        IEnumerable<BaseStation> GetBaseStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<Parcel> GetUnbelongParcels();
        IEnumerable<BaseStation> AvailableSlots();
        void UpdateDrone(int dId, Drone drone);
        void UpdateBaseStation(int bId, BaseStation baseStation);
        void UpdateCustomer(string cId, Customer customer);
        bool ExistsInBaseStation(int id);
        bool ExistsInDroneList(int id);
        bool ThereAreFreePositions(int sId);
        int SumOfDronesInSpecificStation(int sId);
        bool ExistsInCustomerList(string cId);
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