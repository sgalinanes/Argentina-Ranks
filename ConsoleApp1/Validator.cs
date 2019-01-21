using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Validator
    {
        

        private static bool IsValidArgument(string[] arguments)
        {

            // We expect that the first argument is a string; and the second one is an integer.
            // If these assumptions are false, then we must throw an exception.
            int number = 0;
            number = Int32.Parse(arguments[Constants.Argument_Number_Position]);
 

            // Check if string is equal to a valid string
            int i;
            for (i = 0; i < Constants.Arguments.Count; i++)
            {

                if (string.Compare(arguments[Constants.Argument_String_Position].ToLower(), Constants.Arguments[i].ToLower()) == 0)
                {
                    break;
                }

            }

            if (i == Constants.Arguments.Count)
            {
                return false;
            }


            return true;

        }

        private static Constants.States GetArgument(string str)
        {

            if (String.Compare(str.ToLower(), Constants.Arguments[Constants.Argument_Elo_Position].ToLower()) == 0)
            {
                return Constants.States.ARGUMENT_ELO;
            }
            else if(String.Compare(str.ToLower(), Constants.Arguments[Constants.Argument_Years_Position].ToLower()) == 0)
            {
                return Constants.States.ARGUMENT_YEAR;
            }
            else
            {
                return Constants.States.ARGUMENT_ADD;
            }
        }

        public static Constants.States Arguments(string[] arguments, ref Constants.States action)
        {

            // Possible arguments:
            // 1) Elo N where N is the number of teams < 100 (sorted) : List of ELO rated teams
            // 2) Years N where N is the number of teams < 100 (sorted) : Give yearly lead of each team. [revised for month weight]
            // 3) Add file where FILE is a .txt file with RSSF.org format

            if(arguments.Length > Constants.ArgumentsLength)
            {
                return Constants.States.ERROR_MAX_ARGUMENT;
            }

            if(!IsValidArgument(arguments))
            {
                return Constants.States.ERROR_NOT_VALID_ARGUMENT;
            }

            action = GetArgument(arguments[Constants.Argument_String_Position]);


            return Constants.States.OK;

        }


    }
}


