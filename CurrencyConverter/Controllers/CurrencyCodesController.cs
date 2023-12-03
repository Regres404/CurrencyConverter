using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CurrencyConverter.DAL;
using CurrencyConverter.Models;

namespace CurrencyConverter.Controllers
{
    public class CurrencyCodesController : Controller
    {
        private CurrencyContext db = new CurrencyContext();

        // GET: CurrencyCodes
        public ActionResult Index()
        {
            return View(db.CurrencyCodes.ToList());
        }

        // GET: CurrencyCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurrencyCodes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code")] CurrencyCodeModel currencyCode)
        {
            if (ModelState.IsValid)
            {
                db.CurrencyCodes.Add(currencyCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currencyCode);
        }

        // GET: CurrencyCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyCodeModel currencyCode = db.CurrencyCodes.Find(id);
            if (currencyCode == null)
            {
                return HttpNotFound();
            }
            return View(currencyCode);
        }

        // POST: CurrencyCodes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code")] CurrencyCodeModel currencyCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currencyCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currencyCode);
        }

        // GET: CurrencyCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyCodeModel currencyCode = db.CurrencyCodes.Find(id);
            if (currencyCode == null)
            {
                return HttpNotFound();
            }
            return View(currencyCode);
        }

        // POST: CurrencyCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CurrencyCodeModel currencyCode = db.CurrencyCodes.Find(id);
            db.CurrencyCodes.Remove(currencyCode);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
