using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Result
    {

		public List<string> ConvertType { get; set; }
		public List<object> ConvertRate { get; set; }

        public Result()
        {
            this.ConvertRate = new List<object>();
            this.ConvertType = new List<string>();
        }
        
        override
        public string ToString()
        {
            string res = "";
            string each = "";
            for(int i = 0; i < this.ConvertType.Count; i++)
            {
                each = this.ConvertType[i] + " : " + this.ConvertRate[i].ToString();
                res = String.Concat(res, each);
            }
            
            return res;
        }


    }
}
