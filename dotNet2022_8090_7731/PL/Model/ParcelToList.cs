using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    ///A class of ParcelToList that contains:
    ///Id
    ///SenderName
    ///GetterName
    ///Weight
    ///MyPriority
    ///Status
    /// </summary>
    public class ParcelToList
    {
        public ParcelToList(BO.ParcelToList parcel)
        {
            Id = parcel.Id;
            SenderName = parcel.SenderName;
            GetterName = parcel.GetterName;
            Weight = (PO.WeightCategories)parcel.Weight;
            MyPriority = (PO.Priority)parcel.MyPriority;
            Status = (PO.ParcelStatus)parcel.Status;
        }

        public int Id { get; init; }
        public string SenderName { get; set; }
        public string GetterName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MyPriority { get; set; }
        public ParcelStatus Status { get; set; }
    }
}
