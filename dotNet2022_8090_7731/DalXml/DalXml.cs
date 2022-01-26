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
    public sealed class DalXml : Singleton<DalXml>, DalApi.IDal
    {
        readonly string xmlFilesLocation;
        private DalXml()
        {
            xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\..\..\XmlFiles";
            InitializeFiles();
        }

        private void InitializeFiles()
        {
            InitializeDrones();
            InitializeBaseStations();
            InitializeCustomers();
            InitializeParcels();
        }

        private void InitializeParcels()
        {
            throw new NotImplementedException();
        }

        private void InitializeCustomers()
        {
            throw new NotImplementedException();
        }

        private void InitializeBaseStations()
        {
            throw new NotImplementedException();
        }
        internal static Random Rand;
        const int INITIALIZE_DRONE = 5;
        const int INITIALIZE_CUSTOMER = 10;
        const int INITIALIZE_BASE_STATION = 2;
        const int INITIALIZE_PARCEL = 5;

        private void InitializeDrones()
        {
            StreamWriter writer = new StreamWriter(GetXmlFilePath(typeof(Drone)));
            XmlSerializer serializer = new XmlSerializer(typeof(Drone));
            for (int i = 0; i < INITIALIZE_DRONE; ++i)
            {
                serializer.Serialize(writer,  new Drone()
                {
                    Id = Rand.Next(100000000, 1000000000),
                    Model = Rand.Next(1000, 10000).ToString(),
                    MaxWeight = (WeightCategories)Rand.Next(0, Enum.GetNames(typeof(WeightCategories)).Length),
                });
            }
            writer.Close();
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
                foreach (var obj in root.Elements())
                {
                    if (int.Parse(obj.Element("Id").Value) == id)
                        obj.Element(propertyName).Value = (string)newValue;
                }
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
            return 0;
            //return ++DataSource.Config.IndexParcel;
            //return XDocument.Load(configFilePath).Root;
        }

        public (double, double, double, double, double) PowerConsumptionRequest()
        {
            return (0, 0, 0, 0, 0);
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

