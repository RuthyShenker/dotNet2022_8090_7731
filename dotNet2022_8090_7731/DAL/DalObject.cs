using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DO;
using Singleton;
using System.Runtime.CompilerServices;

namespace Dal
{
    /// <summary>
    /// sealed partial class of DalObject:IDal
    /// </summary>
    internal sealed partial class DalObject : Singleton<DalObject>, DalApi.IDal
    /// <summary>
    /// A class that contains:
    /// Add
    /// BelongingParcel
    /// PickingUpParcel
    /// DeliveryPackage
    /// ChangeDroneStatus
    /// ChargingDrone
    /// ReleasingDrone
    /// BaseStationDisplay
    /// DroneDisplay
    /// CustomerDisplay
    /// ParcelDisplay
    /// GetBaseStations
    /// DisplayingDrones
    /// DisplayingCustomers
    /// DisplayingParcels
    /// DisplayingUnbelongParcels
    /// AvailableSlots
    /// </summary>
    {
        static DalObject()
        {

        }

        /// <summary>
        /// A constructor of DalObject that activates the function Initialize
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).Any(item => item.Id == Id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable
        {
            var item = GetFromDalByCondition<T>(item => item.Id == Id);

            if (item.Equals(default(T)))
                throw new IdDoesNotExistException($"Id: {Id} does not exist");
            else
                return item;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FirstOrDefault(item => predicate(item));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FindAll(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<T> GetListFromDal<T>() where T : IDalObject
        {
            return (List<T>)DataSource.Data[typeof(T)];
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalObject
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add<T>(T item) where T : IDalObject
        {
            Type type = typeof(T);
            if (DoesExistInList(item))
            {
                throw new InValidActionException($" This item already exists in list {type}");
            }
            ((List<T>)DataSource.Data[typeof(T)]).Add(item);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject
        {
            Type type = typeof(T);
           
            var oldItem = GetFromDalById<T>(id);

            DataSource.Data[type].Remove(oldItem);
            //if (newValue != null)//TODO: //האם צרחך את הבדיקה הזאת?
            //{
                //type.GetProperty(propertyName).SetValue(oldItem, newValue);
                T obj = oldItem;
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                object boxed = obj;
                propertyInfo.SetValue(boxed, newValue, null);
                obj = (T)boxed;
                oldItem = obj;
            //}
            DataSource.Data[type].Add(oldItem);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove<T>(T item) where T : IDalObject
        {
            if (DoesExistInList(item))
            {
                DataSource.Data[typeof(T)].Remove(item);
            }
        }

        private bool DoesExistInList<T>(T item) where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).Any(i => i.Equals(item));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetIndexParcel()
        {
            return ++DataSource.Config.IndexParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            return (DataSource.Config.Available, DataSource.Config.LightWeight, DataSource.Config.MediumWeight, 
                DataSource.Config.HeavyWeight, DataSource.Config.ChargingRate);
        }
    }
}
