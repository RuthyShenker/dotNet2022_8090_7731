
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace PL.Drones
{
    /// <summary>
    /// Interaction logic for AddNewDroneView.xaml
    /// </summary>
    public partial class AddNewDroneView : UserControl
    {
        IBL.IBL bl;
        Action refreshDroneList, closeWindow;

        public AddNewDroneView(IBL.IBL bl, Action refreshDroneList, Action closeWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.refreshDroneList = refreshDroneList;
            this.closeWindow = closeWindow;
            AddNewDronePanel.DataContext = new Drone();
            //DeliveryComboBox.DataContext = bl.AvailableSlots();
            StationIdComboBox.DataContext = bl.AvailableSlots().Select(slot => slot.Id);
            //ChangeVisibility(Visibility.Collapsed, BatteryContainer, StatusContainer, Location);
        }
        private void Button_Click_Ok_Adding_New_Drone(object sender, RoutedEventArgs e)
        {
            var stationId = (int)StationIdComboBox.SelectedValue;
            //var a = stationId.GetType().GetProperty("Id").GetValue("")
            var drone = (Drone)AddNewDronePanel.DataContext;

            //var nDrone = new Drone(drone.Id, drone.Model, drone.Weight, DroneStatus.Maintenance);
            bool IsCorrect = CheckValidDrone(drone, (Button)sender);
            if (IsCorrect)
            {
                //var a = bl.GetStations();
                //var b = a.First().Id;
                bl.AddingDrone(drone, stationId);
                refreshDroneList();
                this.closeWindow();
            }
        }
        private bool CheckValidDrone(Drone drone, Button sender)
        {
            //            Id
            //WeightCategoriesEnum
            //DroneStatusEnum
            //DeliveredParcelId

            //רק בדיקות מבחנת פורמט ןלא בדיקות תקינות
            bool isExist = bl.GetDrones().Any(d => d.Id == drone.Id);
            if (isExist)
            {
                var textBox = (TextBox)AddNewDronePanel.FindName("IdTextBox");
                AddToolTip(textBox, " Id is not available ");
                return false;
            }
            return true;
        }
        void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (e.Handled = new Regex("[^0-9]+").IsMatch(e.Text))
            {
                textBox.Background = Brushes.Gray;
                ToolTip toolTip = new ToolTip();
                AddToolTip(textBox, " Input has to contain only digits ");
            }
            else
            {
                textBox.Background = Brushes.White;
            }
        }
        private void AddToolTip(TextBox textBox, string str)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.Content = str;
            toolTip.IsOpen = true;
            textBox.ToolTip = toolTip;

            // Turn off ToolTip
            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };
            timer.Tick += new EventHandler(delegate (object timerSender, EventArgs timerArgs)
            {
                toolTip.IsOpen = false;
                timer = null;
            });
        }

    }
}
