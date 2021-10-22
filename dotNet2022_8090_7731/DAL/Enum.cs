using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public enum WeightCategories { Light, Medium, Heavy};
        public enum UrgencyStatuses { Normal, Fast, Emergency };
        public enum DroneStatuses { Available, Maintenance, Delivery };
        public enum ProgramOptions {Add=1,Update,DisplaySpecific, DisplayList,Exit };

    };
}
