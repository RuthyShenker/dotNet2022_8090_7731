
using BO;
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
        BlApi.IBL bl;
        Action refreshDroneList, closeWindow;

        public AddNewDroneView(BlApi.IBL bl, Action refreshDroneList, Action closeWindow)
        {
            InitializeComponent();

            this.bl = bl;
            this.refreshDroneList = refreshDroneList;
            this.closeWindow = closeWindow;

            AddNewDronePanel.DataContext = new Drone();
            StationIdComboBox.DataContext = bl.AvailableSlots().Select(slot => slot.Id);
            //ChangeVisibility(Visibility.Collapsed, BatteryContainer, StatusContainer, Location);
        }

        private void Button_Click_Ok_Adding_New_Drone(object sender, RoutedEventArgs e)
        {
            try
            {
                var stationId = (int)StationIdComboBox.SelectedValue;
                var drone = (Drone)AddNewDronePanel.DataContext;

                bl.AddingDrone(drone, stationId);
                refreshDroneList();

                if (MessageBox.Show($"Succesfuly Adding!",
                   "New drone",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    closeWindow();
                }
            }
            catch (IdIsAlreadyExistException)
            {
                //MessageBox.Show(exception.Message, " Id is not available ", MessageBoxButton.OK, MessageBoxImage.Error);
                var textBox = (TextBox)AddNewDronePanel.FindName("IdAddingTextBox");
                InternalClass.AddToolTip(textBox, " Id is not available ");
                textBox.Background = Brushes.SteelBlue;
            }
        }

        void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            InternalClass.TextBox_OnPreviewTextInputt(sender, e);
        }

        private void Close_Window_Click(object sender, RoutedEventArgs e)
        {
            closeWindow();
        }

        //private bool CheckValidDrone(Drone drone, Button sender)
        //{

        //}
    }
}
