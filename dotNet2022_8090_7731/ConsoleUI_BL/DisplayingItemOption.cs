using BL;
using IDal.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleUI_BL.MEnum;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void DisplayingItemOption(IBL.IBL bL)
        {
            tools.PrintEnum(typeof(DisplayingItem));

            CheckValids.CheckValid(1, 4, out int input);
            
            switch ((DisplayingItem)input)
            {
                case DisplayingItem.BaseStation:
                    Console.WriteLine(Tools.ToStringProps(bL.GetStation(GettingId("Base Station"))));
                    break;
                case DisplayingItem.Drone:
                    Console.WriteLine(Tools.ToStringProps(bL.GetDrone(GettingId("Drone"))));
                    break;
                case DisplayingItem.Customer:
                    Console.WriteLine(Tools.ToStringProps(bL.GetCustomer(GettingId("Customer"))));
                    break;
                case DisplayingItem.Parcel:
                    Console.WriteLine(Tools.ToStringProps(bL.GetParcel(GettingId("Parcel"))));
                    break;
                default:
                    break;
            }
        }
    }
}
