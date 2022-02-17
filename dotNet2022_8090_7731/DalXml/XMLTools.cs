﻿using System;
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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
        {
            try
            {
                FileStream file = new(filePath, FileMode.Create);
                XmlSerializer x = new(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    
                        var reader = new StreamReader(filePath);
                        XmlSerializer x = new(typeof(List<T>));
                        var list = (List<T>)x.Deserialize(reader);
                        reader.Close();

                        return list;
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

        /// <summary>
        /// A function that write - save Drone list to xml by XElement
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public static void SaveDroneListToXmlWithXElement(IEnumerable<DO.Drone> list, string filePath)
        {
            XElement Drones = new("ArrayOfDrones",
                                            from drone in list
                                            select new XElement("Drone",
                                                        new XElement("Id", drone.Id),
                                                        new XElement("MaxWeight", drone.MaxWeight),
                                                        new XElement("Model", drone.Model)
                                                        )
                                            );
            Drones.Save(filePath);
        }

        /// <summary>
        /// <returns> A function that load the drone list from xml file with 
        /// XElement and convert it to type of DO.Drone.</returns>
        /// </summary>
        /// <param name="filePath"></param>
        public static IEnumerable<DO.Drone> LoadDroneListFromXmlWithXElement(string filePath)
        {
            var document = XElement.Load(filePath);
            var list =
                from drone in document.Elements()
                select new DO.Drone()
                {
                    Id = int.Parse(drone.Element("Id").Value),
                    MaxWeight = (DO.WeightCategories)Enum.Parse(typeof(DO.WeightCategories), drone.Element("MaxWeight").Value),
                    Model = drone.Element("Model").Value
                };
            document.Save(filePath);
            return list;
        }
    }
}

