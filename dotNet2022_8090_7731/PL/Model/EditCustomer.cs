using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    /// <summary>
    /// A public class EditCustomer impliments : ObservableBase, IDataErrorInfo
    /// includes:
    /// Id
    /// Name
    /// Phone
    /// Location
    /// LFromCustomer
    /// LForCustomer
    /// </summary>
    public class EditCustomer : ObservableBase, IDataErrorInfo
    {

        public int Id { get; init; }

        private string _name;
        public string Name
        {
            get => _name;

            set
            {
                Set(ref _name, value);
                validityMessages[nameof(Name)] = NameMessage(value);
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;

            set
            {
                Set(ref _phone, value);
                validityMessages[nameof(Phone)] = PhoneMessage(value,10);
            }
        }

       // public Location Location { get; set; }
        private Location location;
        public Location Location
        {
            get { return location; }
            set { location = value; }
        }


        /// <summary>
        /// A list of parcels from this customer:
        /// </summary>
        public IEnumerable<ParcelInCustomer> LFromCustomer { get; set; }

        /// <summary>
        /// A list of parcels for this customer:
        /// </summary>
        public IEnumerable<ParcelInCustomer> LForCustomer { get; set; }

        // --------------IDataErrorInfo---------------------
        public string Error => validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";

        public string this[string propertyName]
        {
            get
            {
                if (validityMessages.ContainsKey(propertyName))
                    return validityMessages[propertyName];
                return string.Empty;
            }
        }

        /// <summary>
        /// A dictionary of validation.
        /// </summary>
        private readonly Dictionary<string, string> validityMessages = new()
        {
            [nameof(Name)] = string.Empty,
            [nameof(Phone)] = string.Empty,
        };
    }
}
