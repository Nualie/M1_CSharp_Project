using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bank {

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
	public class Client
	{
		public string _guid { get; set; }
		public string _firstname { get; set; }
		public string _lastname { get; set; }
		public int _pin { get; set; }
		public List<string> currencyList { get; set; }
		public string _mainCurrency { get; set; }

		public override String ToString()
		{
			return $"\n{_firstname} {_lastname}\nGuid:{_guid}\nMain currency:{_mainCurrency}";
		}
		public void print()
		{
			Console.WriteLine("Client:\n");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
			{
				try
				{
					string name = descriptor.Name;
					object value = descriptor.GetValue(this);
					Console.WriteLine("{0}={1}", name, value);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}
	}

	public class Root
	{
		public List<Client> Client { get; set; }
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
*/