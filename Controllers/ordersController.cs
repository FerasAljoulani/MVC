using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon.Models;
using System.IO;

namespace Amazon.Controllers
{
    public class ordersController : Controller
    {
        private amazonDBEntities1 db = new amazonDBEntities1();

        // GET: orders
        [HttpGet]
        public ActionResult Index()
        {
            var orders = db.orders.Include(o => o.item);
            return View(orders.ToList());
        }
        [HttpPost]
        public ActionResult Index(string cname,order order)
        {
            var orders = db.orders.ToList().Where(m => m.cname.StartsWith(cname));
            return View(orders);
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.items, "Id", "iname");
            return View();
        }

        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ono,cname,phone,adress,email,gender,image,shipping,ship_price,date_order,id")] order order, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                order.image = "~/images/client/n.jpg";
                db.orders.Add(order);
                db.SaveChanges();
                string i = "";
                if (imgFile != null)
                {
                    i = "~/images/client/" + order.ono + ".jpg";
                    imgFile.SaveAs(Server.MapPath(i));
                    order.image = i;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.items, "Id", "iname", order.id);
            return View(order);
        }

        // GET: orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.items, "Id", "iname", order.id);
            return View(order);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ono,cname,phone,adress,email,gender,image,shipping,ship_price,date_order,id")] order order,HttpPostedFileBase imgFile)
        {
            var originalRec = db.orders.Where(x => x.ono == order.ono).ToList().FirstOrDefault();
            string i = "";
            if (imgFile != null)
            {
                i = "~/images/client/" + order.ono + ".jpg";
                imgFile.SaveAs(Server.MapPath(i));
                order.image = i;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.id = new SelectList(db.items, "Id", "iname", order.id);
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
        public int Count()
        {
            return db.orders.ToList().Count;
        }
    }
}
