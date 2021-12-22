﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using Singleton;
using static Dal.DataSource.Config;
using static Dal.DataSource;
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
        /// <summary>
        /// A constructor of DalObject that activates the function Initialize
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).Any(item => item.Id == Id);
        }

        public T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable
        {
            var item = GetFromDalByCondition<T>(item => item.Id == Id);

            if (item.Equals(default(T)))
                throw new IdIsNotExistException();
            else
                return item;
        }

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FirstOrDefault(item => predicate(item));
        }

        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FindAll(predicate).ToList();
        }

        public IEnumerable<T> GetListFromDal<T>() where T : IDalObject
        {
            return ((List<T>)DataSource.Data[typeof(T)]).ToList();
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalObject
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}
        public void Add<T>(T item) where T : IDalObject
        {
            Type type = typeof(T);
            if (DoesExistInList(item))
            {
                throw new InValidActionException($" This item already exists in list {type}");
            }
            ((List<T>)DataSource.Data[typeof(T)]).Add(item);
        }

        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject
        {
            Type type = typeof(T);

            var oldItem = GetFromDalById<T>(id);

            DataSource.Data[type].Remove(oldItem);
            if (newValue != null)//TODO: //האם צרחך את הבדיקה הזאת?
            {
                //type.GetProperty(propertyName).SetValue(oldItem, newValue);
                T obj = oldItem;
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                object boxed = obj;
                propertyInfo.SetValue(boxed, newValue, null);
                obj = (T)boxed;
                oldItem = obj;
            }
            DataSource.Data[type].Add(oldItem);
        }

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

        public int GetIndexParcel()
        {
            return ++DataSource.Config.IndexParcel;
        }

        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            return (Available, LightWeight, MediumWeight, HeavyWeight, ChargingRate);

        }
    }
}
