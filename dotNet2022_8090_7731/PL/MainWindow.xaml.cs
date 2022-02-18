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
      
        public MainWindow()
        {
            InitializeComponent();
            #region
            //xmlFilesLocation = $@"{Directory.GetCurrentDirectory()}\XmlFiles";
            //List<Drone> list = new ();
            //List<Customer> list1 =new();
            //List<Station> list2 = new ();
            //List<Parcel> list3 = new();
            //List<Drone> list = new List<Drone>();
            //List<T> list1 = new List<T>();
            //list.Add(item);
            //XMLTools.SaveListToXmlSerializer<Drone>(list, GetXmlFilePath(typeof(Drone)));
            //XMLTools.SaveListToXmlSerializer<Customer>(list1, GetXmlFilePath(typeof(Customer)));
            //XMLTools.SaveListToXmlSerializer<Station>(list2, GetXmlFilePath(typeof(Station)));
            //XMLTools.SaveListToXmlSerializer<Parcel>(list3, GetXmlFilePath(typeof(Parcel)));
            #endregion
            this.DataContext = new MainWindowViewModel();

        }

        private void Closeddd(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in Refresh.workers)
            {
                item.Value.CancelAsync();
            }

        }
    }
}

