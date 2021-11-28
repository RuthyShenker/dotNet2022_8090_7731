using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class MEnum
    {
        public enum ProgramOptions { Adding = 1, 
            Updating, 
            DisplayingItem, 
            DisplayingList, 
            Exit 
        };

        public enum Adding { BaseStation = 1, 
            Drone, 
            Customer, 
            Parcel
        };

        public enum Updating { DroneDetails = 1,
            StationDetails,
            CustomerDetails, 
            SendingDroneToChargingPosition,
            RealesingDroneFromChargingPosition,
            BelongingParcelToDrone,
            PickingParcelByDrone,
            DeliveryParcelToDestination
        };

        public enum DisplayingItem { BaseStation = 1, 
            Drone, 
            Customer, 
            Parcel 
        };

        public enum DisplayingList { BaseStation = 1,
            Drone,
            Customer,
            Parcel,
            PackageWhichArentBelongToDrone,
            StationsWithAvailablePositions 
        };
    }
}
