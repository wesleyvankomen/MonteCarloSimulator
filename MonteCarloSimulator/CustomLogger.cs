using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace MonteCarloSimulator
{
    class CustomLogger
    {
        public static void Log(String msg)
        {
            try { 
                // append message to end of file
                //TODO: use dynamic pathing for log locations
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"..\..\logs\log.txt", true);

                //write time stamp and message
                file.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"), msg));

                file.Close();
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
        }

        public static void LogCritical(String msg)
        {
            try {
                //TODO: use dynamic pathing for log locations
                // append message to end of file
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"..\..\logs\critical_log.txt", true);

                //write time stamp and message
                file.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"), msg));

                file.Close();
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
