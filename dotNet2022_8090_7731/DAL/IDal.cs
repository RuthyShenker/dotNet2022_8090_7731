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
        // adding
        void AddingBaseStation(BaseStation baseStation);
        void AddingDrone(Drone drone);
        void AddingCustomer(Customer customer);
        void AddingParcel(Parcel parcel);

        void AddingItemToDList<T>(T item) where T : IIdentifiable;

        void AddDroneToCharge(int dId, int sId);
        
        // updating
        void UpdateBelongedParcel(Parcel parcel, int dId);
        void PickingUpParcel(int Id);
        void ProvidingPackage(int Id);
        void ReleasingDrone(int dId);
        void UpdateDrone(int dId, Drone drone);
        void UpdateBaseStation(int bId, BaseStation baseStation);
        void UpdateCustomer(int cId, Customer customer);
        int SumOfDronesInSpecificStation(int sId);

        // get an item
        T GetFromDalById<T>(int Id) where T : IIdentifiable;
        T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IIdentifiable;
       
        // get list
        IEnumerable<T> GetListFromDal<T>() where T : IIdentifiable;
        IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IIdentifiable;
        IEnumerable<Parcel> GetUnbelongParcels();
        IEnumerable<BaseStation> AvailableSlots();
        IEnumerable<ChargingDrone> GetChargingDrones();

        // bool functions
        bool IsIdExistInList<T>(int Id) where T : IIdentifiable;
        bool IsExistInList<T>(List<T> list, Predicate<T> predicate);
        bool AreThereFreePositions(int sId);
        double[] PowerConsumptionRequest();
    }
}