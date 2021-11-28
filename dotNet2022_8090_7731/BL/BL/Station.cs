﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    // <summary>
    ///A class of Station that contains:
    ///Id
    ///Name Station
    ///Station Location
    ///Num Available Positions
    ///List_BL_ChargingDrone
    /// </summary>
    public class Station
    {
        public Station(int id, string nameStation, Location sLocation, int numAvailablePositions, List<ChargingDrone> lBL_ChargingDrone)
        {
            Id = id;
            NameStation = nameStation;
            SLocation = sLocation;
            NumAvailablePositions = numAvailablePositions;
            LBL_ChargingDrone = lBL_ChargingDrone;
        }
        public int Id { get; init; }
        public string NameStation { get; set; }
        public Location SLocation{ get; set; }
        public int NumAvailablePositions { get; set; }
        //  רשימת רחפנם בטעינה
        public List<ChargingDrone> LBL_ChargingDrone { get; set; }
    }
}
