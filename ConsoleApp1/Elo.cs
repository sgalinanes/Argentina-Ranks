using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Elo
    {

        public Elo()
        {
            // Constructor
        }


        public int GetKFactor(double rating)
        {


            if(rating < Constants.Elo_Rating_Limit_1)
            {
                return Constants.Elo_K_Factor_Limit_1;

            } else if(rating < Constants.Elo_Rating_Limit_2)
            {
                return Constants.Elo_K_Factor_Limit_2;

            } else
            {
                return Constants.Elo_K_Factor_Last;
            }

 
        }
        public void GetNewRating(string resultOne, string resultTwo, double teamOneRating, double teamTwoRating, ref double teamOneNewRating, ref double teamTwoNewRating)
        {
            // Calculates new Elo for teamOne.

            double result = 0;

            double r1 = teamOneRating;
            double r2 = teamTwoRating;

            // We know they are numbers for sure because they have the format required to be in here.
            bool winOne = (int.Parse(resultOne) > int.Parse(resultTwo));

            if (winOne) { result = 1; }
            else if (int.Parse(resultOne) == int.Parse(resultTwo)) { result = 0.5; }
            else
                { result = 0; }

            double q1 = Math.Pow(10, (r1 / 400));
            double q2 = Math.Pow(10, (r2 / 400));
            double e1 = q1 / (q1 + q2);
            double e2 = q2 / (q1 + q2);

            teamOneNewRating = r1 + GetKFactor(r1) * (result - e1);
            teamTwoNewRating = r2 + GetKFactor(r2) * ((1 - result) - e2);

        }


        
    }
}
