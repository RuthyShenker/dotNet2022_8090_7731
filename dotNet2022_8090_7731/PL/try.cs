using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    class Tryial<T>
    {

        public object Id
        {
            get
            {
                var a = typeof(T).GetProperty("id");
                return a;
            }

            set
            {
                var a = typeof(T).GetProperty("id");
                a.SetValue(a, value);
                //RaisePropertyChanged("Id");
                //validityMessages["Id"] = IdMessage(value, ID_LENGTH);
            }
        }

    }
}
