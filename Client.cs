using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Bank {

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
	public class Client
	{
		public string guid { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public int pin { get; set; }
		public int tries { get; set; } = 3;
		public bool blocked { get; set; } = false;
		public List<string> currencyList { get; set; }
		public List<int> currencyAmount { get; set; }
		public string mainCurrency { get; set; }

        public Client()
		{
			this.currencyList = new List<string>();
			this.currencyAmount = new List<int>();
		}

        public Client(string firstname,string lastname,string mainCurrency) //used to create entirely new client
		{
			this.guid = Guid.NewGuid().ToString();
			this.firstName = firstname;
			this.lastName = lastname;
			this.pin = 0;
			this.mainCurrency = mainCurrency;
			this.currencyList = new List<string>();
			this.currencyAmount = new List<int>();
			this.currencyList.Add(mainCurrency);
			this.currencyAmount.Add(0);
		}

		public Client(Guid guid, string firstname, string lastname, string mainCurrency, int pin) //actually used to copy client
		{
			this.guid = guid.ToString();
			this.firstName = firstname;
			this.lastName = lastname;
			this.mainCurrency = mainCurrency;
			this.pin = pin;
			this.currencyList = new List<string>();
			this.currencyAmount = new List<int>();
		}

		public override String ToString()
		{
			return $"\n{firstName} {lastName}\nGuid:{guid}\nMain currency:{mainCurrency}";
		}

		public async void ViewTotalAmount()
        {
			float total = 0;
			Console.WriteLine(2);
			ConversionData info = new ConversionData();
			for (int i = 1; i<currencyList.Count;i++)
            {
				
				info = await Processor.ReturnConvertInfo(currencyList[i], mainCurrency);
				
				total = info.ConvertRate * currencyAmount[i] + total;
				
			}
			Console.WriteLine("The total money is "+total+" "+mainCurrency);
        }

		public static Client RetrieveMoney(Client c)
        {
			Client.Print(c);
			Console.WriteLine("Pick an account to withdraw from.");
			string read = Console.ReadLine();
            while (!c.currencyList.Contains(read))
            {
				Console.WriteLine("Not a valid account. Pick an existing account to withdraw from.");
				read = Console.ReadLine();
			}
			string account = read;

			Console.WriteLine("Retrieve how much money? Type in a positive integer.");
			int newmoney = 0;
			while (!Int32.TryParse(read,out newmoney) && newmoney > 0)
			{
				Console.WriteLine("Not a valid positive integer. Try again.");
				read = Console.ReadLine();
			}

			c.currencyAmount[c.currencyList.IndexOf(account)] -= newmoney;

			Client.Print(c);

			return c;

		}


        public static Client AddMoney(Client c)
        {
			Client.Print(c);
			Console.WriteLine("Pick an account to add to.");
			string read = Console.ReadLine();
			while (!c.currencyList.Contains(read))
			{
				Console.WriteLine("Not a valid account. Pick an existing account to add to.");
				read = Console.ReadLine();
			}
			string account = read;

			Console.WriteLine("Add how much money? Type in a positive integer.");
			int newmoney = 0;
			while (!Int32.TryParse(read, out newmoney) && newmoney>0)
			{
				Console.WriteLine("Not a valid positive integer. Try again.");
				read = Console.ReadLine();
			}

			c.currencyAmount[c.currencyList.IndexOf(account)] += newmoney;

			Client.Print(c);

			return c;
		}

		public static Client ChangePin(Client c)
        {
			int pin = Admin.AskUserForPIN();
			Console.WriteLine("\n");
			c.pin = pin;
			Console.WriteLine("Pin changed successfully.");
			return c;
		}

		
		public async Task<ConversionData> ExchangeBetweenCurrencies()
        {
			string firstcurrency = Admin.AskForCurrencySymbol();
			string othercurrency = Admin.AskForCurrencySymbol();
			ConversionData info = await Processor.ReturnConvertInfo(firstcurrency, othercurrency);
			return info;

		}

		public static void PrintClientData(Client c) //View GUID and credentials
		{
			Console.WriteLine("Client:");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(c))
			{
				try
				{
					string name = descriptor.Name;
					if (name != "currencyList" && name != "currencyAmount")
					{
						object value = descriptor.GetValue(c);
						Console.WriteLine("{0}= {1}", name, value);
					}

				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public static void PrintAccountData(Client c) //View GUID and credentials
		{
			Console.WriteLine("Client:");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(c))
			{
				try
				{
					string name = descriptor.Name;
					if (name == "currencyList")
					{
						Console.WriteLine("Currency list:");
						for (int i = 0; i < c.currencyList.Count; i++)
						{
							Console.WriteLine($"   - {c.currencyList[i]}: {c.currencyAmount[i]}");
						}
					}else if(name == "guid"){
						object value = descriptor.GetValue(c);
						Console.WriteLine("{0}= {1}", name, value);
					}

				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public static void Print(Client c) //View GUID and credentials
		{
			Console.WriteLine("Client:");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(c))
			{
				try
				{
					string name = descriptor.Name;
					if (name == "currencyList" || name == "currencyAmount")
					{
						if(name== "currencyList")
                        {
							Console.WriteLine("Currency list:");
							for (int i = 0; i < c.currencyList.Count; i++)
							{
								Console.WriteLine($"   - {c.currencyList[i]}: {c.currencyAmount[i]}");
							}
						}
						
					}
					else
					{
						object value = descriptor.GetValue(c);
						Console.WriteLine("{0}= {1}", name, value);
					}
					
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}
	}

	



	
	}





/*
 * Client:
• A GUID (use guid library in c#), GUID is a global unique id
• A first name and last name, not unique
• A pin, unique with the same guid
• List of currencies and amount in each currency (use list)
• Main currency
• Any other important fields for the smooth run of the software

Functionalities:

View GUID and credentials (all except pin!)
• View total amount in preferred currency
• Retrieve money from currency
• Add money to currency
• Change pin
• Exchange between currencies
• Transfer between client (optional)
• Leave message for admin(optional)
*/