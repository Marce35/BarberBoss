using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BarberBoss.Models;

namespace BarberBoss.Controllers
{
    public class ServiceOffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceOffers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.ServiceOffers.ToList());
        }

        // GET: ServiceOffers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOffer serviceOffer = db.ServiceOffers.Find(id);
            if (serviceOffer == null)
            {
                return HttpNotFound();
            }
            return View(serviceOffer);
        }

        // GET: ServiceOffers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ServiceOffer serviceOffer)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(serviceOffer.ImageFile.FileName);
                string extension = Path.GetExtension(serviceOffer.ImageFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                serviceOffer.ImagePath = "~/Pictures/OfferPictures/" + fileName;

                fileName = Path.Combine(Server.MapPath("~/Pictures/OfferPictures/"), fileName);
                serviceOffer.ImageFile.SaveAs(fileName);


                db.ServiceOffers.Add(serviceOffer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceOffer);
        }

        // GET: ServiceOffers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOffer serviceOffer = db.ServiceOffers.Find(id);
            if (serviceOffer == null)
            {
                return HttpNotFound();
            }
            return View(serviceOffer);
        }

        // POST: ServiceOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ServiceOffer serviceOffer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceOffer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceOffer);
        }

        // GET: ServiceOffers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOffer serviceOffer = db.ServiceOffers.Find(id);
            if (serviceOffer == null)
            {
                return HttpNotFound();
            }
            return View(serviceOffer);
        }

        // POST: ServiceOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceOffer serviceOffer = db.ServiceOffers.Find(id);
            db.ServiceOffers.Remove(serviceOffer);
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
