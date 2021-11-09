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
        int ClientNum { get; set; }
        ClientDBAccess sql { get; set; }
        ClientJsonAccess json { get; set; }

        public bool DBaccess = false;

        string username = "admin";

        string password = "password";

        public Admin()
        {
            sql = new ClientDBAccess();
            json = new ClientJsonAccess();
            ClientNum = 2; //json.AllClientJSONData.Client.Count();
        }

        public bool RequestDBAccess()
        {
            return this.DBaccess = sql.CheckVersion();
        }

        public static int AskUserForPIN()
        {
            Console.WriteLine("What to change PIN to?");
            string read = Console.ReadLine();
            int pin = 0;
            while (!(Int32.TryParse(read, out pin) && pin >= 0 && pin < 10000))
            {
                Console.WriteLine($"{read} is an invalid PIN. What to change PIN to?");
                read = Console.ReadLine();
            }
            return pin;
        }

        public void Menu()
        {
            string choice = "0";
            Console.WriteLine("Welcome, Administrator.");
            while (choice != "12")
            {

            Console.WriteLine($"\nDatabase access: {DBaccess}\nType in an action id.");
            Console.WriteLine("\n0.Request SQLite database access.\n1.View all clients.\n2.Create a client.\n3.Delete a client." +
                "\n4.Change a client's PIN.\n5.List all accounts by GUID\n6.Reset the database from JSON\n7.Reset JSON from database"+
                "\n8.Reset client tries\n9.Block client\n10.Unblock client\n11.Turn off database access\n12.Quit\n");
            choice = Console.ReadLine();
            Console.WriteLine();
            switch (choice)
            {
                case "0":
                    RequestDBAccess();
                        break;
                case "1":
                    ListAllClients();
                    break;
                case "2":
                    CreateClient();
                    break;
                case "3":
                    DeleteClient();
                    break;
                case "4":
                    ChangeClientPin();
                    break;
                case "5":
                    ListAllAccounts();
                    break;
                case "6":
                    ResetDatabase();
                    break;
                case "7":
                    ResetJson();
                    break;
                case "8":
                    ResetTries();
                    break;
                case "9":
                    BlockClient();
                    break;
                case "10":
                    UnblockClient();
                    break;
                case "11":
                    DBaccess=false;
                    break;
                case "12":
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
                }
            }
        }

        private void UnblockClient()
        {
            if (SelectClient())
            {
                if (DBaccess)
                {
                    sql.UnblockClient(sql.GetClientGuid(selectedClient));
                }
                else
                {
                    json.UnblockClient(json.getClientGuid(selectedClient));
                }
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }
        }

        private void BlockClient()
        {
            if (SelectClient())
            {
                if (DBaccess)
                {
                    sql.BlockClient(sql.GetClientGuid(selectedClient));
                }
                else
                {
                    json.BlockClient(json.getClientGuid(selectedClient));
                }
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }
        }

        public void ChangeClientPin()
        {
            if (SelectClient())
            {
                if (DBaccess)
                {
                    sql.ChangePIN(sql.GetClientGuid(selectedClient));
                }
                else
                {
                    json.ChangePIN(json.getClientGuid(selectedClient));
                }
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }

        }

        public void ResetTries()
        {
            if (SelectClient())
            {
                if (DBaccess)
                {
                    sql.ResetTries(sql.GetClientGuid(selectedClient));
                }
                else
                {
                    json.ResetTries(json.getClientGuid(selectedClient));
                }
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
            if (DBaccess)
            {
                sql.GetClientData();
            }
            else
            {
                json.GetClientData(); 
            }
            Console.WriteLine();
        }

        public void DeleteClient()
        {
            if (SelectClient())
            {
                if (DBaccess)
                {
                    sql.DeleteClient(sql.GetClientGuid(selectedClient));
                }
                else
                {
                    json.DeleteClient(json.getClientGuid(selectedClient));
                }
            }
            else
            {
                Console.WriteLine("Invalid client number.");
            }
        }

        public void CreateClient()
        {
            if (DBaccess)
            {
                sql.CreateUser();
            }
            else
            {
                json.CreateUser();
            }
        }

        public void ResetDatabase()
        {
            if (DBaccess)
            {
                sql.ResetDatabaseFromJSON();
                sql.GetAll();
            }
            else
            {
                Console.WriteLine("This command requires database access.");
            }
        }

        public void ResetJson()
        {
            if (DBaccess) 
            { 
                json.AllClientJSONData = sql.GetRootFromDatabase();
                json.UpdateJson();
                json.GetAll();
            }
            else
            {
                Console.WriteLine("This command requires database access.");
            }
        }

        public void ListAllAccounts()
        {
            if (DBaccess)
            {
                sql.GetAccountData();
            }
            else
            {
                json.GetAccountData();
            }
        }

        public bool SelectClient()
        {
            ListAllClients();
            Console.WriteLine("\nEnter the number of the client you want to manage.");

            string read = Console.ReadLine();

            if (DBaccess)
            {
                ClientNum = sql.UpdateClientNumber();
            }
            else
            {
                ClientNum = json.UpdateClientNumber();
            }

            if (Int32.TryParse(read, out int n) && n >= 0 && n<ClientNum)
            {
                selectedClient = n;
                return true;
            }

            return false;
        }

        public bool login(string username, string password)
        {
            if (username == this.username && password == this.password)
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