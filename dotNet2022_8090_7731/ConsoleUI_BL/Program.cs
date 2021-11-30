using System;
using static ConsoleUI_BL.MEnum;


namespace ConsoleUI_BL
{
    partial class Program
    {
        static IBL.IBL bL = new BL.BL();
        static void Main(string[] args)
        {
            int input = 0;
            while ((ProgramOptions)input != ProgramOptions.Exit)
            {
                try
                {
                    tools.PrintMain(typeof(ProgramOptions));
                    
                    CheckValids.CheckValid(1, 5, out input);
            
                    switch ((ProgramOptions)input)
                    {
                        case ProgramOptions.Adding:             AddingOption();                     break;

                        case ProgramOptions.Updating:           UpdatingOption();                   break;

                        case ProgramOptions.DisplayingItem:     DisplayingItemOption();             break;

                        case ProgramOptions.DisplayingList:     DisplayingListOption();             break;

                        case ProgramOptions.Exit:               Console.WriteLine("good bye!");     break;

                        default: break;

                    }
                }
                catch (BL.IdIsNotExistException idIsNotExistException)
                {
                    Console.WriteLine(idIsNotExistException);
                }
                catch (DalObject.IdIsNotExistException idIsNotExistException)
                {
                    Console.WriteLine(idIsNotExistException);
                }
                catch (BL.IdIsAlreadyExistException idIsAlreadyExistException)
                {
                    Console.WriteLine(idIsAlreadyExistException);
                }
                catch (BL.ListIsEmptyException listIsEmptyException)
                {
                    Console.WriteLine(listIsEmptyException);
                }
                catch (BL.InValidActionException inValidActionException)
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

