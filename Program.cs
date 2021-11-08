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
            /*
            Console.WriteLine("Welcome, administrator.\nPlease enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter password:");
            string password = Console.ReadLine();

            while (!Admin.login(username,password))
            {
                Console.WriteLine("Wrong username and/or password. Please try again.");
                Console.WriteLine("\n(Tip: enter admin, password)\nPlease enter username:");
                username = Console.ReadLine();
                Console.WriteLine("Please enter password:");
                password = Console.ReadLine();
            }*/

            Admin admin = new Admin();
            admin.ResetDatabase();


        }

       
 

    }

    
}

