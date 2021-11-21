﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInParcel
    {
        public int Id{ get; set; }
        public float BatteryStatus{ get; set; }
        public Location CurrLocation { get; set; }
        
        public DroneInParcel (int id, float batteryStatus, Location location)
	    {
            Id=id;
            BatteryStatus = batteryStatus;
            CurrLocation = location;
	    }
    }

}
