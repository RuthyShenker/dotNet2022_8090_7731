using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    /// </summary>
    public interface IDal
    {

        void Add<T>(T item) where T : IDalObject;
        void Update<T>(int Id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject;
        void Remove<T>(T item) where T : IDalObject;
        bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject;
        T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable;
        T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject;

        IEnumerable<T> GetListFromDal<T>() where T : IDalObject;
        IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject;

        // ToDo this function public?
        (double,double,double,double,double) PowerConsumptionRequest();
        int GetIndexParcel();


        // שיניתי להרשאה פרטית
        //int SumDronesInStation(int sId);

        //bool IsExistInList<T>(List<T> list, Predicate<T> predicate) where T : IDalObject;

        //Adding:
        //void AddingBaseStation(BaseStation baseStation);
        //void AddingDrone(Drone drone);
        //void AddingCustomer(Customer customer);
        //void AddingParcel(Parcel parcel);


        // updating
        //void UpdateBelongedParcel(Parcel parcel, int dId);
        //void PickingUpParcel(int Id);
        //void ProvidingPackage(int Id);

        //void UpdateDrone(int dId, Drone drone);
        //void UpdateBaseStation(int bId, BaseStation baseStation);
        //void UpdateCustomer(int cId, Customer customer);

        //IEnumerable<Parcel> GetUnbelongParcels();
        //IEnumerable<BaseStation> AvailableSlots();
        //IEnumerable<ChargingDrone> GetChargingDrones();
    }
}