using BO;
using PL.Drones;
using PL.View;
using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //BlApi.IBL bl;
        //public RelayCommand<object> CustomerListViewCommand;
        //public RelayCommand<object> DroneListViewCommand;
        //public RelayCommand<object> StationListViewCommand;
        //public RelayCommand<object> ParcelListViewCommand;
        readonly string xmlFilesLocation;
        
  string GetXmlFilePath(Type type) => $@"{xmlFilesLocation}\{type.Name}List.xml";
        string configFilePath => $@"{xmlFilesLocation}\Config.xml";

        public MainWindow()
        {
            InitializeComponent();
            xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\XmlFiles";
            List<Drone> list = new List<Drone>();
            List<Customer> list1 = new List<Customer>();
            List<Station> list2 = new List<Station>();
            List<Parcel> list3 = new List<Parcel>();
            //List<Drone> list = new List<Drone>();
            //List<T> list1 = new List<T>();
            //list.Add(item);
            XMLTools.SaveListToXmlSerializer<Drone>(list, GetXmlFilePath(typeof(Drone)));
            XMLTools.SaveListToXmlSerializer<Customer>(list1, GetXmlFilePath(typeof(Customer)));
            XMLTools.SaveListToXmlSerializer<Station>(list2, GetXmlFilePath(typeof(Station)));
            XMLTools.SaveListToXmlSerializer<Parcel>(list3, GetXmlFilePath(typeof(Parcel)));
            this.DataContext = new MainWindowViewModel();
            //mDroneView.DataContext = new DisplayViewModel();

            //CustomerListViewCommand = new RelayCommand<object>(ShowCustomerListView);
            //DroneListViewCommand = new RelayCommand<object>(ShowDroneListView);
            //StationListViewCommand = new RelayCommand<object>(ShowStationListView);
            //ParcelListViewCommand = new RelayCommand<object>(ShowParcelListView);

        }

        //private void ShowParcelListView(object obj)
        //{
        //    new ParcelListView(new ParcelListViewModel(bl)).Show();
        //}

        //private void ShowStationListView(object obj)
        //{
        //    new Station().Show();
        //}

        //public void ShowDroneListView(object obj)
        //{
        //    new DroneListView(new DroneListViewModel(bl)).Show();
        //}

        //private void ShowCustomerListView(object obj)
        //{
        //    new CustomerListView(new CustomerListViewModel(bl)).Show();
        //}
        public class XMLTools
        {
            private static readonly string dirPath = @"xml\";
            static XMLTools()
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
            }
            #region SaveLoadWithXElement
            public static void SaveListToXmlElement(XElement rootElem, string filePath)
            {
                try
                {
                    rootElem.Save(dirPath + filePath);
                }
                catch (Exception ex)
                {
                    // throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
                    throw;
                }
            }

            public static XElement LoadListFromXmlElement(string filePath)
            {
                try
                {
                    if (File.Exists(dirPath + filePath))
                    {
                        return XElement.Load(dirPath + filePath);
                    }
                    else
                    {
                        XElement rootElem = new XElement(dirPath + filePath);
                        rootElem.Save(dirPath + filePath);
                        return rootElem;
                    }
                }
                catch (Exception ex)
                {
                    //throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                    throw;
                }
            }
            #endregion

            #region SaveLoadWithXMLSerializer
            public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
            {
                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    XmlSerializer x = new XmlSerializer(list.GetType());

                    x.Serialize(file, list);
                    file.Close();
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
                        List<T> list;
                        XmlSerializer x = new XmlSerializer(typeof(List<T>));
                        FileStream file = new FileStream(dirPath + filePath, FileMode.Open);
                        list = (List<T>)x.Deserialize(file);
                        file.Close();
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
        }
    }
}
