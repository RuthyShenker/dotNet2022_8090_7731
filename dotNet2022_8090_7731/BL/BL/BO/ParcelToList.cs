using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
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
        /// <summary>
        /// A constructor of ParcelToList with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senderName"></param>
        /// <param name="getterName"></param>
        /// <param name="weight"></param>
        /// <param name="myPriority"></param>
        /// <param name="status"></param>
        public ParcelToList(int id,string senderName, string getterName,
           WeightCategories weight, Priority myPriority, ParcelStatus status)
        {
            Id = id;
            SenderName = senderName;
            GetterName = getterName;
            Weight = weight;
            MyPriority = myPriority;
            Status = status;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string SenderName { get; set; }
        public string GetterName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MyPriority { get; set; }
        public ParcelStatus Status { get; set; }
    }
}
