using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ErrorHandler
    {

        public static void Exception(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }

        public static void Errors(Constants.States st)
        {
            Console.WriteLine("Error: " + st);
            Console.ReadLine();
        }
    }
}
