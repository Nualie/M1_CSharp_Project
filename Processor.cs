using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Bank.CurrencyListClass;

namespace Bank
{
    public class Processor
    {
        public CurrencyListClass AllCurrencies { get; set; }
        public List<string> AllCurrencyIds { get; set; }

        public Processor()
        {
            AllCurrencies = new CurrencyListClass();
            AllCurrencyIds = new List<string>();
            foreach(Currency a in AllCurrencies.AllCurrencies.Currencies)
            {
                AllCurrencyIds.Add(a.id);
            }

        }
        public static string convertCurrency(string to, string from)
        {
            return from+"_"+to;
        }



        public static async Task<Result> LoadConvertInformation(string convertString)
        {
            string key = "7d7e5775270005b2bf61";
            string url = $"https://free.currconv.com/api/v7/convert?q="+convertString+"&compact=ultra&apiKey="+key;
            
            //https://free.currconv.com/api/v7/convert?q=USD_PHP&compact=ultra&apiKey=7d7e5775270005b2bf61

            ApiHelper help = new ApiHelper();
            using (HttpResponseMessage response = await help.ApiClient.GetAsync(url))
            {
                Console.WriteLine("Status code: " + response.StatusCode);
                Result result = new Result();
                if (response.IsSuccessStatusCode)
                {

                    Result info = new Result();

                    try
                    {
                        info = await response.Content.ReadAsAsync<Result>();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    result.ConvertType = info.ConvertType; 
                    result.ConvertRate = info.ConvertRate; 

                }
                else
                {
                    Console.WriteLine("Status code: " + response.StatusCode);
                    throw new Exception(response.ReasonPhrase);
                    
                }
                return result;
            }
            


        }

        public static async Task<Result> ReturnConvertInfo(string from, string to)
        {
            if(from == to)
            {
                return new Result();
            }
            string convertString = convertCurrency(from, to);
            Result info = await Processor.LoadConvertInformation(convertString);
            
            return info;
        
        }



        public static async void PrintConvertInfo(string from, string to)
        {
            var info = await ReturnConvertInfo(to, from);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(info))
            {
                try
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(info);
                    Console.WriteLine("{0}= {1}", name, value);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


        }
        public static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine("Status code: "+response.StatusCode);
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                //Any exception will returns false.
                Console.WriteLine(e);
                return false;
            }

        }
    } 
}

