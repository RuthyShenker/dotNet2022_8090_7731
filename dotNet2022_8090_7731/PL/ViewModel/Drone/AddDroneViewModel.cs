﻿using BO;
using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class AddDroneViewModel
    {
        public DroneToAdd Drone { get; set; }
        //BlApi.IBL bl;
        Action<BO.Drone> switchView;
        //Action refreshDrones;
        public Array WeightOptions { get; set; } = System.Enum.GetValues(typeof(BO.WeightCategories));
        public List<int> StationOptions { get; set; }
        public RelayCommand<object> AddDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public AddDroneViewModel(Action<BO.Drone> switchView /*BlApi.IBL bl,*//* Action refreshDrones*/)
        {
            Drone = new();
            this.switchView = switchView;
            //this.bl = bl;
            //this.refreshDrones = refreshDrones;
            StationOptions = BlApi.BlFactory.GetBl().AvailableSlots().Select(station => station.Id).ToList();
            AddDroneCommand = new RelayCommand<object>(AddDrone, param => Drone.Error == "");
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        private void AddDrone(object parameters)
        {
            try
            {
                MessageBox.Show("AddCustomer Drone");
                var blDrone = Map(Drone);
                BlApi.BlFactory.GetBl().AddingDrone(blDrone, Drone.StationId);
                Refresh.Invoke();
                switchView(blDrone);
            }
            catch (Exception)
            {
                MessageBox.Show("id is already exist");
            }
        }

        private Drone Map(DroneToAdd drone)
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
