﻿using System;

namespace ConsoleUI_BL
{
    partial class Program
    {
       static IBL.IBL bL = new BL.BL();
        static void Main(string[] args)
        {
            
            int input = 0;
            
            while ((Enum.ProgramOptions)input != Enum.ProgramOptions.Exit)
            {
                Console.WriteLine
                  ($"\nTap the desired option:\n" +
                  $"1 - Adding\n" +
                  $"2 - Updating\n" +
                  $"3 - Displaying a specific item\n" +
                  $"4 - Displaying a specific List\n" +
                  $"5 - Exit");
                CheckValids.CheckValid(1, 5, out input);
                switch ((Enum.ProgramOptions)input)
                {
                    case Enum.ProgramOptions.Add:
                        AddingOption();
                        break;

                    case Enum.ProgramOptions.Update:
                        UpdatingOption();
                        break;

                    case Enum.ProgramOptions.DisplaySpecific:
                        DisplayingSpecificObjOption();
                        break;

                    case Enum.ProgramOptions.DisplayList:
                        DisplayingListOption();
                        break;

                    case Enum.ProgramOptions.Exit:
                        {
                            Console.WriteLine("good bye!");
                            break;
                        }

                    default:
                        break;
                }
            }
        }
    }
}