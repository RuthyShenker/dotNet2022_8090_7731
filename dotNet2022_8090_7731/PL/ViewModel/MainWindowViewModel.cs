using PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var bl = BlApi.BlFactory.GetBl();
            MainViewModel = new DisplayViewModel(bl);
            //MainView = new SignInView();
        }
        public object MainViewModel { get; set; }
    }
}
