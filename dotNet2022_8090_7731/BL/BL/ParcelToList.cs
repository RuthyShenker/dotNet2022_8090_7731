using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;

namespace IBL.BO
{
    class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string GetterName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MyPriority { get; set; }
        public ParcelStatus Status { get; set; }
    }
}
