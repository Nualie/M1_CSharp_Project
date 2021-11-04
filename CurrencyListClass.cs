using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class CurrencyListClass
    {
        public CurrencyList AllCurrencies;

        public CurrencyListClass()
        {
            string directory = Directory.GetCurrentDirectory();

            int index = directory.LastIndexOf("bin\\"); //get back to main resources directory
            if (index >= 0)
                directory = directory.Substring(0, index);

            this.AllCurrencies = LoadCurrencyJson(directory + "\\Json\\Currencies.json");
            
        }

        

        public CurrencyList LoadCurrencyJson(string address)
        {
            try
            {
                string jsonString = File.ReadAllText(address); //gets the json into a string
                CurrencyList myDeserializedClass = JsonConvert.DeserializeObject<CurrencyList>(jsonString);  //string to object
                return myDeserializedClass;

            }


            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
        public class Currency
        {
            public string currencyName { get; set; }
            public string currencySymbol { get; set; }
            public string id { get; set; }
        }

        public class CurrencyList
        {
            public List<Currency> Currencies { get; set; }
        }




    }
}
