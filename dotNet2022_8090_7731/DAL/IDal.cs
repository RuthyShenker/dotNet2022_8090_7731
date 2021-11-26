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
        void AddingParcel(Parcel parcel);
        void AddDroneToCharge(int dId, int sId);
        void BelongingParcel(Parcel parcel, int dId);
        void PickingUpParcel(int Id);
        void ProvidePackage(int Id);
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

        T GetFromDalById<T>(int Id) where T : IIdentifiable;
        T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IIdentifiable;
        IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IIdentifiable;

        bool IsExistInList<T>(List<T> list, Predicate<T> predicate);

        bool IsIdExistInList<T>(int Id) where T : IIdentifiable;

        IEnumerable<T> GetListFromDal<T>() where T : IIdentifiable;
        

        void UpdateDrone(int dId, Drone drone);
        void UpdateBaseStation(int bId, BaseStation baseStation);
        void UpdateCustomer(int cId, Customer customer);
        bool ExistsInBaseStation(int id);
        bool ExistsInDroneList(int id);
        Drone GetDrone(int id);
        bool ExistsInParcelList(int pId);
        bool ThereAreFreePositions(int sId);
        int SumOfDronesInSpecificStation(int sId);
        bool ExistsInCustomerList(int cId);



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