using System;

namespace ConsoleUI_BL
{
    class Program:IBL
    {
        static void Main(string[] args)
        {
            IBL BL = new BL();

            int input = 0;
            while ((ProgramOptions)input != ProgramOptions.Exit)
            {
                Console.WriteLine
                  ($"\nTap the desired option:\n" +
                  $"1 - Adding\n" +
                  $"2 - Updating\n" +
                  $"3 - Displaying a specific item\n" +
                  $"4 - Displaying a specific List\n" +
                  $"5 - Exit");
                CheckValids.CheckValid(1, 5, out input);
                switch ((ProgramOptions)input)
                {
                    case ProgramOptions.Add:
                        AddingOption();
                        break;

                    case ProgramOptions.Update:
                        UpdatingOption();
                        break;

                    case ProgramOptions.DisplaySpecific:
                        DisplayingSpecificObjOption();
                        break;

                    case ProgramOptions.DisplayList:
                        DisplayingListOption();
                        break;

                    case ProgramOptions.Exit:
                        {
                            Console.WriteLine("good bye!");
                            break;
                        }

                    default:
                        break;
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
            return int.Parse(Console.ReadLine());
        }


    }
}
    
}
