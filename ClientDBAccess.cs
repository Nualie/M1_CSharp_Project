using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bank.CurrencyListClass;

namespace Bank
{
    class ClientDBAccess : IClientDataAccess
    {

        public Root LoadClientsForJSON(Root newData)
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();
            string stm = $"SELECT * FROM clients ORDER BY guid";
            using var cmd = new SQLiteCommand(stm, con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                newData.Client.Add(new Client(rdr.GetGuid(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(4), rdr.GetInt32(3)));
            }
            rdr.Close();
            return newData;
        }

        public Client LoadAccountsForJSON(Client c)
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM accounts WHERE guid='{c.guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                c.currencyList.Add(rdr.GetString(1));
                c.currencyAmount.Add(rdr.GetInt32(2));
            }
            rdr.Close();
            return c;
        }

        public Root GetRootFromDatabase() //use from Admin, to reset json from database
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();
            string stm = $"SELECT * FROM clients ORDER BY guid";
            using var cmd = new SQLiteCommand(stm, con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            Root newData = new Root();

            newData = LoadClientsForJSON(newData);

            for(int i = 0; i< newData.Client.Count();i++)
            {
                 newData.Client[i]= LoadAccountsForJSON(newData.Client[i]);
            }

            return newData;

        }

        public int UpdateClientNumber()
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();
            string stm = $"SELECT * FROM clients ORDER BY guid";
            using var cmd = new SQLiteCommand(stm, con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int total = 0;

            while (rdr.Read())
            {
                total++;    
            }

            return total;
        }


        public void ResetDatabaseFromJSON()
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "DROP TABLE IF EXISTS clients";
            cmd.ExecuteNonQuery(); 
            cmd.CommandText = "DROP TABLE IF EXISTS accounts";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE clients(guid GUID PRIMARY KEY,
            firstName TEXT, lastName TEXT, pin INT, mainCurrency TEXT, tries INT, blocked BOOL)";
            cmd.ExecuteNonQuery();

            

            Root info = Admin.LoadClientJson(directory + "\\Json\\ClientList.json");
            for(int i = 0; i < info.Client.Count; i++)
            {
                cmd.CommandText = $"INSERT INTO clients(guid, firstName, lastName, pin, mainCurrency, tries, blocked) VALUES('{info.Client[i].guid}','{info.Client[i].firstName}','{info.Client[i].lastName}',{info.Client[i].pin},'{info.Client[i].mainCurrency}',{info.Client[i].tries},{info.Client[i].blocked})";
                Console.WriteLine("blocked check:"+info.Client[i].blocked);
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = @"CREATE TABLE accounts(guid GUID,
            currency TEXT, amount INT, PRIMARY KEY(guid, currency))";
            cmd.ExecuteNonQuery();

            //Console.WriteLine("accounts table created");

            for (int i = 0; i < info.Client.Count; i++)
            {
                for(int j = 0; j <info.Client[i].currencyList.Count; j++)
                {
                cmd.CommandText = $"INSERT INTO accounts(guid, currency, amount) VALUES('{info.Client[i].guid}','{info.Client[i].currencyList[j]}','{info.Client[i].currencyAmount[j]}')";
                cmd.ExecuteNonQuery();
                }
            }

        }

        public void ChangePIN(Guid guid)
        {
            GetClient(guid);

            int pin = Admin.AskUserForPIN();

            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";
            
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"UPDATE clients SET pin={pin} WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            GetClient(guid);

            Console.WriteLine("Pin changed successfully.");

        }

        internal void BlockClient(Guid guid)
        {
            GetClient(guid);

            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"UPDATE clients SET blocked=true WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            GetClient(guid);

            Console.WriteLine("Client blocked successfully.");
        }

        internal void UnblockClient(Guid guid)
        {
            GetClient(guid);

            int pin = Admin.AskUserForPIN();
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"UPDATE clients SET blocked=false WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            GetClient(guid);

            Console.WriteLine("Client unblocked successfully.");
            ResetTries(guid);
        }

        internal void ResetTries(Guid guid)
        {
            GetClient(guid);

            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"UPDATE clients SET tries=3 WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            GetClient(guid);

            Console.WriteLine("Tries reset successfully.");
        }

        public Guid GetClientGuid(int n)
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT guid FROM clients ORDER BY guid";

            using var cmd = new SQLiteCommand(stm, con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            for(int i = 0; i<=n; i++)
            {
                rdr.Read(); 
            }

            Console.WriteLine($"Selected client {rdr.GetGuid(0)}");
            Guid guid =rdr.GetGuid(0);
            rdr.Close();
            return guid;
        }

        public int GetClientNumber()
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT COUNT(*) FROM clients ORDER BY guid";
            using var cmd = new SQLiteCommand(stm, con);
            int RowCount = 0;

            RowCount = Convert.ToInt32(cmd.ExecuteScalar());
            
            con.Close();
            return RowCount;

        }

        public void GetAccountData()
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM accounts ORDER BY guid";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int num = 0;
            while (rdr.Read())
            {
                Console.WriteLine($"{num}.\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetInt32(2)}");
                num++;
            }
            rdr.Close();
            con.Close();
        }

        public void GetClientData()
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM clients ORDER BY guid";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int num = 0;
            while (rdr.Read())
            {
                Console.WriteLine($"{num}.\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetString(2)}\nPin: {rdr.GetInt32(3)}\nMain currency: {rdr.GetString(4)}\nTries:{rdr.GetInt32(5)}\nBlocked:{rdr.GetBoolean(6)}");
                num++;
            }
            rdr.Close();
            con.Close();
        }

        public bool CheckVersion()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            try
            {
                using var con = new SQLiteConnection(cs);
                con.Open();

                using var cmd = new SQLiteCommand(stm, con);
                string version = cmd.ExecuteScalar().ToString();

                Console.WriteLine($"SQLite version: {version}");
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public void GetAll()
        {
            GetClientData();
            GetAccountData();
        }

        public void CreateUser(Client c) //from Client
        {
            Guid guid = Guid.Parse(c.guid);
            string firstname = c.firstName;
            string lastname = c.lastName;
            int pin = c.pin;
            string mainCurrency = c.mainCurrency;
            List<string> currencyList = c.currencyList;
            List<int> currencyAmount = c.currencyAmount;

            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM clients ORDER BY guid";

            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"INSERT INTO clients(guid, firstName, lastName, pin, mainCurrency, tries, blocked) VALUES('{guid}','{firstname}','{lastname}',{pin},'{mainCurrency}',3,false)";
            cmd.ExecuteNonQuery();

            for(int i = 0; i<c.currencyList.Count(); i++)
            {
                cmd.CommandText = $"INSERT INTO accounts(guid, currency, amount) VALUES('{guid}','{currencyList[i]}',{currencyAmount[i]})";
                cmd.ExecuteNonQuery();
            }
            

            GetAll();
        }

        public void CreateUser()
        {
            Guid guid = Guid.NewGuid();
            string firstname = "";
            string lastname = "";
            int pin = 0;
            string mainCurrency = "";
            string[] currencyList = {};
			int[] currencyAmount = {0};

            string read = Admin.AskForCurrencySymbol();
            mainCurrency = read;
            currencyList.Append(read);
            read = "";

            Console.WriteLine("Please enter the new client's first name.");
            read = Console.ReadLine();
            firstname = read;
            read = "";
            Console.WriteLine("Please enter the new client's last name.");
            read = Console.ReadLine();
            lastname = read;

            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM clients ORDER BY guid";

            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"INSERT INTO clients(guid, firstName, lastName, pin, mainCurrency, tries, blocked) VALUES('{guid}','{firstname}','{lastname}',{pin},'{mainCurrency}',3,false)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = $"INSERT INTO accounts(guid, currency, amount) VALUES('{guid}','{mainCurrency}',0)";
            cmd.ExecuteNonQuery();

            GetAll();
        }

        public void GetClient(Guid guid)
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";
            
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                rdr.Read();
                Console.WriteLine($"\nClient selected:\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetString(2)}\nPin: {rdr.GetInt32(3)}\nMain currency: {rdr.GetString(4)}\nTries: {rdr.GetInt32(5)}\nBlocked: {rdr.GetBoolean(6)}");
                
            }
        }

        public void UpdateClient(Client c) //updated client goes in
        {
            if (Guid.TryParse(c.guid, out Guid guid))
            {
                DeleteClient(guid);
                CreateUser(c);
            }
            else
            {
                Console.WriteLine("Something went wrong. Invalid guid.");
            }
            

        }

        public void DeleteClient(Guid guid)
        {
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";
            Console.WriteLine("\n");
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            GetClient(guid);

            cmd.CommandText = $"DELETE FROM clients WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            cmd.CommandText = $"SELECT * FROM accounts WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"DELETE FROM accounts WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            Console.WriteLine("\nNew tables:");
            GetClientData();
            GetAccountData();
            


        }
    }
}
