using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class CustomerToAdd : ObservableBase, IDataErrorInfo
    {
        public CustomerToAdd()
        {
            Location = new();
        }

        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
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
            ["Id"] = "",
            ["Name"] = "",
            ["Phone"] = "",
            ["Location"]=""
        };

        

        
    }
}
