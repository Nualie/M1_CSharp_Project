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
            //Root test = LoadJson("D:\\School\\CSharp\\Project\\M1_CSharp_Project\\Json\\ClientList.json");

            Processor process = new Processor();
            var info = Processor.LoadInformation();
            Console.WriteLine(info.Result.ToString());
        }
        static Root LoadJson(string address)
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

