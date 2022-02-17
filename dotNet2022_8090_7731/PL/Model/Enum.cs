using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
   
        public enum GroupBy { Id, SenderName, GetterName }
        public enum GroupOptionsForStationList { All, FullPositions, AvailablePositions }
        public enum GroupByDroneStatus {Id, DStatus }        
    
    /// <summary>
    /// An Enum of WeightCategories contains:
    /// Light, Medium, Heavy.
    /// </summary>
    public enum WeightCategories { Light, Medium, Heavy };

    /// <summary>
    /// An Enum of UrgencyStatuses contains:
    /// Normal, Fast, Emergency.
    /// </summary>
    public enum Priority { Normal, Fast, Emergency };

    /// <summary>
    /// An Enum of DroneStatuses contains:
    /// Free, Maintenance, Delivery.
    /// </summary>
    public enum DroneStatus { Free, Maintenance, Delivery };

    /// <summary>
    /// An Enum of ParcelStatus contains:
    /// made, belonged, collected,InDestination.
    /// </summary>
    public enum ParcelStatus { made, belonged, collected, InDestination }
}

