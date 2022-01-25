﻿using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DalXml
{
    public class DalXml
    {
        //??
        readonly string xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\..\..\..\..\XmlFiles";
        //???
        string GetXmlFilePath(Type type) => $@"{xmlFilesLocation}\{type.Name}.xml";
        string configFilePath => $@"{xmlFilesLocation}\config.xml";

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject
        {
            try
            {
                XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
                XElement root = document.Root;
                XElement e = (from element in root.Elements()
                           where Int32.Parse(element.Element("Id").Value)==Id
                           select element).FirstOrDefault();
                if (e == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
            //return ((List<T>)DataSource.Data[typeof(T)]).Any(item => item.Id == Id);
        }

        public T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable
        {
            //var item = GetFromDalByCondition<T>(item => item.Id == Id);

            //if (item.Equals(default(T)))
            //    throw new IdIsNotExistException();
            //else
            //    return item;

            //try
            //{
                //XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
                //XElement root = document.Root;
                //XElement e = (from element in root.Elements()
                //              where Int32.Parse(element.Element("Id").Value) == Id
                //              select element).FirstOrDefault();
                ////
                ////if (e == null) return ;
                //return Extensions.MapFromXmlToObj<T>(e);

                StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                IEnumerable<T> List = (IEnumerable<T>)serializer.Deserialize(reader);
                return List.First(obj=>obj.Id==Id);
            //}
            //catch
            //{
            //    //return false;
            //}

        }

       

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            //return ((List<T>)DataSource.Data[typeof(T)]).FirstOrDefault(item => predicate(item));


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
}