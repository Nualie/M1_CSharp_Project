using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class SQLiteManager
    {

        public static void test()
        {
            string directory = Program.returnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "DROP TABLE IF EXISTS clients";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE clients(guid TEXT PRIMARY KEY,
            firstname TEXT, lastname TEXT, pin INT, mainCurrency TEXT)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("table created");

            Root info = Program.LoadClientJson(directory + "\\Json\\ClientList.json");
            for(int i = 0; i < info.Client.Count; i++)
            {
                cmd.CommandText = $"INSERT INTO clients(guid, firstname, lastname, pin, mainCurrency) VALUES('{info.Client[i].guid}','{info.Client[i].firstname}','{info.Client[i].lastname}',{info.Client[i].pin},'{info.Client[i].mainCurrency}')";
                cmd.ExecuteNonQuery();
            }
            //TODO: EACH CURRENCY THINGIES (SECOND DATABASE TORESPECT NF?)
            ReadData();

        }

        public static void ReadData()
        {
            string directory = Program.returnDirectory();
            string cs = $@"URI=file:{directory}\\Database\\Client.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM clients LIMIT 5";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetString(0)} {rdr.GetString(1)} {rdr.GetString(2)} {rdr.GetInt32(3)} {rdr.GetString(4)}");
            }
        }

        public static void CheckVersion()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"SQLite version: {version}");
        }

    }
}
