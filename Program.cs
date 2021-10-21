using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            Root test = LoadJson("D:\\School\\CSharp\\Project\\M1_CSharp_Project\\ClientList.json");
            try
            {
                test.Client[0].print();
            }catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
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

