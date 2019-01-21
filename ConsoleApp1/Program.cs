using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            string conn_str = Constants.SqlConnectionString;
            Constants.States st;
            Constants.States action = Constants.States.OK;
            SqlConnection conn = Database.Connect(conn_str); 
            try
            {
                // FIX ELO & YEARS NUMBER CAN BE ANYTHING. 
                if ((st = Validator.Arguments(args, ref action)) != Constants.States.OK)
                {
                    ErrorHandler.Errors(st);
                }

                // Possible arguments:
                // 1) Elo N where N is the number of teams < 100 (sorted) : List of ELO rated teams
                // 2) Years N where N is the number of teams < 100 (sorted) : Give yearly lead of each team. [revised for month weight]
                // 3) Add file where FILE is a .txt file with RSSF.org format
                conn.Open();

                if(action == Constants.States.ARGUMENT_ADD)
                {
                    Reader reader = new Reader();


                    if(( st = reader.Read(args[Constants.Argument_Number_Position], conn)) != Constants.States.OK)
                    {
                        ErrorHandler.Errors(st);
                    }
                    
                }

                // MAKE CONSTANTS FOR THIS....
                //query = "CREATE TABLE IF NOT EXISTS " + argument + " (Team char(50), Before_ELO float, After_ELO float)";
                //SqlCommand command = new SqlCommand(query, conn);
                //command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                ErrorHandler.Exception(ex);
            }
            finally
            {

                conn.Close();
                conn.Dispose();
            }


            Console.ReadLine();

        }
    }
}
