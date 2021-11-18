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
        
        void AddingBaseStation(BaseStation baseStation);
        void AddingDrone(Drone drone);
        void AddingCustomer(Customer customer);
        void GettingParcelForDelivery(Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int Id);
        void DeliveryPackage(int Id);
        void ChargingDrone(int IdDrone);
        void ReleasingDrone(int dId);

        // GetFromDalById
        //BaseStation GetBaseStation(int id);
        //Drone GetDrone(int Id);
        //Customer GetCustomer(string Id);
        //Parcel GetParcel(int Id);


        // GetListFromDal
        //IEnumerable<BaseStation> GetBaseStations();
        //IEnumerable<Drone> GetDrones();
        //IEnumerable<Customer> GetCustomers();
        //IEnumerable<Parcel> GetParcels();

        IEnumerable<Parcel> GetUnbelongParcels();
        IEnumerable<BaseStation> AvailableSlots();

        void UpdateDrone(int dId, Drone drone);
        void UpdateBaseStation(int bId, BaseStation baseStation);
        void UpdateCustomer(string cId, Customer customer);
        bool ExistsInBaseStation(int id);
        bool ExistsInDroneList(int id);
        bool ExistsInParcelList(int pId);
        bool ThereAreFreePositions(int sId);
        int SumOfDronesInSpecificStation(int sId);
        bool ExistsInCustomerList(string cId);
        double[] PowerConsumptionRequest();
        void AddDroneToCharge(int dId, int sId);
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