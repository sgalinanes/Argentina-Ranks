using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp1
{
    class Database
    {
        public static SqlConnection Connect(string conn_str)
        {
            SqlConnection conn = new SqlConnection(conn_str);
            return conn;
        }

        public static double GetCurrentRating(SqlConnection conn, string name)
        {
            double rating = Constants.Elo_Rating_Start;
            object value;

            string query = "SELECT elo FROM " + Constants.SqlDatabaseELOName + " WHERE Team = '" + name + "'";
            Console.WriteLine("Query to database: " + query);

            SqlCommand command = new SqlCommand(query, conn);
            if(( value = command.ExecuteScalar()) == null)
            {
                query = "INSERT INTO " + Constants.SqlDatabaseELOName + " (Team, elo) VALUES (@name, @rating)";
                Console.WriteLine("name is: " + name + ", rating is: " + rating);
                Console.WriteLine("Query to execute: " + query);

                SqlCommand comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@name", name);
                comm.Parameters.AddWithValue("@rating", rating);

                comm.ExecuteNonQuery();
            }
            else
            {
                rating = (double)value;
            }

            return rating;
        }

        public static void Update(SqlConnection conn, List<string[]> data)
        {

            string teamOne = String.Empty; string teamTwo = String.Empty; string resultOne = String.Empty; string resultTwo = String.Empty;

            string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            SqlCommand c = new SqlCommand(query, conn);
            int roundCount = (int)c.ExecuteScalar();


            query = "CREATE TABLE dbo.Round_" + roundCount + " (Team varchar(max) NOT NULL, Before_ELO float NOT NULL, After_ELO float NOT NULL) ";
            SqlCommand comm = new SqlCommand(query, conn);
            comm.ExecuteNonQuery();
            comm.Dispose();

            for (int j = 0; j < data.Count; j++)
            {
                // TODO HARD CODE
                teamOne = data[j][0];
                resultOne = data[j][1];
                resultTwo = data[j][2];
                teamTwo = data[j][3];

                DataTable dt = new DataTable();
                Elo elo = new Elo();

                dt = MakeEloDataTable();

                double ratingOne = GetCurrentRating(conn, teamOne);
                Console.WriteLine("Rating of team " + teamOne + " is " + ratingOne);
                double ratingTwo = GetCurrentRating(conn, teamTwo);
                Console.WriteLine("Rating of team " + teamTwo + " is " + ratingTwo);


                double newRatingOne = 0; double newRatingTwo = 0;
                elo.GetNewRating(resultOne, resultTwo, ratingOne, ratingTwo, ref newRatingOne, ref newRatingTwo);

                Console.WriteLine("New rating of team " + teamOne + " is " + newRatingOne);
                Console.WriteLine("New rating of team " + teamTwo + " is " + newRatingTwo);

                Console.ReadLine();
                dt.Rows.Add(teamOne, ratingOne, newRatingOne);
                dt.Rows.Add(teamTwo, ratingTwo, newRatingTwo);



                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    query = "INSERT INTO dbo.Round_" + roundCount + " (Team, Before_ELO, After_ELO) VALUES (@team, @before_elo, @after_elo)";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@team", k == 0 ? teamOne : teamTwo);
                    command.Parameters.AddWithValue("@before_elo", k == 0 ? ratingOne : ratingTwo);
                    command.Parameters.AddWithValue("@after_elo", k == 0 ? newRatingOne : newRatingTwo);
                    command.ExecuteNonQuery();

                    query = "UPDATE " + Constants.SqlDatabaseELOName + " SET elo = @elo WHERE Team = @team";
                    SqlCommand command2 = new SqlCommand(query, conn);
                    command2.Parameters.AddWithValue("@team", k == 0 ? teamOne : teamTwo);
                    command2.Parameters.AddWithValue("@elo", k == 0 ? newRatingOne : newRatingTwo);
                    command2.ExecuteNonQuery();

                }
            }
            
        }

        public static DataTable MakeEloDataTable()
        {
            // Returns a dataTable that has the properties of the SQL table.
            DataTable dt = new DataTable();



            dt.Columns.Add("Team", typeof(string));
            dt.Columns.Add("Before_ELO", typeof(float));
            dt.Columns.Add("After_ELO", typeof(float));

            
            
            return dt;
        }

    }
}
