﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
{
    /// <summary>
    ///A class of Drone contains:
    ///Id
    ///Model
    ///Weight
    ///BatteryStatus
    ///DroneStatus
    ///ParcelInTransfer
    ///CurrLocation
    /// </summary>
    public class Drone
    {
         public Drone(int id, string model,WeightCategories weight, double batteryStatus, DroneStatus droneStatus, ParcelInTransfer pInTransfer, Location currLocation)
        {
            Id = id;
            Model = model;
            Weight = weight;
            BatteryStatus = batteryStatus;
            DroneStatus = droneStatus;
            PInTransfer = pInTransfer;
            CurrLocation = currLocation;
        }
        public int Id { get; init; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double BatteryStatus { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public ParcelInTransfer PInTransfer { get; set; }
        public Location CurrLocation { get; set; } 
       
    }
}
