using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ParcelToAdd : ObservableBase, IDataErrorInfo
    {
        public string this[string columnName] => "";

        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }

        public CustomerInParcel Sender { get; set; } = new();
        public CustomerInParcel Getter { get; set; } = new();

        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }


        //public void SetMessage()
        //{
        //    if (sender.Id == getter.Id)
        //    {
        //        validityMessages["SenderId"] = "Sender Id can not be worth to Getter Id";
        //        validityMessages["GetterId"] = "Sender Id can not be worth to Getter Id";
        //    }
        //    else
        //    {
        //        validityMessages["SenderId"] = string.Empty;
        //        validityMessages["GetterId"] = string.Empty;
        //    }
        //}



        // --------------IDataErrorInfo---------------------
        public string Error => Sender.Id != 0 && Getter.Id != 0 && Sender.Id != Getter.Id ? string.Empty : "Invalid input";

        //public string this[string columnName]
        //{
        //    //get
        //    //{
        //    //    if (validityMessages.ContainsKey(columnName))
        //    //        return validityMessages[columnName];
        //    //    return string.Empty;
        //    //}
        //}

        //private Dictionary<string, string> validityMessages = new()
        //{
        //    ["SenderId"] = " ",
        //    ["GetterId"] = " ",
        //};
    }
}
        //public DroneInParcel DInParcel { get; set; }
        //public DateTime MakingParcel { get; set; }
        //public DateTime? BelongParcel { get; set; }
        //public DateTime? PickingUp { get; set; }
        //public DateTime? Arrival { get; set; }
        /// <summary>
        /// A constructor of Parcel with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sender"></param>
        /// <param name="getter"></param>
        /// <param name="parcelWeight"></param>
        /// <param name="parcelMPriority"></param>
        /// <param name="dInParcel"></param>
        /// <param name="parcelMakingParcel"></param>
        /// <param name="parcelBelongParcel"></param>
        /// <param name="parcelPickingUp"></param>
        /// <param name="parcelArrival"></param>
        //public ParcelToAdd(int id, CustomerInParcel sender, CustomerInParcel getter, WeightCategories parcelWeight,
        //    Priority parcelMPriority, DroneInParcel dInParcel, DateTime parcelMakingParcel,
        //       DateTime? parcelBelongParcel, DateTime? parcelPickingUp, DateTime? parcelArrival)
        //{
        //    Id = id;
        //    Sender = sender;
        //    Getter = getter;
        //    //Arrival = parcelArrival;
        //    //PickingUp = parcelPickingUp;
        //    //BelongParcel = parcelBelongParcel;
        //    Weight = parcelWeight;
        //    MPriority = parcelMPriority;
        //    DInParcel = dInParcel;
        //    //MakingParcel = parcelMakingParcel;
        //}