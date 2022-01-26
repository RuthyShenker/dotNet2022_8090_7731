using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class Refresh
    {
        public static event DelEventHandler add;

        public static void Add()
        {
            add += new DelEventHandler(India);
            add += new DelEventHandler(StationsList);
            add += new DelEventHandler(England);
            add.Invoke();

            //Console.ReadLine();
        }

        public static Action StationsList;
        //public static void USA()
        //{
        //    MessageBox.Show("USA");
        //    //Console.WriteLine("USA");
        //}

        public static void India()
        {
            MessageBox.Show("India");

            //Console.WriteLine("India");
        }

        public static void England()
        {
            MessageBox.Show("England");

        }


    }
}

