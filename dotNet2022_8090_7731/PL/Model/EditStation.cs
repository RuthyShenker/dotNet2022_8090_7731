using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    public class EditStation : ObservableBase, IDataErrorInfo
    {
        public int Id { get; init; }

        public Location Location { get; init; }

        public IEnumerable<ChargingDrone> ListChargingDrone { get; set; }

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                Set(ref _name, value);
                validityMessages["Name"] = value is null or "" ? "" :
                                                                StringMessage(value);
            }
        }

        private int? _numPositions;

        public object NumPositions
        {
            get => _numPositions == null ? null : _numPositions;
            set
            {
                if (value is null or "")
                {
                    Set(ref _numPositions, null);
                }
                else if (int.TryParse(value.ToString(), out int id))
                {
                    Set(ref _numPositions, id);
                }
                validityMessages["NumPositions"] = IntMessage(value);
            }
        }

        private Dictionary<string, string> validityMessages = new Dictionary<string, string>()
        {
            ["Name"] = "",
            ["NumPositions"] = "",
        };

        // --------------IDataErrorInfo---------------------
        public string Error
        {
            get
            {
                return (_name is null or "" && _numPositions == null) ? string.Empty : "Invalid input";
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
    }
}
