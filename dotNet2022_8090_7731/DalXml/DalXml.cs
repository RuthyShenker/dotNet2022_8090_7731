using DalXml;
using DO;
using Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    /// <summary>
    /// A class DalXml that inherits from Singleton<DalXml> and Implements interface DalApi.IDal.
    /// this class includes functions:
    /// IsIdExistInList
    /// GetFromDalById
    ///GetFromDalByCondition
    ///GetDalListByCondition
    ///GetListFromDal
    ///Add
    ///Update
    ///Remove
    ///DoesExistInList
    ///GetIndexParcel
    ///PowerConsumptionRequest
    /// </summary>
    internal sealed partial class DalXml : Singleton<DalXml>, DalApi.IDal
    {
        /// <summary>
        /// A readonly field of all xml files location.
        /// </summary>
        readonly string xmlFilesLocation;

        /// <summary>
        /// A function that gets Data type and returns the xml file location of this type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetXmlFilePath(Type type) => $@"{xmlFilesLocation}\{type.Name}List.xml";

        /// <summary>
        ///  A readonly field of all config file location.
        /// </summary>
        string ConfigFilePath => $@"{xmlFilesLocation}\Config.xml";

        /// <summary>
        /// A Static constructor of DalXml.
        /// </summary>
        static DalXml()
        {

        }

        /// <summary>
        /// A private constructor of DalXml.
        /// </summary>
        private DalXml()
        {
            xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\..\..\XmlFiles";

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
        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalDo
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                return XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone))).Any(item => item.Id == Id);
            }
            else
            {
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                .Any(item => item.Id == Id);
            }
        }

        /// <summary>
        /// A generic function that gets id of entity  of type of T and returns this entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns>returns the entity that it's id is the same as the parameter id.</returns>
        public T GetFromDalById<T>(int Id) where T : IDalDo, IIdentifiable
        {
            var item = GetFromDalByCondition<T>(item => item.Id == Id);

            if (item.Equals(default(T)))
            {
                throw new IdDoesNotExistException();
            }
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
        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalDo
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                var drone = XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone))).FirstOrDefault(item =>
                            predicate((T)Convert.ChangeType(item, typeof(T))));
                return (T)Convert.ChangeType(drone, typeof(T));
            }
            else
            {
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                .FirstOrDefault(item => predicate(item));
            }

        }

        /// <summary>
        /// A generic function that gets generic predicate of type of T and 
        /// returns all the entities that stand on this predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>returns all the entities that stand on this predicate.</returns>
        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalDo
        {
            //problem:!!!
            if (typeof(T) == typeof(DO.Drone))
                return XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone)))?.Cast<T>().Where(item => predicate(item));
            else
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))?
              .FindAll(predicate);
        }

        /// <summary>
        /// A generic function that returns all the entities of type of T. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>returns IEnumerable<T> all the entities of type of T. </returns>
        public IEnumerable<T> GetListFromDal<T>() where T : IDalDo
        {
            if (typeof(T) == typeof(DO.Drone))
                return (IEnumerable<T>)XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone)));
            else
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate) where T : IDalDo
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}

        /// <summary>
        /// A generic function that gets an entity of type of T 
        /// and add it to the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Add<T>(T item) where T : IDalDo
        {
            Type type = typeof(T);
            if (DoesExistInList(item))
            {
                throw new InValidActionException($" This item already exists in list {type}");
            }

            if (typeof(T) == typeof(DO.Drone))
            {
                var droneList = XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone))).ToList();
                Drone drone = (Drone)Convert.ChangeType(item, typeof(Drone));
                droneList.Add(drone);
                XMLTools.SaveDroneListToXmlWithXElement(droneList, GetXmlFilePath(typeof(Drone)));
            }
            else
            {
                List<T> list = XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
                list.Add(item);
                XMLTools.SaveListToXmlSerializer<T>(list, GetXmlFilePath(typeof(T)));
            }
        }

        /// <summary>
        /// A generic function that gets id of entity of type of T to update and new value and the propertyName
        /// and update this entity in this propertyName with the new value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalDo
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                XElement root = XDocument.Load(GetXmlFilePath(typeof(T))).Root;
                XElement element = root.Elements().Single(obj =>
                int.Parse(obj.Element("Id").Value) == id);
                element.Element(propertyName).RemoveAttributes();
                element.SetElementValue(propertyName, newValue);
                root.Save(GetXmlFilePath(typeof(T)));
            }
            else
            {
                List<T> list = XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));

                Type type = typeof(T);
                var oldItem = list.FirstOrDefault(item => item.Id == id);
                list.Remove(oldItem);

                if (newValue != null)
                {
                    //type.GetProperty(propertyName).SetValue(oldItem, newValue);
                    T obj = oldItem;
                    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                    object boxed = obj;
                    propertyInfo.SetValue(boxed, newValue, null);
                    obj = (T)boxed;
                    oldItem = obj;
                }

                list.Add(oldItem);

                XMLTools.SaveListToXmlSerializer<T>(list, GetXmlFilePath(typeof(T)));
            }
        }

        /// <summary>
        /// A generic function that gets entity of type of T and remove it 
        /// from the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Remove<T>(T item) where T : IDalDo
        {

            if (DoesExistInList(item))
            {

                #region
                //if (DoesExistInList(item))
                //{
                //    //DataSource.Data[typeof(T)].Remove(item);
                //    XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
                //    XElement root = document.Root;
                //    foreach (var obj in root.Elements())
                //    {
                //        if (int.Parse(obj.Element("Id").Value) ==item)
                //            obj.Element(propertyName).Value = (string)newValue;
                //    }
                //    document.Save(GetXmlFilePath(typeof(T)));
                #endregion
                if (typeof(T) == typeof(Drone))
                {
                    var droneList = XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone))).ToList();
                    droneList.Remove((Drone)Convert.ChangeType(item, typeof(Drone)));
                    XMLTools.SaveDroneListToXmlWithXElement(droneList, GetXmlFilePath(typeof(T)));
                    //list.Remove();
                }
                else
                {
                    var list = XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
                    list.Remove(item);
                    XMLTools.SaveListToXmlSerializer(list, GetXmlFilePath(typeof(T)));
                }
            }
        }

        /// <summary>
        /// A function that load the config file and returns the index of the Parcel.
        /// </summary>
        /// <returns> returns the running number.</returns>
        public int GetIndexParcel()
        {
            XElement root = XDocument.Load(ConfigFilePath).Root;
            int index = int.Parse(root.Element("IndexParcel").Value);
            root.Element("IndexParcel").SetValue(index + 1);
            root.Save(ConfigFilePath);
            return ++index;
            #region
            //return ++DataSource.Config.IndexParcel;
            //return XDocument.Load(configFilePath).Root;
            #endregion
        }

        /// <summary>
        /// A function that load the config file and returns the powerConsumption.
        /// </summary>
        /// <returns>returns the powerConsumption.</returns>
        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            XElement root = XDocument.Load(ConfigFilePath).Root;
            double Available = double.Parse(root.Element("Available").Value);
            double LightWeight = double.Parse(root.Element("LightWeight").Value);
            double MediumWeight = double.Parse(root.Element("MediumWeight").Value);
            double HeavyWeight = double.Parse(root.Element("HeavyWeight").Value);
            double ChargingRate = double.Parse(root.Element("ChargingRate").Value);

            root.Save(ConfigFilePath);
            return (Available,
                    LightWeight,
                    MediumWeight,
                    HeavyWeight,
                    ChargingRate);
            #region
            //return (Available, LightWeight, MediumWeight, HeavyWeight, ChargingRate);


            //XDocument document = XDocument.Load(configFilePath);
            //XElement root = document.Root;
            //foreach (var obj in root.Elements())
            //{

            //}
            //document.Save(GetXmlFilePath(typeof(T)));
            #endregion
        }

        /// <summary>
        /// A generic function that gets entity of type of T and returns if it exists in the list of type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>returns true if it exists in the list of type of T else false.</returns>
        private bool DoesExistInList<T>(T item) where T : IDalDo
        {
            if (typeof(T) == typeof(Drone))
                return XMLTools.LoadDroneListFromXmlWithXElement(GetXmlFilePath(typeof(DO.Drone)))
                    .ToList().Any(i => i.Equals(item));
            else
            {
                StreamReader reader = new(GetXmlFilePath(typeof(T)));
                XmlSerializer serializer = new(typeof(List<T>));
                List<T> List = (List<T>)serializer.Deserialize(reader);
                reader.Close();
                return List.Any(i => i.Equals(item));
            }

        }
    }
}
