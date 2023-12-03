using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Models;

namespace CurrencyConverter.DAL
{
    public class CurrencyInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CurrencyContext>
    {
        protected override void Seed(CurrencyContext context)
        {
            var currencyCodes = new List<CurrencyCodeModel>
            {
                new CurrencyCodeModel { Code = "USD" },
                new CurrencyCodeModel { Code = "JPY" },
                new CurrencyCodeModel { Code = "BGN" },
                new CurrencyCodeModel { Code = "CZK" },
                new CurrencyCodeModel { Code = "DKK" },
                new CurrencyCodeModel { Code = "GBP" },
                new CurrencyCodeModel { Code = "HUF" },
                new CurrencyCodeModel { Code = "PLN" },
                new CurrencyCodeModel { Code = "RON" },
                new CurrencyCodeModel { Code = "SEK" },
                new CurrencyCodeModel { Code = "CHF" },
                new CurrencyCodeModel { Code = "ISK" },
                new CurrencyCodeModel { Code = "NOK" },
                new CurrencyCodeModel { Code = "TRY" },
                new CurrencyCodeModel { Code = "AUD" },
                new CurrencyCodeModel { Code = "BRL" },
                new CurrencyCodeModel { Code = "CAD" },
                new CurrencyCodeModel { Code = "CNY" },
                new CurrencyCodeModel { Code = "HKD" },
                new CurrencyCodeModel { Code = "IDR" },
                new CurrencyCodeModel { Code = "ILS" },
                new CurrencyCodeModel { Code = "INR" },
                new CurrencyCodeModel { Code = "KRW" },
                new CurrencyCodeModel { Code = "MXN" },
                new CurrencyCodeModel { Code = "MYR" },
                new CurrencyCodeModel { Code = "NZD" },
                new CurrencyCodeModel { Code = "PHP" },
                new CurrencyCodeModel { Code = "SGD" },
                new CurrencyCodeModel { Code = "THB" },
                new CurrencyCodeModel { Code = "ZAR" }
            };

            currencyCodes.ForEach(s => context.CurrencyCodes.Add(s));
            context.SaveChanges();
        }
    }
}
