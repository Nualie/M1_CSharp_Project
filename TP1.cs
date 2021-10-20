using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;

namespace TP1_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            ex5();
        }


        static void LoadJson(string address)
        {
            try
            {
                string jsonString = File.ReadAllText(address); //gets the json into a string
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);  //string to object GET THE OBJECT CODE OFF A WEBSITE

                //display below
                Console.WriteLine("\nConfig");
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myDeserializedClass.config))
                {
                    try
                    {
                        string name = descriptor.Name;
                        object value = descriptor.GetValue(myDeserializedClass.config);
                        Console.WriteLine("{0}={1}", name, value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                Console.WriteLine("\nUsage");
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myDeserializedClass.usage))
                {
                    try
                    {
                        string name = descriptor.Name;
                        object value = descriptor.GetValue(myDeserializedClass.usage);
                        Console.WriteLine("{0}={1}", name, value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                Console.WriteLine("\nData");

                foreach (var item in myDeserializedClass.data)
                {
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                    {
                        try
                        {
                            string name = descriptor.Name;
                            if (name != "timeSeries")
                            {
                                object value = descriptor.GetValue(item);
                                Console.WriteLine("{0}={1}", name, value);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }


                    }
                }

                Console.WriteLine("TimeSeries");
                for (int i = 0; i < myDeserializedClass.data.Count; i++)
                {
                    foreach (var item in myDeserializedClass.data[0].timeSeries)
                    {
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                        {
                            try
                            {
                                string name = descriptor.Name;
                                object value = descriptor.GetValue(item);
                                Console.WriteLine("{0}={1}", name, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }



                        }
                    }
                }




            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        static void ex5()
        {

            LoadJson("D:\\School\\CSharp\\TP1 CSharp\\DOGE_AllDataPoints_3Days.json");

        }

        static void ex4()
        {
            int[] dimensions = AskUserFor2Parameters();
            int M = dimensions[0];
            int N = dimensions[1];
            Console.WriteLine($"{M}x{N}\n");
            if (M >= 1 && M <= 1000 && N >= 1 && N <= 1000)
            {
                Console.Write("0");
                for (int i = 0; i < M; i++)
                {
                    Console.Write("_");
                }
                Console.Write("0\n");
                for (int j = 0; j < N; j++)
                {
                    Console.Write("|");
                    for (int i = 0; i < M; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("|\n");
                }
                Console.Write("0");
                for (int i = 0; i < M; i++)
                {
                    Console.Write("_");
                }
                Console.Write("0\n");
                return;
            }
            Console.WriteLine("Wrong dimensions.");
        }

        private static int PowerFunction(int x)
        {
            return (int)(Math.Pow(x, 2) - 4);
        }

        static void ex3()
        {
            for (int i = -3; i <= 3; i++)
                try
                {
                    Console.WriteLine($"{i}: {10 / PowerFunction(i)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{i} didn't work.");
                }

        }

        static void ex2c()
        {
            int j = 1;
            int n = AskUserForParameter();
            for (int i = 1; i <= n; i++)
            {
                j *= i;
            }
            Console.WriteLine($"!{n} = {j}");
        }
        static void ex2b()
        {
            int a = 0;
            int b = 1;
            int c = 0;
            int i = 2;
            int n = AskUserForParameter();
            if (n < 1000)
            {
                while (i <= n)
                {
                    c = a + b;
                    a = b;
                    b = c;
                    i += 1;
                }
                Console.WriteLine($" F({n}) = {c}");
                return;
            }
            Console.WriteLine("Number N too big");
        }
        static void ex2a()
        {
            int n = AskUserForParameter();
            int i = 2;
            if (n < 1000)
            {
                while (i < n / 2)
                {
                    if (n % i == 0)
                    {
                        Console.WriteLine($"{n} is not prime (divisible by {i}).");
                        return;
                    }
                    i += 1 + Convert.ToInt32(i % 2 != 0);
                }
                Console.WriteLine($"{n} is prime.");
                return;

            }
            Console.WriteLine("Number N too big");

        }
        static void ex1a()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine("\nTable of {0}", i);
                for (int j = 0; j <= 10; j++)
                {
                    Console.WriteLine($"{i} * {j} = {i * j}");
                }
            }
        }
        static void ex1b()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine("\nTable of {0}", i);
                for (int j = 0; j <= 10; j++)
                {
                    if (j * i % 2 == 1)
                    {
                        Console.WriteLine($"{i} * {j} = {i * j}");
                    }
                }
            }
        }

        private static int[] AskUserFor2Parameters()
        {
            int[] result = new int[2];
            Console.WriteLine("Please write two numbers and press enter :");
            string[] tokens = Console.ReadLine().Split();
            if (tokens.Length == 2)
            {
                int.TryParse(tokens[0], out result[0]);
                int.TryParse(tokens[1], out result[1]);
            }
            else
            {
                Console.WriteLine("Wrong input number.");
                result[0] = 0;
                result[1] = 0;
            }
            return result;
        }

        private static int AskUserForParameter()
        {
            Console.WriteLine("Please write a number and press enter :");
            int.TryParse(Console.ReadLine(), out var result);
            return result;
        }

        static void ex1c()
        {
            int i = AskUserForParameter();
            if (i <= 1000)
            {
                Console.WriteLine("\nTable of {0}", i);
                for (int j = 0; j <= 10; j++)
                {
                    Console.WriteLine($"{i} * {j} = {i * j}");
                }
            }
            else
                Console.WriteLine("N too big");

        }

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Config
    {
        public string data { get; set; }
        public int data_points { get; set; }
        public string interval { get; set; }
        public string symbol { get; set; }
    }

    public class Usage
    {
        public int day { get; set; }
        public int month { get; set; }
    }

    public class TimeSery
    {
        public int asset_id { get; set; }
        public int time { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double volume { get; set; }
        public object market_cap { get; set; }
        public int url_shares { get; set; }
        public int unique_url_shares { get; set; }
        public int reddit_posts { get; set; }
        public int reddit_posts_score { get; set; }
        public int reddit_comments { get; set; }
        public int reddit_comments_score { get; set; }
        public int tweets { get; set; }
        public int tweet_spam { get; set; }
        public int tweet_followers { get; set; }
        public int tweet_quotes { get; set; }
        public int tweet_retweets { get; set; }
        public int tweet_replies { get; set; }
        public int tweet_favorites { get; set; }
        public int tweet_sentiment1 { get; set; }
        public int tweet_sentiment2 { get; set; }
        public int tweet_sentiment3 { get; set; }
        public int tweet_sentiment4 { get; set; }
        public int tweet_sentiment5 { get; set; }
        public int tweet_sentiment_impact1 { get; set; }
        public int tweet_sentiment_impact2 { get; set; }
        public int tweet_sentiment_impact3 { get; set; }
        public int tweet_sentiment_impact4 { get; set; }
        public int tweet_sentiment_impact5 { get; set; }
        public int social_score { get; set; }
        public double average_sentiment { get; set; }
        public int sentiment_absolute { get; set; }
        public int sentiment_relative { get; set; }
        public int news { get; set; }
        public double price_score { get; set; }
        public double social_impact_score { get; set; }
        public double correlation_rank { get; set; }
        public double galaxy_score { get; set; }
        public double volatility { get; set; }
        public int alt_rank { get; set; }
        public int alt_rank_30d { get; set; }
        public double alt_rank_hour_average { get; set; }
        public int market_cap_rank { get; set; }
        public int percent_change_24h_rank { get; set; }
        public int volume_24h_rank { get; set; }
        public int social_volume_24h_rank { get; set; }
        public int social_score_24h_rank { get; set; }
        public int social_contributors { get; set; }
        public int social_volume { get; set; }
        public double price_btc { get; set; }
        public int social_volume_global { get; set; }
        public double social_dominance { get; set; }
        public object market_cap_global { get; set; }
        public double market_dominance { get; set; }
        public double percent_change_24h { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public double price { get; set; }
        public double price_btc { get; set; }
        public long market_cap { get; set; }
        public double percent_change_24h { get; set; }
        public double percent_change_7d { get; set; }
        public double percent_change_30d { get; set; }
        public double volume_24h { get; set; }
        public int social_contributors_calc_24h_previous { get; set; }
        public int url_shares_calc_24h_previous { get; set; }
        public int tweet_spam_calc_24h_previous { get; set; }
        public int news_calc_24h_previous { get; set; }
        public double average_sentiment_calc_24h_previous { get; set; }
        public int social_score_calc_24h_previous { get; set; }
        public int social_volume_calc_24h_previous { get; set; }
        public int alt_rank_30d_calc_24h_previous { get; set; }
        public int alt_rank_calc_24h_previous { get; set; }
        public int social_contributors_calc_24h { get; set; }
        public double social_contributors_calc_24h_percent { get; set; }
        public int url_shares_calc_24h { get; set; }
        public double url_shares_calc_24h_percent { get; set; }
        public int tweet_spam_calc_24h { get; set; }
        public double tweet_spam_calc_24h_percent { get; set; }
        public int news_calc_24h { get; set; }
        public double news_calc_24h_percent { get; set; }
        public double average_sentiment_calc_24h { get; set; }
        public double average_sentiment_calc_24h_percent { get; set; }
        public int social_score_calc_24h { get; set; }
        public double social_score_calc_24h_percent { get; set; }
        public int social_volume_calc_24h { get; set; }
        public double social_volume_calc_24h_percent { get; set; }
        public int asset_id { get; set; }
        public int time { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double volume { get; set; }
        public int url_shares { get; set; }
        public int unique_url_shares { get; set; }
        public int reddit_posts { get; set; }
        public int reddit_posts_score { get; set; }
        public int reddit_comments { get; set; }
        public int reddit_comments_score { get; set; }
        public int tweets { get; set; }
        public int tweet_spam { get; set; }
        public int tweet_followers { get; set; }
        public int tweet_quotes { get; set; }
        public int tweet_retweets { get; set; }
        public int tweet_replies { get; set; }
        public int tweet_favorites { get; set; }
        public int tweet_sentiment1 { get; set; }
        public int tweet_sentiment2 { get; set; }
        public int tweet_sentiment3 { get; set; }
        public int tweet_sentiment4 { get; set; }
        public int tweet_sentiment5 { get; set; }
        public int tweet_sentiment_impact1 { get; set; }
        public int tweet_sentiment_impact2 { get; set; }
        public int tweet_sentiment_impact3 { get; set; }
        public int tweet_sentiment_impact4 { get; set; }
        public int tweet_sentiment_impact5 { get; set; }
        public int social_score { get; set; }
        public double average_sentiment { get; set; }
        public int sentiment_absolute { get; set; }
        public int sentiment_relative { get; set; }
        public int news { get; set; }
        public double price_score { get; set; }
        public double social_impact_score { get; set; }
        public double correlation_rank { get; set; }
        public double galaxy_score { get; set; }
        public double volatility { get; set; }
        public int alt_rank { get; set; }
        public int alt_rank_30d { get; set; }
        public double alt_rank_hour_average { get; set; }
        public int market_cap_rank { get; set; }
        public int percent_change_24h_rank { get; set; }
        public int volume_24h_rank { get; set; }
        public int social_volume_24h_rank { get; set; }
        public int social_score_24h_rank { get; set; }
        public int social_contributors { get; set; }
        public int social_volume { get; set; }
        public int social_volume_global { get; set; }
        public double social_dominance { get; set; }
        public long market_cap_global { get; set; }
        public double market_dominance { get; set; }
        public string tags { get; set; }
        public double close { get; set; }
        public List<TimeSery> timeSeries { get; set; }
    }

    public class Root
    {
        public Config config { get; set; }
        public Usage usage { get; set; }
        public List<Datum> data { get; set; }
    }
}
