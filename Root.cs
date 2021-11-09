using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
	public class Root
	{
		public List<Client> Client { get; set; }

        public Root()
        {
			this.Client = new List<Client>();
        }

	}
}
