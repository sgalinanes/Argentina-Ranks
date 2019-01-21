using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Constants
    {

        public static List<string> Arguments = new List<string>(new string[] { "elo", "years", "add" });
        public static int ArgumentsNumber = 100;
        public static int Argument_String_Position = 0;
        public static int Argument_Number_Position = 1;
        public static int ArgumentsLength = 2;

        public static int Argument_Elo_Position = 0;
        public static int Argument_Years_Position = 1;
        public static int Argument_Add_Position = 2;

        public enum States
        {
            OK,
            ERROR_NOT_VALID_ARGUMENT,
            ERROR_MAX_ARGUMENT,

            ERROR_FILE_PATH_NOT_EXIST,
            FILE_EOF,
            ERROR_NULL,

            ARGUMENT_ELO,
            ARGUMENT_YEAR,
            ARGUMENT_ADD
        }

        public static string SqlConnectionString = "data source = localhost\\SQLEXPRESS;initial catalog = master; trusted_connection=true;";
        public static string SqlDatabaseTableName = "Elo";
        public static string SqlDatabaseELOName = "dbo.Current_Elo";

        public static string File_PathDelimiter = "Round";

        public static int MatchDataLength = 4;
        public static char MatchDataDelimiter = ':';

        public static double Elo_Rating_Start = 1500;

        public static int Elo_Rating_Limit_1 = 2100;
        public static int Elo_Rating_Limit_2 = 2400;

        public static int Elo_K_Factor_Limit_1 = 32;
        public static int Elo_K_Factor_Limit_2 = 24;
        public static int Elo_K_Factor_Last = 16;

    }
}
