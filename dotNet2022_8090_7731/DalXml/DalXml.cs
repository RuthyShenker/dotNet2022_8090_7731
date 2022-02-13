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
    internal sealed partial class DalXml : Singleton<DalXml>, DalApi.IDal
    {
        readonly string xmlFilesLocation;
        string GetXmlFilePath(Type type) => $@"{xmlFilesLocation}\{type.Name}List.xml";
        string configFilePath => $@"{xmlFilesLocation}\Config.xml";

        //static DalXml()
        //{
        //

        private DalXml()
        {
            xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\..\..\XmlFiles";
            #region
            //List<Drone> list = new List<Drone>();
            //List<Customer> list1 = new List<Customer>();
            //List<BaseStation> list2 = new List<BaseStation>();
            //List<Parcel> list3 = new List<Parcel>();
            //List<Drone> list = new List<Drone>();
            //List<T> list1 = new List<T>();
            //list.Add(item);
            //XMLTools.SaveListToXmlSerializer<Drone>(list, GetXmlFilePath(typeof(Drone)));
            //XMLTools.SaveListToXmlSerializer<Customer>(list1, GetXmlFilePath(typeof(Customer)));
            //XMLTools.SaveListToXmlSerializer<BaseStation>(list2, GetXmlFilePath(typeof(BaseStation)));
            //XMLTools.SaveListToXmlSerializer<Parcel>(list3, GetXmlFilePath(typeof(Parcel)));
            #endregion
        }

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                return XMLTools.LoadDroneListFromXmlToDrone().Any(item => item.Id == Id);
            }
            else
            {
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                .Any(item => item.Id == Id);
            }
        }

        public T GetFromDalById<T>(int Id) where T : IDalObject, IIdentifiable
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                var drone = XMLTools.LoadDroneListFromXmlToDrone().First(item => item.Id == Id);
                return (T)Convert.ChangeType(drone, typeof(T));
            }
            else
            {
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                .First(obj => obj.Id == Id);
            }
        }

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            if (typeof(T) == typeof(DO.Drone))
            {
                var drone = XMLTools.LoadDroneListFromXmlToDrone().FirstOrDefault(item =>
                            predicate((T)Convert.ChangeType(item, typeof(T))));
                return (T)Convert.ChangeType(drone, typeof(T));
            }
            else
            {
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                .FirstOrDefault(item => predicate(item));
            }

        }

        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            //problem:!!!
            //if (typeof(T) == typeof(DO.Drone))
            //    return LoadDroneListFromXmlToDrone().ToList().FindAll(predicate);
            //else
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)))
                  .FindAll(predicate);
        }

        public IEnumerable<T> GetListFromDal<T>() where T : IDalObject
        {
            if (typeof(T) == typeof(DO.Drone)) 
                return (IEnumerable<T>)XMLTools.LoadDroneListFromXmlToDrone();
            else 
                return XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalObject
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}
        public void Add<T>(T item) where T : IDalObject
        {
            #region
            //Type type = typeof(T);
            //if (DoesExistInList(item))
            //{
            //    throw new InValidActionException($" This item already exists in list {type}");
            //}
            //((List<T>)DataSource.Data[typeof(T)]).Add(item);

            //ToDo:


            //StreamWriter writer = new StreamWriter(GetXmlFilePath(typeof(T)));
            //XmlSerializer serializer2 = new XmlSerializer(typeof(List<T>));
            //serializer.Serialize(writer, list);
            //writer.Close();
            //Type type = typeof(T);
            //if (DoesExistInList(item))
            //{

            //}

            //List<T> list1 = new List<T>();
            #endregion
            if (typeof(T) == typeof(DO.Drone))
            {
                var droneList = XMLTools.LoadDroneListFromXmlToDrone().ToList();
                Drone drone = (Drone)Convert.ChangeType(item, typeof(Drone));
                droneList.Add(drone);
                XMLTools.SaveDroneListToXml(droneList,GetXmlFilePath(typeof(Drone)));
            }
            else
            {
                List<T> list = XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
                list.Add(item);
                XMLTools.SaveListToXmlSerializer<T>(list, GetXmlFilePath(typeof(T)));
            }
            #region
            //XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
            //string xmlString = ConvertObjectToXMLString(item);
            //// Save C# class object into Xml file
            //XElement xElement = XElement.Parse(xmlString);
            //document.Root.Elements().Append(xElement);
            //document.Save(GetXmlFilePath(typeof(T)));
            #endregion
        }


        //static string ConvertObjectToXMLString(object classObject)
        //{
        //    string xmlString = null;
        //    Type type = classObject.GetType();
        //    XmlSerializer xmlSerializer = new XmlSerializer(type);
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        xmlSerializer.Serialize(memoryStream, classObject);
        //        memoryStream.Position = 0;
        //        xmlString = new StreamReader(memoryStream).ReadToEnd();
        //    }
        //    return xmlString;
        //}


        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject
        {
            #region
            //Type type = typeof(T);

            //var oldItem = GetFromDalById<T>(id);

            //DataSource.Data[type].Remove(oldItem);
            //if (newValue != null)//TODO: //האם צרחך את הבדיקה הזאת?
            //{
            //    //type.GetProperty(propertyName).SetValue(oldItem, newValue);
            //    T obj = oldItem;
            //    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            //    object boxed = obj;
            //    propertyInfo.SetValue(boxed, newValue, null);
            //    obj = (T)boxed;
            //    oldItem = obj;
            //}
            //DataSource.Data[type].Add(oldItem);
            #endregion
            try
            {
                if (typeof(T)==typeof(DO.Drone))
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
                    var oldItem = list.FirstOrDefault(item=>item.Id==id);
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
            catch
            {
                //בעיה:
                // return false;
            }
        }

        public void Remove<T>(T item) where T : IDalObject
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
                var droneList =XMLTools.LoadDroneListFromXmlToDrone().ToList();
                droneList.Remove((Drone)Convert.ChangeType(item, typeof(Drone)));
                XMLTools.SaveDroneListToXml(droneList,GetXmlFilePath(typeof(T)));
                //list.Remove();
            }
            else 
            { 
                var list= XMLTools.LoadListFromXmlSerializer<T>(GetXmlFilePath(typeof(T)));
                list.Remove(item);
                XMLTools.SaveListToXmlSerializer(list, GetXmlFilePath(typeof(T)));
            }
        }

        private bool DoesExistInList<T>(T item) where T : IDalObject
        {
            //return ((List<T>)DataSource.Data[typeof(T)]).Any(i => i.Equals(item));

            if (typeof(T) == typeof(Drone))
                return XMLTools.LoadDroneListFromXmlToDrone().ToList().Any(i => i.Equals(item));
            else
            {
                StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                List<T> List = (List<T>)serializer.Deserialize(reader);
                reader.Close();
                return List.Any(i => i.Equals(item));
            }
        }

        public int GetIndexParcel()
        {
            XElement root = XDocument.Load(configFilePath).Root;
            int index = int.Parse(root.Element("IndexParcel").Value);
            root.Element("IndexParcel").SetValue(index + 1);
            root.Save(configFilePath);
            return ++index;
            #region
            //return ++DataSource.Config.IndexParcel;
            //return XDocument.Load(configFilePath).Root;
            #endregion
        }

        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            XElement root = XDocument.Load(configFilePath).Root;
            double Available = double.Parse(root.Element("Available").Value);
            double LightWeight = double.Parse(root.Element("LightWeight").Value);
            double MediumWeight = double.Parse(root.Element("MediumWeight").Value);
            double HeavyWeight = double.Parse(root.Element("HeavyWeight").Value);
            double ChargingRate = double.Parse(root.Element("ChargingRate").Value);

            root.Save(configFilePath);
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
    }
}
