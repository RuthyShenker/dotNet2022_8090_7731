
using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    /// <summary>
    ///  public class AddDroneViewModel
    /// </summary>
    public class AddDroneViewModel
    {
        public DroneToAdd Drone { get; set; }
        //BlApi.IBL bl;
        Action<BO.Drone> switchView;
        //Action refreshDrones;
        public Array WeightOptions { get; set; } = System.Enum.GetValues(typeof(PO.WeightCategories));
        public List<int> StationOptions { get; set; }
        public RelayCommand<object> AddDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        /// <summary>
        /// A constructor that gets action.
        /// </summary>
        /// <param name="switchView"></param>
        public AddDroneViewModel(Action<BO.Drone> switchView )
        {
            Drone = new();
            this.switchView = switchView;
            //this.bl = bl;
            StationOptions = BlApi.BlFactory.GetBl().AvailableSlots().Select(station => station.Id).ToList();
            AddDroneCommand = new RelayCommand<object>(AddDrone, param => Drone.Error == "");
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        /// <summary>
        /// A function that adds drone.
        /// </summary>
        /// <param name="parameters"></param>
        private void AddDrone(object parameters)
        {
            try
            {
                var blDrone = Map(Drone);
                BlApi.BlFactory.GetBl().AddingDrone(blDrone, Drone.StationId);
                Refresh.Invoke();
                MessageBox.Show("The Drone Added Succeesfully!");
                switchView(blDrone);
            }
            catch (BO.IdAlreadyExistsException)
            {
                MessageBox.Show("id is already exist");
            }
            catch (BO.InValidActionException)
            {

            }
            catch (BO.IdDoesNotExistException)
            {

            }
            catch (BO.XMLFileLoadCreateException)
            {

            }
        }

        /// <summary>
        /// A function that converts DroneToAdd to BO.Drone
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private BO.Drone Map(DroneToAdd drone)
        {
            return new()
            {
                Id = (int)drone.Id,
                Model = drone.Model,
                Weight = (BO.WeightCategories)drone.MaxWeight,
                BatteryStatus = 0,
                DroneStatus = (BO.DroneStatus)PO.DroneStatus.Maintenance,
                PInTransfer = null,
                CurrLocation = BlApi.BlFactory.GetBl().GetStation(Drone.StationId).Location
            };
        }
    }
}
