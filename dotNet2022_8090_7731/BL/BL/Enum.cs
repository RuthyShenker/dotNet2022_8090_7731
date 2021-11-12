using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Enum
    {
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
        /// Available, Maintenance, Delivery.
        /// </summary>
        public enum DroneStatus { Available, Maintenance, Delivery };

        /// <summary>
        /// An Enum of ProgramOptions contains:
        /// Add, Update, DisplaySpecific, DisplayList, Exit.
        /// </summary>
        public enum ProgramOptions { Add = 1, Update, DisplaySpecific, DisplayList, Exit };



        public enum ParcelStatus { made,belonged,collected,InDestination}
    }
}
