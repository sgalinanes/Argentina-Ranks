using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Reader
    {


        public Reader()
        {

        }

        public Constants.States Read(string argument, SqlConnection conn)
        {
            /*
             * Reads upon a txt file, defined by certain parameters, procceses the information
             * and returns a DataTable with some pre-established configuration.
             * Exception is catched by Program.cs
             * Errors will return a null dt(?
             */


            // HARD CODED
            string path = @"C:\Users\TIAGO\txts\" + argument + ".txt";
            Constants.States state = Constants.States.OK;
            
            if(!File.Exists(path))
            {
                return Constants.States.ERROR_FILE_PATH_NOT_EXIST;
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string line = "";
                int i = 1;

                // Go through data.
                while ((line = sr.ReadLine()) != null)
                {

                    //Look for the FIRST path delimiter.
                    if (line.ToLower().Contains(Constants.File_PathDelimiter.ToLower() + " " + i))
                    {
                        // Update current path delimiter count
                        i++;
                        for (i = 2; ; i++)
                        {
                            List<string[]> temp = new List<string[]>();
                            
                            if( (state = this.Process(i, ref temp, ref line, sr)) != Constants.States.OK)
                            {
                                Database.Update(conn, temp);
                                break;
                            }

                            Database.Update(conn, temp);
                        }
                            
                    }
                }
            }

            return Constants.States.OK;
        }

        public Constants.States Process(int i, ref List<string[]> round, ref string line, StreamReader sr)
        {

            // Procces the information into the data table.
            while ((line = sr.ReadLine()) != null)
            {

                // Look IF WE ARE IN the next path delimiter.
                if (line.ToLower().Contains(Constants.File_PathDelimiter.ToLower() + " " + i))
                {

                    return Constants.States.OK;

                }

                // Process the information.
                line = line.Trim();

                // Check if the data has correct format (i.e its a match)


                string[] game = new string[Constants.MatchDataLength];
                if (isDataFormat(line, ref game))
                {
                    if(game == null)
                    {
                        return Constants.States.ERROR_NULL;
                    }

                    Console.WriteLine("Reading data...");
                    Console.WriteLine(game[0] + ", " + game[1] + ", " + game[2] + ", " + game[3]);

                    round.Add(game);
                    continue;

                }

            }

            return Constants.States.FILE_EOF;
        }

        public bool isDataFormat(string line, ref string[] data)
        {
            string[] result = null;
            string[] _data = new string[] { "", "", "", "" };
            int realLength = Constants.MatchDataLength;
            string str = "";
            int n = 0; int counter = 0;
            bool added = false;


            char[] charSeparators = new char[] { ':', ' '};

            result = line.Split(charSeparators);

            if(result.Length < Constants.MatchDataLength)
            {
                return false;
            }


            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == String.Empty)
                {
                    continue;
                }

                bool isNumeric = int.TryParse(result[i], out n);
                str = result[i];

                if(!isNumeric)
                {
                    int j;
                    for(j = 1; i+j < result.Length; j++)
                    {
                        isNumeric = int.TryParse(result[i + j], out n);
                        if(isNumeric || result[i+j] == String.Empty) { added = true;  i += (j-1); /* We add j-1 because its gonna add +1 for i++ */ break; }
                        str += " " + result[i + j];
                    }

                    if(added)
                    {
                        // nada...
                    } else { i += (j-1); }
                    added = false;

                }

                // HARD - CODED
                if(!str.Contains('[') && counter < _data.Length)
                {

                    isNumeric = int.TryParse(str, out n);
                    if(counter == 0 || counter == 3)
                    {
                        if(isNumeric) { _data = null;  return false; }
                    } else if(counter == 1 || counter == 2)
                    {
                        if(!isNumeric) { _data = null;  return false; }
                    }

                    _data[counter] = str;
                    counter++;
                }



            }

            foreach(string s in _data)
            {

                if(s == String.Empty)
                {
                    realLength -= 1;
                }

            }

            if(_data.Length != realLength)
            {
                return false;
            }

            data = _data;

            return true;
        }
    }
}
