using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    /// <summary>
    /// An interface of IDal-functions to get db details ,contains:
    /// Add
    /// Update
    /// Remove
    /// GetFromDalById
    /// GetFromDalByCondition
    /// GetListFromDal
    /// GetDalListByCondition
    /// GetIndexParcel
    /// IsIdExistInList
    /// PowerConsumptionRequest
    /// </summary>
    public interface IDal
    {
        //ADD:
        void Add<T>(T item) where T : IDalDo;

        //UPDATE:
        void Update<T>(int Id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalDo;

        //REMOVE:
        void Remove<T>(T item) where T : IDalDo;

        //GET:
        T GetFromDalById<T>(int Id) where T : IDalDo, IIdentifiable;
        T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalDo;
        IEnumerable<T> GetListFromDal<T>() where T : IDalDo;
        IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalDo;
        int GetIndexParcel();

        //EXIST:
        bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalDo;

        //DATA:
        (double, double, double, double, double) PowerConsumptionRequest();

        #region
        // שיניתי להרשאה פרטית
        //int SumDronesInStation(int sId);

        //bool IsExistInList<T>(List<T> list, Predicate<T> predicate) where T : IDalDo;

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
        #endregion
    }
}