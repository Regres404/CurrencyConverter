using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Models;

namespace CurrencyConverter.DAL
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext() : base("CurrencyContext")
        {
        }
        public DbSet<CurrencyCodeModel> CurrencyCodes { get; set; }
    }
}
