using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    /// <summary>
    /// partial class of DalObject:IDal
    /// </summary>
    public partial class DalObject : IDal.IDal
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
        public DalObject()
        {
            DataSource.Initialize();
        }

        //Update Generic:
        //void Update<T>(int id, T obj) where T : IDalObject, IIdentifiable
        //{
        //    data[typeof(T)].Remove(((List<T>)data[typeof(T)]).Find(Object => Object.Id == id));
        //    data[typeof(T)].Add(obj);
        //}

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
            //TODO: בדיקת תקינות
            ((List<T>)DataSource.Data[typeof(T)]).Add(item);
        }
        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject
        {
            Type type = typeof(T);
            var oldItem = ((List<T>)DataSource.Data[type]).FirstOrDefault(item => item.Id == id);
            if (oldItem.Equals(default(T)))
            {
                throw new IdIsNotExistException();
            }

           

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
            DataSource.Data[typeof(T)].Remove(item);
        }
    }
}
