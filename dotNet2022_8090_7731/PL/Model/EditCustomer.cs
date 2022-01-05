using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    public class EditCustomer:ObservableBase,IDataErrorInfo
    {
        public EditCustomer(int id, string name, string phone, double longitude, double latitude,
            IEnumerable<ParcelInCustomer> lFromCustomer,IEnumerable<ParcelInCustomer> lForCustomer)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Location = new Location(longitude,latitude);
            LFromCustomer =lFromCustomer;
            LForCustomer =lForCustomer;
        }

  
        public int Id { get; init; }

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                Set(ref _name, value);
                validityMessages["Name"] = NameMessage(value);
            }
        }
        private string _phone;

        public string Phone
        {
            get => _phone;

            set
            {
                Set(ref _phone, value);
                validityMessages["Phone"] =PhoneMessage(value);
            }
        }
        //TODO
        //NEED TO EB]NABLE EDIT
        public Location Location { get; set; }
        // two lists
        public IEnumerable<ParcelInCustomer> LFromCustomer { get; set; }
        public IEnumerable<ParcelInCustomer> LForCustomer { get; set; }

        // --------------IDataErrorInfo---------------------
        public string Error
        {
            get
            {
                return validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";
            }
        }

        public string this[string propertyName]
        {
            get
            {
                if (validityMessages.ContainsKey(propertyName))
                    return validityMessages[propertyName];
                return string.Empty;
            }
        }

        private Dictionary<string, string> validityMessages = new Dictionary<string, string>()
        {
            ["Name"] = "",
            ["Phone"] = "",
        };
    }
}
