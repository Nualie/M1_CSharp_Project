using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class ClientJsonAccess : IClientDataAccess
    {
        public Root AllClientJSONData { get; set; } //contains deserialized client JSON data

        public ClientJsonAccess()
        {
            string url = Admin.ReturnDirectory() + "\\Json\\ClientList.json";
            this.AllClientJSONData = Admin.LoadClientJson(url);
        }

        public void CreateUser()
        {
            string firstname = "";
            string lastname = "";
            string mainCurrency = "";

            string read = Admin.AskForCurrencySymbol();
            mainCurrency = read;
            read = "";

            Console.WriteLine("Please enter the new client's first name.");
            read = Console.ReadLine();
            firstname = read;
            read = "";
            Console.WriteLine("Please enter the new client's last name.");
            read = Console.ReadLine();
            lastname = read;

            Client c = new Client(firstname,lastname,mainCurrency);

            CreateUser(c);
        }

        public void CreateUser(Client c)
        {
            AllClientJSONData.Client.Add(c);
            if (UpdateJson())
            {
                Console.WriteLine("Client added successfully.");
            }
        }

        public void DeleteClient(Guid guid)
        {
            int i = 0;
            while (i < AllClientJSONData.Client.Count() && AllClientJSONData.Client[i].guid != guid.ToString())
            {
                i++;
            }
            if (i < AllClientJSONData.Client.Count() ? AllClientJSONData.Client.Remove(AllClientJSONData.Client[i]) : false && UpdateJson())
            {
                Console.WriteLine("Client removed successfully.");
            }
            else
            { 
                Console.WriteLine("No such client.");
            }


            
        }

        public Client returnClientFromGuid(Guid guid)
        {
            int i = 0;
            while (i < AllClientJSONData.Client.Count() && AllClientJSONData.Client[i].guid != guid.ToString())
            {
                i++;
            }
            if (i < AllClientJSONData.Client.Count())
            {
                return AllClientJSONData.Client[i];
            }
            else
            {
                Console.WriteLine("No such client.");
                return null;
            }
        }

        public void ChangePIN(Guid guid )
        {
            Client c = returnClientFromGuid(guid);
            UpdateClient(c);
        }

        public void GetAll()
        {
            GetClientData();
            GetAccountData();
        }
        public void GetClientData()
        {
            for(int i = 0; i<  AllClientJSONData.Client.Count();i++)
            {
                Console.WriteLine($"{i}.");
                Client.PrintClientData(AllClientJSONData.Client[i]);
            }
        }

        public void GetAccountData()
        {
            for (int i = 0; i < AllClientJSONData.Client.Count(); i++)
            {
                Console.WriteLine($"{i}.");
                Client.PrintAccountData(AllClientJSONData.Client[i]);
            }
        }

        public void GetClient(Guid guid)
        {
            Client c = returnClientFromGuid(guid);
            if (c!= null)
            {
                Client.Print(c);
            }
        }

        public bool UpdateJson()
        {
            string json = JsonConvert.SerializeObject(AllClientJSONData.Client.ToArray(),Formatting.Indented);
            string url = Admin.ReturnDirectory() + "\\Json\\ClientList.json";
            try
            {
                System.IO.File.WriteAllText(url, json, Encoding.UTF8);
            }catch(Exception e)
            {
                Console.WriteLine("Failed to save. \n" + e);
                return false;
            }
            return true;
        }

        internal void UnblockClient(Guid guid)
        {
            Client c = returnClientFromGuid(guid);
            c.blocked = false;
            UpdateClient(c);
        }

        internal void BlockClient(Guid guid)
        {
            Client c = returnClientFromGuid(guid);
            c.blocked = false;
            UpdateClient(c);
        }

        public void ResetTries(Guid guid)
        {
            Client c = returnClientFromGuid(guid);
            c.tries = 3;
            UpdateClient(c);
        }

        public Guid getClientGuid(int selectedClient)
        {
            return Guid.Parse(AllClientJSONData.Client[selectedClient].guid);
        }

        public void UpdateClient(Client c)
        {
            int i = 0;
            while (i < AllClientJSONData.Client.Count() && AllClientJSONData.Client[i].guid != c.guid.ToString())
            {
                i++;
            }
            if (i < AllClientJSONData.Client.Count())
            {
                AllClientJSONData.Client[i] = c;
            }
            else
            {
                CreateUser(c); //if client doesnt exist, create it
            }
            if (UpdateJson())
            {
                Console.WriteLine("Client updated successfully.");
            }

        }
    }
}
