using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    
    class Admin
    {
        int selectedClient { get; set; } = -1;
        Processor Process { get; set; } //contains currency data (API is capricious so it has been saved for offline use)
        Root AllClientJSONData { get; set; } //contains deserialized client JSON data
        int ClientNum { get; set; }
        ClientDBAccess sql { get; set; }

        public Admin()
        {
            Process = new Processor();
            string url = ReturnDirectory() +"\\Json\\ClientList.json";
            AllClientJSONData = LoadClientJson(url);
            sql = new ClientDBAccess();
            ClientNum = sql.getClientNumber();
        }

        public void ChangeClientPin()
        {
            
            if (SelectClient())
            {
                sql.ChangePIN(sql.getClientGuid(selectedClient));
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }

        }

        public static string ReturnDirectory()
        {
            string directory = Directory.GetCurrentDirectory();

            int index = directory.LastIndexOf("bin\\"); //get back to main resources directory
            if (index >= 0)
                directory = directory.Substring(0, index);

            return directory;
        }
        public void ListAllClients()
        {
            sql.GetClientData();
        }

        public void DeleteClient()
        {
            if (SelectClient())
            {
                sql.DeleteClient(sql.getClientGuid(selectedClient));
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }
        }

        public void CreateClient()
        {
            sql.CreateUser();
        }

        public void ResetDatabase()
        {
            sql.ResetDatabaseFromJSON();
            sql.GetAll();
        }

        public void ListAllAccounts()
        {
            sql.GetAccountData();
        }

        public bool SelectClient()
        {
            ListAllClients();
            Console.WriteLine("\nEnter the number of the client you want to manage.");

            string read = Console.ReadLine();

            if (Int32.TryParse(read, out int n) && n >= 0 && n<ClientNum)
            {
                selectedClient = n;
                return true;
            }

            return false;
        }

        public static bool login(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string AskForCurrencySymbol()
        {
            Processor process = new Processor();
            foreach (CurrencyListClass.Currency item in process.AllCurrencies.AllCurrencies.Currencies)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Please enter the new client's main currency. Must be a valid ID.");
            string read = Console.ReadLine();
            while (!process.AllCurrencyIds.Contains(read))
            {
                Console.WriteLine("Invalid ID.\nPlease enter the new client's main currency. Must be a valid ID.");
                read = Console.ReadLine();
            }
            return read;
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
}/*
• Create a client
• Manage a client (unblock, block, change pin, reset tries, delete client….)
• Verify user transactions(optional)
• View a list of all users

  */