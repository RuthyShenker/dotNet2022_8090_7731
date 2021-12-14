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
    /// <summary>
    ///An interface of IDal 
    ///contains:
    ///AddingBaseStation
    ///AddingDrone
    ///AddingCustomer
    ///AddingParcel
    ///AddingDroneToCharge
    ///BelongingParcel
    ///PickingUpParcel
    ///ProvidePackage
    ///ReleasingDrone
    ///GetUnbelongParcels
    ///AvailableSlots
    ///GetFromDalById<T>
    ///GetFromDalByCondition<T>
    ///GetDalListByCondition<T>
    ///IsExistInList<T>
    ///IsIdExistInList<T>
    ///GetListFromDal<T>
    ///UpdateDrone
    ///UpdateBaseStation
    ///UpdateCustomer
    ///ExistsInBaseStation
    ///ExistsInDroneList
    ///ExistsInParcelList
    ///ThereAreFreePositions
    ///SumOfDronesInSpecificStation
    ///ExistsInCustomerList
    ///GetChargingDrones
    ///PowerConsumptionRequest

    /// </summary>
    public interface IDal
    {
        void AddingToData<T>(T item) where T : IDalObject;

        void UpdatingInData<T>(int Id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject;
        void ReleasingDrone(int dId);
       
        T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable;
        T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject;

        IEnumerable<T> GetListFromDal<T>() where T : IDalObject;
        IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject;

        bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject;
        int AreThereFreePositions(int sId);
        double[] PowerConsumptionRequest();

       
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