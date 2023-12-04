using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;
using CurrencyConverter.DAL;
using CurrencyConverter.Models;

namespace CurrencyConverter.Controllers
{
    public class CurrencyController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // URL for loading XML currency exchange rate data from the European Central Bank
        protected readonly string _ecbUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

        /// <summary>
        /// GET method to display the currency exchange rate page.
        /// </summary>
        [HttpGet]
        public ActionResult CurrencyRate()
        {
            try
            {
                List<CurrencyCodeModel> currencies = GetCurrencyCodes();
                ViewBag.AvailableCurrencies = new SelectList(currencies, "Code", "Code");
                List<string> selectedCurrencies = CreateCurrencyList();
                EnvelopeModel envelopeModel = GetEcbEnvelopeData(null, null, selectedCurrencies);

                return View(envelopeModel);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return View("Error");
            }
        }

        /// <summary>
        /// POST method to handle the form submission with selected currencies and dates.
        /// </summary>
        [HttpPost]
        public ActionResult CurrencyRate(DateTime? fromDate, DateTime? toDate, string selectedCurrency)
        {
            try
            {
                EnvelopeModel envelopeModel = new EnvelopeModel();

                List<string> validationErrors = ValidateDates(fromDate, toDate);

                if (validationErrors.Any())
                {
                    TempData["ValidationErrors"] = validationErrors;
                    return RedirectToAction("CurrencyRate");
                }

                List<CurrencyCodeModel> currencies = GetCurrencyCodes();
                ViewBag.AvailableCurrencies = new SelectList(currencies, "Code", "Code");
                List<string> selectedCurrencies = CreateCurrencyList(selectedCurrency);
                envelopeModel = GetEcbEnvelopeData(fromDate, toDate, selectedCurrencies);

                return View(envelopeModel);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return View("Error");
            }
        }

        /// <summary>
        /// Method to validate entered dates.
        /// </summary>
        public List<string> ValidateDates(DateTime? fromDate, DateTime? toDate)
        {
            List<string> validationErrors = new List<string>();

            if (fromDate == null)
            {
                validationErrors.Add("Date From is Required");
            }

            if (toDate == null)
            {
                validationErrors.Add("Date To is Required");
            }

            if (fromDate > toDate)
            {
                validationErrors.Add("Wrong date period");
            }

            return validationErrors;
        }

        /// <summary>
        /// Get all currencies from the database
        /// </summary>
        public List<CurrencyCodeModel> GetCurrencyCodes()
        {
            List<CurrencyCodeModel> currencies;
            using (var db = new CurrencyContext())
            {

                currencies = db.CurrencyCodes.ToList();
            }

            return currencies;
        }

        /// <summary>
        /// Method to get currency exchange rate data using XML.
        /// </summary>
        private EnvelopeModel GetEcbEnvelopeData(DateTime? fromDate, DateTime? toDate, List<string> selectedCurrencies)
        {
            return LoadXmlData(_ecbUrl, fromDate, toDate, selectedCurrencies);
        }

        /// <summary>
        /// GET method to display the currency conversion page.
        /// </summary>
        [HttpGet]
        public ActionResult CurrencyConverter()
        {
            try
            {
                List<CurrencyCodeModel> currencies = GetCurrencyCodes();
                ViewBag.AvailableCurrencies = new SelectList(currencies, "Code", "Code");
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return View("Error");
            }
            
        }

        /// <summary>
        /// POST method to handle currency conversion and return the result in JSON format.
        /// </summary>
        [HttpPost]
        public JsonResult CurrencyConverter(CurrencyConverterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    log.Info($"Attempt to convert {model.Amount} {model.CurrencyFrom} to {model.CurrencyTo}");
                    DateTime? specifiedDate = model.Date;

                    List<string> selectedCurrencies = CreateCurrencyList(model.CurrencyFrom, model.CurrencyTo);

                    EnvelopeModel envelope = GetEcbEnvelopeData(specifiedDate, specifiedDate, selectedCurrencies);

                    decimal? currencyFromRate = envelope.Cube.FirstOrDefault()?.Cubes.Find(x => x.Currency == model.CurrencyFrom)?.Rate;
                    decimal? currencyToRate = envelope.Cube.LastOrDefault()?.Cubes.Find(x => x.Currency == model.CurrencyTo)?.Rate;

                    decimal result = 0;
                    if (currencyFromRate != null && currencyFromRate != null)
                    {
                        result = model.Amount / currencyFromRate.Value * currencyToRate.Value;
                    }

                    model.Amount = result;

                    return Json(new { Result = result });
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new { Errors = errors });
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return Json(new { Errors = new List<string>{ "Something went wrong" }});
            }
            
        }

        /// <summary>
        /// Method to create a list of selected currencies based on the provided parameters.
        /// </summary>
        private List<string> CreateCurrencyList(params string[] currencies)
        {
            List<string> selectedCurrencies = new List<string>();

            foreach (var currency in currencies)
            {
                if (!string.IsNullOrEmpty(currency))
                {
                    selectedCurrencies.Add(currency);
                }
            }

            return selectedCurrencies;
        }

        /// <summary>
        /// Method to load XML data.
        /// </summary>
        private EnvelopeModel LoadXmlData(string url, DateTime? fromDate, DateTime? toDate,
            List<string> selectedCurrencies)
        {
            XDocument xmlDocument = XDocument.Load(url);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EnvelopeModel));

            var reader = xmlDocument.Root.CreateReader();
            var cubes = (EnvelopeModel)xmlSerializer.Deserialize(reader);

            var envelopeModel = new EnvelopeModel
            {
                Cube = cubes.Cube
                    .Where(cube => (!fromDate.HasValue || cube.Time.Date >= fromDate.Value.Date)
                                   && (!toDate.HasValue || cube.Time.Date <= toDate.Value.Date)
                    )
                    .Select(cube => new CubeModel
                    {
                        Time = cube.Time.Date,
                        Cubes = cube.Cubes
                            .Where(cubeItem =>
                                !selectedCurrencies.Any() || selectedCurrencies.Contains(cubeItem.Currency))
                            .Select(cubeItem => new CubeItemModel
                            {
                                Rate = cubeItem.Rate,
                                Currency = cubeItem.Currency
                            })
                            .ToList()
                    })
                    .ToList()
            };
            return envelopeModel;
        }
    }
}