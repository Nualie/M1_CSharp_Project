using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Bank {

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
	public class Client
	{
		public string guid { get; set; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public int pin { get; set; }
		public List<string> currencyList { get; set; }
		public List<int> currencyAmount { get; set; }
		public string mainCurrency { get; set; }

		public override String ToString()
		{
			return $"\n{firstname} {lastname}\nGuid:{guid}\nMain currency:{mainCurrency}";
		}

		public async void ViewTotalAmount()
        {
			float total = 0;
			Console.WriteLine(2);
			Result info = new Result();
			for (int i = 1; i<currencyList.Count;i++)
            {
				
				info = await Processor.ReturnConvertInfo(currencyList[i], mainCurrency);
				
				total = info.ConvertRate * currencyAmount[i] + total;
				
			}
			Console.WriteLine("hello");
			Console.WriteLine("Your total money is "+total+" "+mainCurrency);
        }

		public void RetrieveMoney()
        {

        }

		public void AddMoney()
        {

        }

		public void ChangePIN()
        {

        }

		public async Task ExchangeBetweenCurrencies()
        {
			string firstcurrency = "USD";
			string othercurrency = "EUR";
			Result info = await Processor.ReturnConvertInfo(firstcurrency, othercurrency);

		}

		public void Print() //View GUID and credentials
		{
			Console.WriteLine("Client:");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
			{
				try
				{
					string name = descriptor.Name;
					if (name == "currencyList" || name == "currencyAmount")
					{
						if(name== "currencyList")
                        {
							Console.WriteLine("Currency list:");
							for (int i = 0; i < currencyList.Count; i++)
							{
								Console.WriteLine($"   - {currencyList[i]}: {currencyAmount[i]}");
							}
						}
						
					}
					else
					{
						object value = descriptor.GetValue(this);
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