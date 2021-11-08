using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class ClientDBAccess : IClientDataAccess
    {

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
            firstname TEXT, lastname TEXT, pin INT, mainCurrency TEXT, blocked BOOL)";
            cmd.ExecuteNonQuery();

            //Console.WriteLine("clients table created");

            Root info = Admin.LoadClientJson(directory + "\\Json\\ClientList.json");
            for(int i = 0; i < info.Client.Count; i++)
            {
                cmd.CommandText = $"INSERT INTO clients(guid, firstname, lastname, pin, mainCurrency, blocked) VALUES('{info.Client[i].guid}','{info.Client[i].firstname}','{info.Client[i].lastname}',{info.Client[i].pin},'{info.Client[i].mainCurrency}',false)";
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = @"CREATE TABLE accounts(guid GUID,
            currency TEXT, amount FLOAT, PRIMARY KEY(guid, currency))";
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

            int pin = AskUserForPIN();
            string directory = Admin.ReturnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";
            Console.WriteLine("\n");
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = $"SELECT * FROM clients WHERE guid='{guid}'";
            using var cmd = new SQLiteCommand(stm, con);

            cmd.CommandText = $"UPDATE clients SET pin={pin} WHERE guid='{guid}'";
            cmd.ExecuteNonQuery();

            GetClient(guid);

            Console.WriteLine("Pin changed successfully.");

        }

        public int AskUserForPIN()
        {
            Console.WriteLine("What to change PIN to?");
            string read = Console.ReadLine();
            int pin = 0;
            while(!(Int32.TryParse(read, out pin) && pin >= 0 && pin<10000))
            {
                Console.WriteLine($"{read} is an invalid PIN. What to change PIN to?");
                read = Console.ReadLine();
            }
            return pin;
        }

        public Guid getClientGuid(int n)
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

        public int getClientNumber()
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
                Console.WriteLine($"{num}.\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetFloat(2)}");
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
                Console.WriteLine($"{num}.\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetString(2)}\nPin: {rdr.GetInt32(3)}\nMain currency: {rdr.GetString(4)}\nBlocked:{rdr.GetBoolean(5)}");
                num++;
            }
            rdr.Close();
            con.Close();
        }

        public void CheckVersion()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"SQLite version: {version}");
            con.Close();
        }

        public void GetAll()
        {
            GetClientData();
            GetAccountData();
        }

        public void CreateUser()
        {
            Guid guid = new Guid();
            string firstname = "";
            string lastname = "";
            //etc
            //TODO
            throw new NotImplementedException();
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
                Console.WriteLine($"Client selected:\n{rdr.GetGuid(0)} {rdr.GetString(1)} {rdr.GetString(2)}\nPin: {rdr.GetInt32(3)}\nMain currency: {rdr.GetString(4)}\nBlocked:{rdr.GetBoolean(5)}");
                
            }
        }

        public void UpdateClient(Client c)
        {
            //MAYBE SCRAP FOR ADMIN? OR USE TO UPDATE FROM JSON
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
