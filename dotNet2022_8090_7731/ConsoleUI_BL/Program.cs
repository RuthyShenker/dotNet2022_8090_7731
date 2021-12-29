using System;
using static ConsoleUI_BL.MEnum;


namespace ConsoleUI_BL
{
    partial class Program
    {
        private static readonly BlApi.IBL bl = BlApi.BlFactory.GetBl();

        static void Main(string[] args)
        {
            BlApi.IBL bL = BlApi.BlFactory.GetBl();
            int input = 0;
            while ((ProgramOptions)input != ProgramOptions.Exit)
            {
                try
                {
                    tools.PrintMain(typeof(ProgramOptions));
                    
                    CheckValids.CheckValid(1, 5, out input);
            
                    switch ((ProgramOptions)input)
                    {
                        case ProgramOptions.Adding:             AddingOption(bL);                     break;

                        case ProgramOptions.Updating:           UpdatingOption(bL);                   break;

                        case ProgramOptions.DisplayingItem:     DisplayingItemOption(bL);             break;

                        case ProgramOptions.DisplayingList:     DisplayingListOption(bL);             break;

                        case ProgramOptions.Exit:               Console.WriteLine("good bye!");     break;

                        default: break;

                    }
                }
                catch (BO.IdIsNotExistException idIsNotExistException)
                {
                    Console.WriteLine(idIsNotExistException);
                }
                catch (BO.IdIsAlreadyExistException idIsAlreadyExistException)
                {
                    Console.WriteLine(idIsAlreadyExistException);
                }
                catch (BO.ListIsEmptyException listIsEmptyException)
                {
                    Console.WriteLine(listIsEmptyException);
                }
                catch (BO.InValidActionException inValidActionException)
                {
                    Console.WriteLine(inValidActionException);
                }
            }
        }

        /// <summary>
        /// A function that gets from the user Id and returns it as string
        /// </summary>
        /// <param name="obj">a name of object</param>
        /// <returns>an id as string</returns>
        private static string GettingIdAsString(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return Console.ReadLine();
        }

        /// <summary>
        /// A function that gets from the user Id and returns it as int
        /// </summary>
        /// <param name="obj">a name of object</param>
        /// <returns>an id as int</returns>
        private static int GettingId(string obj)
        {
            Console.WriteLine($"Enter The Id Of The {obj}:");
            return CheckValids.InputNumberValidity("Id Of " + obj);
        }
    }
}

