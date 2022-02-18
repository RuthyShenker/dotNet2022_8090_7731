using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    /// <summary>
    /// A public class CustomerInParcel impliments:ObservableBase
    /// contains:
    /// Id
    /// Name
    /// </summary>
    public class CustomerInParcel : ObservableBase
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                Set(ref id, value);
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                Set(ref name, value);
                StringMessage(value);
            }
        }
    }
}
