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
        public EditStation(int id, string name, int numAvailablePositions, Location location,
            IEnumerable<BO.ChargingDrone> listChargingDrone)
        {
            Id = id;
            Name = name;
            NumAvailablePositions = numAvailablePositions;
            Location = location;
            ListChargingDrone = listChargingDrone;
        }

        public int Id { get; init; }

        public Location Location { get; init; }

        public IEnumerable<BO.ChargingDrone> ListChargingDrone { get; set; }

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                Set(ref _name, value);
                validityMessages["Name"] = StringMessage(value);
            }
        }

        private int? _availablePositions;

        public object NumAvailablePositions
        {
            get => _availablePositions == null ? null : _availablePositions;
            set
            {
                if (value is null or "")
                {
                    Set(ref _availablePositions, null);
                }
                else if (int.TryParse(value.ToString(), out int id))
                {
                    Set(ref _availablePositions, Convert.ToInt32(value));
                    validityMessages["NumAvailablePositions"] = IntMessage(_availablePositions);
                }
                else
                {
                    validityMessages["NumAvailablePositions"] = IntMessage("invalid input");
                }
                
            }
        }

        // --------------IDataErrorInfo---------------------
        public string Error
        {
            get
            {
                return validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (validityMessages.ContainsKey(columnName))
                    return validityMessages[columnName];
                return string.Empty;
            }
        }

        private Dictionary<string, string> validityMessages = new Dictionary<string, string>()
        {
            ["Name"] = "",
            ["NumAvailablePositions"] = "",
        };
    }
}
