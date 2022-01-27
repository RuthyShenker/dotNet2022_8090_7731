using DO;
using Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    internal sealed partial class  DalXml : Singleton<DalXml>, DalApi.IDal
    {
        readonly string xmlFilesLocation;

        private DalXml()
        {
            xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\..\..\XmlFiles";
        }

        static DalXml()
        {

        }


        string GetXmlFilePath(Type type) => $@"{xmlFilesLocation}\{type.Name}List.xml";
        string configFilePath => $@"{xmlFilesLocation}\Config.xml";

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable, IDalObject
        {
            try
            {
                XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
                XElement root = document.Root;
                XElement e = (from element in root.Elements()
                              where int.Parse(element.Element("Id").Value) == Id
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
           

            StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            IEnumerable<T> List = (IEnumerable<T>)serializer.Deserialize(reader);
            reader.Close();
            return List.First(obj => obj.Id == Id);
            //}
            //catch
            //{
            //    //return false;
            //}
        }



        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            //return ((List<T>)DataSource.Data[typeof(T)]).FirstOrDefault(item => predicate(item));
            StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> List = (List<T>)serializer.Deserialize(reader);
            reader.Close();
            return List.FirstOrDefault(item => predicate(item));

        }

        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IDalObject
        {
            /*return ((List<T>)DataSource.Data[typeof(T)]).FindAll(predicate).ToList();*/
            StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> List = (List<T>)serializer.Deserialize(reader);
            reader.Close();
            return List.FindAll(predicate).ToList();
        }

        public IEnumerable<T> GetListFromDal<T>() where T : IDalObject
        {
            //return ((List<T>)DataSource.Data[typeof(T)]).ToList();
            //using (FileStream fs = new FileStream(GetXmlFilePath(typeof(T)), FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T))))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    //var o = serializer.Deserialize(reader);

                    return (List<T>)serializer.Deserialize(reader);
                    //ToDo:
                }
            }
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalObject
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}
        public void Add<T>(T item) where T : IDalObject
        {
            //Type type = typeof(T);
            //if (DoesExistInList(item))
            //{
            //    throw new InValidActionException($" This item already exists in list {type}");
            //}
            //((List<T>)DataSource.Data[typeof(T)]).Add(item);
            StreamWriter writer = new StreamWriter(GetXmlFilePath(typeof(T)));
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, item);
            writer.Close();
        }

        public void Update<T>(int id, object newValue = null, string propertyName = null) where T : IIdentifiable, IDalObject
        {
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

            try
            {
                XDocument document = XDocument.Load(GetXmlFilePath(typeof(T)));
                XElement root = document.Root;

                XElement element = root.Elements().Single(obj =>
                 int.Parse(obj.Element("Id").Value) == id);
                element.Element(propertyName).RemoveAttributes();
                element.SetElementValue(propertyName, newValue);
                document.Save(GetXmlFilePath(typeof(T)));
            }
            catch
            {

                // return false;
            }
        }

        public void Remove<T>(T item) where T : IDalObject
        {
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
            //}

        }

        private bool DoesExistInList<T>(T item) where T : IDalObject
        {
            //return ((List<T>)DataSource.Data[typeof(T)]).Any(i => i.Equals(item));
            StreamReader reader = new StreamReader(GetXmlFilePath(typeof(T)));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> List = (List<T>)serializer.Deserialize(reader);
            //ToDo:
            reader.Close();
            return List.Any(i => i.Equals(item));
        }

        public int GetIndexParcel()
        {
            XDocument document = XDocument.Load(configFilePath);
            int index = int.Parse(document.Root.Element("IndexParcel").Value);
            document.Root.Element("IndexParcel").SetValue(index + 1);
            document.Save(configFilePath);
            return ++index;
            //return ++DataSource.Config.IndexParcel;
            //return XDocument.Load(configFilePath).Root;
        }

        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            XDocument document = XDocument.Load(configFilePath);
            double Available = double.Parse(document.Root.Element("Available").Value);
            double LightWeight = double.Parse(document.Root.Element("LightWeight").Value);
            double MediumWeight = double.Parse(document.Root.Element("MediumWeight").Value);
            double HeavyWeight = double.Parse(document.Root.Element("HeavyWeight").Value);
            double ChargingRate = double.Parse(document.Root.Element("ChargingRate").Value);

            document.Save(configFilePath);
            return (Available,
                    LightWeight,
                    MediumWeight,
                    HeavyWeight,
                    ChargingRate);

            //return (Available, LightWeight, MediumWeight, HeavyWeight, ChargingRate);


            //XDocument document = XDocument.Load(configFilePath);
            //XElement root = document.Root;
            //foreach (var obj in root.Elements())
            //{

            //}
            //document.Save(GetXmlFilePath(typeof(T)));
        }
    }
}

