using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237Assignment6.Models;

namespace cis237Assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageESchoberEntities db = new BeverageESchoberEntities();

        // GET: Beverages
        public ActionResult Index()
        {
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            string filterName = "";
            string filterPack = "";
            string filterMinPrice = "";
            string filterMaxPrice = "";

            //Define the default min and max price value
            decimal minPrice = 0;
            decimal maxPrice = Decimal.MaxValue; 
            //This defines the maximum value that can be stored by an decimal variable.


            if (Session["name"] != null && !string.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }

            if (Session["pack"] != null && !string.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                filterPack = (string)Session["pack"];
            }

            if (Session["minPrice"] != null && !string.IsNullOrWhiteSpace((string)Session["minPrice"]))
            {
                filterMinPrice = (string)Session["minPrice"];
                minPrice = Decimal.Parse(filterMinPrice);
            }

            if (Session["maxPrice"] != null && !string.IsNullOrWhiteSpace((string)Session["maxPrice"]))
            {
                filterMaxPrice = (string)Session["maxPrice"];
                maxPrice = Decimal.Parse(filterMaxPrice);
            }

            //Filter the BeveragesToFilter Dataset using a lambda expression including the input values.
            IEnumerable<Beverage> filtered = BeveragesToFilter.Where(beverage => beverage.price >= minPrice &&
                                                      beverage.price <= maxPrice &&
                                                      beverage.name.Contains(filterName) &&
                                                      beverage.pack.Contains(filterPack));

            //Convert the filtered dataset to a list.
            IEnumerable<Beverage> finalFiltered = filtered.ToList();


            ViewBag.filterName = filterName;
            ViewBag.filterPack = filterPack;
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

            return View(finalFiltered);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            string id = beverage.id.ToString();
            if (db.Beverages.Find(id) == null)
            {
                if (ModelState.IsValid)
                {
                    db.Beverages.Add(beverage);
                    db.SaveChanges();
                    ViewBag.ErrorMessage = "";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ErrorMessage = "A beverage with this ID already exists!";
            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            string name = Request.Form.Get("name");
            string pack = Request.Form.Get("pack");
            string minPrice = Request.Form.Get("minPrice");
            string maxPrice = Request.Form.Get("maxPrice");

            Session["name"] = name;
            Session["pack"] = pack;
            Session["minPrice"] = minPrice;
            Session["maxPrice"] = maxPrice;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetFilter()
        {
            string name = "";
            string pack = "";
            string minPrice = "";
            string maxPrice = "";

            Session["name"] = name;
            Session["pack"] = pack;
            Session["minPrice"] = minPrice;
            Session["maxPrice"] = maxPrice;

            return RedirectToAction("Index");
        }

        public ActionResult Json()
        {
            return Json(db.Beverages.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
