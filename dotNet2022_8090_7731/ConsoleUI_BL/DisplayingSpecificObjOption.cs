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
        private static void DisplayingSpecificObjOption()
        {
            tools.PrintEnum(typeof(DisplayingItem));

            CheckValids.CheckValid(1, 4, out int input);
            
            switch ((DisplayingItem)input)
            {
                case DisplayingItem.BaseStation:
                    Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.BaseStation, IBL.BO.Station>(GettingId("Base Station"))));
                    break;
                case DisplayingItem.Drone:
                    Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Drone, IBL.BO.Drone>(GettingId("Drone"))));
                    break;
                case DisplayingItem.Customer:
                    Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Customer, IBL.BO.Customer>(GettingId("Customer"))));
                    break;
                case DisplayingItem.Parcel:
                    Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Parcel, IBL.BO.Parcel>(GettingId("Parcel"))));
                    break;
                default:
                    break;
            }
        }
    }
}
