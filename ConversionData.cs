using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class ConversionData
    {

        public string ConvertType { get; set; }
		public float ConvertRate { get; set; }

        public ConversionData()
        {
            this.ConvertRate = 1;
            this.ConvertType = "ALL";
        }

       

        public override string ToString()
        {
            string res = "";
            string each = "";
            each = this.ConvertType + " : " + this.ConvertRate.ToString();
            res = String.Concat(res, each);

            return res;
        }


    }
}
