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

namespace Bank
{
    class Program
    {
        static void Main()
        {
        
            string directory = Directory.GetCurrentDirectory();
           
            int index = directory.LastIndexOf("bin\\"); //get back to main resources directory
            if (index >= 0)
                directory = directory.Substring(0, index);
            
            Root test = LoadClientJson(directory+"\\Json\\ClientList.json");

            Processor process = new Processor();
            Console.WriteLine(1);
            test.Client[0].ViewTotalAmount();
            /*var info = Processor.LoadInformation();
            try
            {
                Console.WriteLine(info.Result.ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine(e);            
            }*/


        }

        static Root LoadClientJson(string address)
        {
            try
            {
                string jsonString = File.ReadAllText(address); //gets the json into a string
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);  //string to object GET THE OBJECT CODE OFF A WEBSITE
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

