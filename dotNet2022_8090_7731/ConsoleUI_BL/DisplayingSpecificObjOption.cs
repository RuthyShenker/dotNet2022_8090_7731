using BL;
using IDal.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void DisplayingSpecificObjOption()
        {
            Console.WriteLine(
                         $"1 - Base station view\n" +
                         $"2 - Drone view\n" +
                         $"3 - Customer view\n" +
                         $"4 - Package view");
            int input;

            CheckValids.CheckValid(1, 4, out input);
            try
            {
                switch (input)
                {
                    case 1:
                        Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.BaseStation, IBL.BO.Station>(GettingId("Base Station"))));
                        break;
                    case 2:
                        Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Drone, IBL.BO.Drone>(GettingId("Drone"))));
                        break;
                    case 3:
                        Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Customer, IBL.BO.Customer>(GettingId("Customer"))));
                        break;
                    case 4:
                        Console.WriteLine(Tools.ToString(bL.GetBLById<IDal.DO.Parcel, IBL.BO.Parcel>(GettingId("Parcel"))));
                        break;
                    default:
                        break;

                }
            }
            catch (IdNotExistInTheListException exception )
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
