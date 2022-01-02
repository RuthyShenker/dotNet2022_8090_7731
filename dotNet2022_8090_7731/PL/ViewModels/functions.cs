using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class Functions
    {
        public static void CloseWindowCommand(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
    }
}
