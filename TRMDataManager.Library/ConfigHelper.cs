using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal taxRate);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("This value was an invalid tax rate.");
            }

            return taxRate;
        }
    }
}
