using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using System.Data.SQLite;

namespace Bank
{
    class Program
    {
        static void Main()
        {

            string directory = returnDirectory();


            Root test = LoadClientJson(directory+"\\Json\\ClientList.json");

            Processor process = new Processor();

            SQLiteManager.CheckVersion();
            SQLiteManager.test();

        }

        public static string returnDirectory()
        {
            string directory = Directory.GetCurrentDirectory();

            int index = directory.LastIndexOf("bin\\"); //get back to main resources directory
            if (index >= 0)
                directory = directory.Substring(0, index);

            return directory;
        }
 
        public static Root LoadClientJson(string address)
        {
            try
            {
                string jsonString = File.ReadAllText(address); //gets the json into a string
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);  //string to object 
                return myDeserializedClass;

            }


            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
    }

    
}

