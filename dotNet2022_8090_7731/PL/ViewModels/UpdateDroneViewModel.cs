using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PO;
namespace PL
{
    class UpdateDroneViewModel
    {
        DroneToUpdate drone { get; set; } = new();
        RelayCommand<object> ChargeDroneCommand { get; set; }

        RelayCommand<object> AssignParcelToDroneCommand { get; set; }

        RelayCommand<object> UpdateModelOfDroneCommand { get; set; }

        RelayCommand<object> CloseWindowCommand { get; set; }


       


        public UpdateDroneViewModel()
        {
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
            UpdateModelOfDroneCommand = new RelayCommand<object>(Update_Model_Click);
        }
        private void Update_Model_Click(object sender)
        {
            // when we changed bl.GetDrones to return new list 
            // before it changed ldronetolist and in the dal ?why??????????

            if (MessageBox.Show($"Are You Sure You Want To Change The Model Of The Drone With Id:{drone.Id} ?",
                   "Update Model",
                   MessageBoxButton.OKCancel,
                   MessageBoxImage.Question) == MessageBoxResult.OK)
            {

                BlApi.BlFactory.GetBl().UpdatingDroneName(drone.Id, drone.Model);
                MessageBox.Show($"Drone With Id:{drone.Id} Updated successfuly!",
                   $" Model Updated Successly {MessageBoxImage.Information}");
                //RefreshDrone(drone);
            }
        }


        private void Close_Window(object sender)
        {

            Window.GetWindow((DependencyObject)sender).Close();
            
        }
    }
}
