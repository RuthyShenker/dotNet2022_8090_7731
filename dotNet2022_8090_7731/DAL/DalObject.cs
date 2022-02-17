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
    /// internal sealed partial class of DalObject that inherits Singleton<DalObject>
    /// and impliments DalApi.IDal.
    /// this class includes:
    /// IsIdExistInList
    /// GetFromDalById
    /// GetFromDalByCondition
    /// GetDalListByCondition
    /// GetListFromDal
    /// Add
    /// Update
    /// Remove
    /// GetIndexParcel
    /// PowerConsumptionRequest
    /// DoesExistInList
    /// </summary>
    internal sealed partial class DalObject : Singleton<DalObject>, DalApi.IDal
    {
        /// <summary>
        /// A static constructor of DalObject.
        /// </summary>
        static DalObject()
        {

        }

        /// <summary>
        /// A private constructor of DalObject that activates the function Initialize
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// A generic function that gets id of entity of type of T and checks if
        /// it is in the list of it's type or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns>
        /// returns true if this id exists in the list of
        /// it's type or false if not.
        /// </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalDo
        {
            return ((List<T>)DataSource.Data[typeof(T)]).Any(item => item.Id == Id);
        }

        /// <summary>
        /// A generic function that gets id of entity  of type of T and returns this entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns>returns the entity that it's id is the same as the parameter id.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetFromDalById<T>(int Id) where T : IDalDo, IIdentifiable
        {
            var item = GetFromDalByCondition<T>(item => item.Id == Id);

            if (item.Equals(default(T)))
                throw new IdDoesNotExistException($"Id: {Id} does not exist");
            else
                return item;
        }

        /// <summary>
        /// A generic function that gets generic predicate of type of T and returns the first or default 
        /// entity that stands on this predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>the first or default 
        /// entity that stands on this predicate.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalDo
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FirstOrDefault(item => predicate(item));
        }

        /// <summary>
        /// A generic function that gets generic predicate of type of T and 
        /// returns all the entities that stand on this predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>returns all the entities that stand on this predicate.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalDo
        {
            return ((List<T>)DataSource.Data[typeof(T)]).FindAll(predicate);
        }

        /// <summary>
        /// A generic function that returns all the entities of type of T. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>returns IEnumerable<T> all the entities of type of T. </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<T> GetListFromDal<T>() where T : IDalDo
        {
            return (List<T>)DataSource.Data[typeof(T)];
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalDo
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}

        /// <summary>
        /// A generic function that gets an entity of type of T 
        /// and add it to the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add<T>(T item) where T : IDalDo
        {
            Type type = typeof(T);
            if (DoesExistInList(item))
            {
                throw new InValidActionException($" This item already exists in list {type}");
            }
            ((List<T>)DataSource.Data[typeof(T)]).Add(item);
        }


        /// <summary>
        /// A generic function that gets id of entity of type of T to update and new value and the propertyName
        /// and update this entity in this propertyName with the new value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalDo
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

        /// <summary>
        /// A generic function that gets entity of type of T and remove it 
        /// from the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove<T>(T item) where T : IDalDo
        {
            if (DoesExistInList(item))
            {
                DataSource.Data[typeof(T)].Remove(item);
            }
        }

        /// <summary>
        /// A function that returns the index of the Parcel.
        /// </summary>
        /// <returns> returns the running number.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetIndexParcel()
        {
            return ++DataSource.Config.IndexParcel;
        }

        /// <summary>
        /// A function that returns the powerConsumptions.
        /// </summary>
        /// <returns>returns the powerConsumption.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            return (DataSource.Config.Available, DataSource.Config.LightWeight, DataSource.Config.MediumWeight, 
                DataSource.Config.HeavyWeight, DataSource.Config.ChargingRate);
        }

        /// <summary>
        /// A generic function that gets entity of type of T and returns if it exists in the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>returns true if it exists in the list of type of T else false.</returns>
        private bool DoesExistInList<T>(T item) where T : IDalDo
        {
            return ((List<T>)DataSource.Data[typeof(T)]).Any(i => i.Equals(item));
        }
    }
}
