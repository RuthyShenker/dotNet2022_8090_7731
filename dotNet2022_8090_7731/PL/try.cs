using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{

    //https://stackoverflow.com/questions/52157249/check-the-entered-value-in-textboxes-are-double-number-in-wpf/52157968
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        public void Set<TValue>(ref TValue field, TValue newValue, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<TValue>.Default.Equals(field, default(TValue)) && field.Equals(newValue)) return;
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public abstract class ViewModelBase : ObservableBase
    {
        public bool IsInDesignMode
            => (bool)DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject))
                .DefaultValue;
    }
}
//    //class Tryial<T>
//    //{

//    //    public object Id
//    //    {
//    //        get
//    //        {
//    //            var a = typeof(T).GetProperty("id");
//    //            return a;
//    //        }

//    //        set
//    //        {
//    //            var a = typeof(T).GetProperty("id");
//    //            a.SetValue(a, value);
//    //            //RaisePropertyChanged("Id");
//    //            //validityMessages["Id"] = IdMessage(value, ID_LENGTH);
//    //        }
//    //    }

//    //}
//    interface   IResourcePolicy
//    {
//        private string version;

//        public string Version
//        {
//            get
//            {
//                return this.version;
//            }
//            set
//            {
//                this.version = value;
//            }
//        }
//    }
//}


