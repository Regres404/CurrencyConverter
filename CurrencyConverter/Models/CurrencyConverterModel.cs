using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Models
{
    public class CurrencyConverterModel
    {
        [Required(ErrorMessage = "Currency From is required.")]
        public string CurrencyFrom { get; set; }

        [Required(ErrorMessage = "Currency To is required.")]
        public string CurrencyTo { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }

        public decimal? ConvertedAmount { get; set; }
    }
}
