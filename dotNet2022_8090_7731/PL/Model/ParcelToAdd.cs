
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


       


        // --------------IDataErrorInfo---------------------
        public string Error => Sender.Id != 0 && Getter.Id != 0 && Sender.Id != Getter.Id ? string.Empty : "Invalid input";

        //to ask ruti:?????

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        if (validityMessages.ContainsKey(columnName))
        //            return validityMessages[columnName];
        //        return string.Empty;
        //    }
        //}

        //private Dictionary<string, string> validityMessages = new()
        //{
        //    [nameof(Sender)] =string.Empty,
        //    [nameof(Getter)] =string.Empty,
        //};
    }
}
