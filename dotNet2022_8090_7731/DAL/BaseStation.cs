﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
         public struct BaseStation
         {
            public string Id { get; set; }
            public string NameStation { get; set; }
            public int NumberOfChargingStations { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

         }

    }
}