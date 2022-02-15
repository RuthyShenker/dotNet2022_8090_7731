using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DalXml
{
    public class XMLTools
    {
        //private static readonly string dirPath = @"xml\";
        static XMLTools()
        {
            //if (!Directory.Exists(dirPath))
            //    Directory.CreateDirectory(dirPath);
        }
        //#region SaveLoadWithXElement
        //public static void SaveListToXmlElement(XElement rootElem, string filePath)
        //{
        //    try
        //    {
        //        rootElem.Save(filePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
        //        throw;
        //    }
        //}

        //public static XElement LoadListFromXmlElement(string filePath)
        //{
        //    try
        //    {
        //        if (File.Exists(filePath))
        //        {
        //            return XElement.Load(dirPath + filePath);
        //        }
        //        else
        //        {
        //            XElement rootElem = new XElement(dirPath + filePath);
        //            rootElem.Save(dirPath + filePath);
        //            return rootElem;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
        //        throw;
        //    }
        //}
        //#endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
        {
            try
            {
                using (FileStream file = new(filePath, FileMode.Create))
                {
                    XmlSerializer x = new(list.GetType());
                    x.Serialize(file, list);
                }
            }
            catch (Exception ex)
            {
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
                throw;
            }
        }
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using var reader = new StreamReader(filePath);
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    return (List<T>)x.Deserialize(reader);
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                throw;
            }
        }
        #endregion
        public static void SaveDroneListToXml(IEnumerable<DO.Drone> list, string filePath)
        {
            XElement Drones = new XElement("ArrayOfDrones",
                                            from drone in list
                                            select new XElement("Drone",
                                                        new XElement("Id", drone.Id),
                                                        new XElement("MaxWeight", drone.MaxWeight),
                                                        new XElement("Model", drone.Model)
                                                        )
                                            );
            Drones.Save(filePath);
        }

       
        public static IEnumerable<DO.Drone> LoadDroneListFromXmlToDrone()
        {
            return
                from drone in XElement.Load(GetXmlFilePath(typeof(DO.Drone))).Elements()
                select new DO.Drone()
                {
                    Id = int.Parse(drone.Element("Id").Value),
                    MaxWeight = (DO.WeightCategories)Enum.Parse(typeof(DO.WeightCategories), drone.Element("MaxWeight").Value),
                    Model = drone.Element("Model").Value
                }; 
        }

        private static String GetXmlFilePath(Type type)
        {
            var xmlFilesLocation= $@"{Directory.GetCurrentDirectory()}\..\..\XmlFiles";
            return $@"{xmlFilesLocation}\{type.Name}List.xml";
        }
        //private DO.Drone ConvertFromXmlToDrone/*<T>*/(XElement element) /*where T : new()*/
        //{

        //}
    }
    //public static class XmlDrone
    //{
     
    //}

}

