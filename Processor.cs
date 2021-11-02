using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Processor
    {

        public static async Task<Result> LoadInformation()
        {
            string key = "7d7e5775270005b2bf61";
            string url = $"https://free.currconv.com/api/v7/convert?q=USD_PHP&compact=ultra&apiKey=" +key;
            
            ApiHelper help = new ApiHelper();
            using (HttpResponseMessage response = await help.ApiClient.GetAsync(url))
            {

                //Console.WriteLine("Status code: " + response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    Result result = new Result();
                    var info = await response.Content.ReadAsAsync<JObject>();
                    
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(info))
                    {
                        try
                        {
                            string name = descriptor.Name;
                            object value = descriptor.GetValue(info);
                            result.ConvertType.Add(name);
                            result.ConvertRate.Add(value);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    return result;
                }
                else
                {
                    Console.WriteLine("Status code: " + response.StatusCode);
                    throw new Exception(response.ReasonPhrase);
                }
            }
            


        }

        
        public async void Print()
        {
            var info = await Processor.LoadInformation();

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

