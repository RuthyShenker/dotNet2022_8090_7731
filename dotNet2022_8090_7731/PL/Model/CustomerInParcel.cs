using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    public class CustomerInParcel : ObservableBase
    {
        public CustomerInParcel()
        {

        }
        public CustomerInParcel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                Set(ref id, value);

                //SetMessage();
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
