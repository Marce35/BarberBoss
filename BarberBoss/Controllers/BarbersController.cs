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
using Microsoft.AspNet.Identity;

namespace BarberBoss.Controllers
{
    public class BarbersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Barbers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Barbers.ToList());
        }

        // GET: Barbers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Appointment> appointments = db.Appointments.Include(a => a.ApplicationUser).Where(a => a.BarberId == id).ToList();
            Barber barber = db.Barbers.Find(id);
            //Barber barber = db.Barbers.Include(b => b.Appointments).FirstOrDefault(b => b.Id == id);
            if (barber == null)
            {
                return HttpNotFound();
            }
            barber.Appointments = appointments;
            return View(barber);
        }

        // GET: Barbers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Barbers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Barber barber)
        {
            if (ModelState.IsValid)
            {
                // Check if an image was uploaded
                if (barber.ImageFile != null)
                {
                    // Save the image and update the ImagePath
                    barber.ImagePath = SaveImageAndGetPath(barber.ImageFile);
                }

                db.Barbers.Add(barber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(barber);
        }

        // GET: Barbers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barber barber = db.Barbers.Find(id);
            if (barber == null)
            {
                return HttpNotFound();
            }
            return View(barber);
        }

        // POST: Barbers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Barber")]
        public ActionResult Edit(Barber barber)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current barber from the database
                var currentBarber = db.Barbers.Find(barber.Id);

                // Check if a new image was uploaded
                if (barber.ImageFile != null)
                {
                    // Delete the old image file
                    if (!string.IsNullOrEmpty(currentBarber.ImagePath))
                    {
                        string oldImagePath = Server.MapPath(currentBarber.ImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image and update the ImagePath
                    currentBarber.ImagePath = SaveImageAndGetPath(barber.ImageFile);
                }

                // Update properties only if they are not null in the new barber model
                currentBarber.Name = barber.Name ?? currentBarber.Name;
                currentBarber.Email = barber.Email ?? currentBarber.Email;
                currentBarber.InstagramProfile = barber.InstagramProfile ?? currentBarber.InstagramProfile;
                currentBarber.FacebookProfile = barber.FacebookProfile ?? currentBarber.FacebookProfile;

                // Save changes
                db.SaveChanges();

                return RedirectToAction("barberProfile");
            }

            return View(barber);
        }

        // GET: Barbers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barber barber = db.Barbers.Find(id);
            if (barber == null)
            {
                return HttpNotFound();
            }
            return View(barber);
        }

        // POST: Barbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Barber barber = db.Barbers.Find(id);
            db.Barbers.Remove(barber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Barber")]
        //[HttpGet, ActionName("listBarberAppointments")]
        [Authorize(Roles = "Barber")]
        public ActionResult listBarberAppointments()
        {
            // Get the current barber's ID
            var currentBarberId = GetCurrentBarberId();

            if (currentBarberId == null)
            {
                return HttpNotFound();
            }

            // Retrieve the barber's appointments
            var barberAppointments = db.Appointments
                .Include(s => s.ServiceOffer)
                .Include(a => a.ApplicationUser)
                .Where(a => a.BarberId == currentBarberId)
                .ToList();

            return View(barberAppointments);
        }

        // Helper method to get the current barber's ID
        [Authorize(Roles = "Barber")]
        private int? GetCurrentBarberId()
        {
            // Get the current user's identity
            var userIdentity = User.Identity;

            // Find the barber associated with the current user
            var currentBarber = db.Barbers.SingleOrDefault(b => b.Email == userIdentity.Name);

            return currentBarber?.Id;
        }
        [HttpGet]
        [Authorize(Roles = "Barber")]
        public ActionResult OfferedServices()
        {
            var currentBarberId = GetCurrentBarberId();
            var barber = db.Barbers.Include(b => b.ServicesOffered).FirstOrDefault(b => b.Id == currentBarberId);

            if (barber == null)
            {
                return HttpNotFound(); // Handle the case where the barber is not found
            }

            // Get all services available for selection
            var allServices = db.ServiceOffers.ToList();

            // Get the services that the barber does not offer
            var availableServices = allServices.Except(barber.ServicesOffered).ToList();

            var viewModel = new OfferedServicesViewModel
            {
                OfferedServices = (List<ServiceOffer>)barber.ServicesOffered,
                AvailableServices = availableServices
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Barber")]
        public ActionResult addServiceToBarber(OfferedServicesViewModel model)
        {
            var currentBarberId = GetCurrentBarberId();
            var barber = db.Barbers.Include(b => b.ServicesOffered).FirstOrDefault(b => b.Id == currentBarberId);

            if (barber == null)
            {
                return HttpNotFound(); // Handle the case where the barber is not found
            }

            // Find the service to be added
            var serviceToAdd = db.ServiceOffers.Find(model.SelectedServiceId);

            if (serviceToAdd != null)
            {
                // Check if the service is not already offered by the barber
                if (!barber.ServicesOffered.Contains(serviceToAdd))
                {
                    barber.ServicesOffered.Add(serviceToAdd);
                    db.SaveChanges();
                }
            }

            // Redirect back to the OfferedServices page
            return RedirectToAction("OfferedServices", "Barbers");

        }

       
        [HttpPost]
        [Authorize(Roles = "Barber")]
        public ActionResult DeleteService(int id)
        {
            var currentBarberId = GetCurrentBarberId();
            var barber = db.Barbers.Include(b => b.ServicesOffered).FirstOrDefault(b => b.Id == currentBarberId);

            if (barber == null)
            {
                return HttpNotFound(); // Handle the case where the barber is not found
            }

            var serviceToDelete = barber.ServicesOffered.FirstOrDefault(s => s.Id == id);

            if (serviceToDelete != null && !HasAppointmentsForService(serviceToDelete, barber.Id))
            {
                barber.ServicesOffered.Remove(serviceToDelete);
                db.SaveChanges();
                return Json(new { canDelete = true });
            }

            return Json(new { canDelete = false });
        }

        private bool HasAppointmentsForService(ServiceOffer service, int barberId)
        {
            // Check if there are appointments for the given service and barber
            return db.Appointments.Any(a => a.ServiceOffer.Id == service.Id && a.Barber.Id == barberId);
        }

        [Authorize(Roles = "Barber")]
        public ActionResult barberProfile()
        {
            var currentBarberId = GetCurrentBarberId();
            Barber barber = db.Barbers.Where(b => b.Id == currentBarberId).FirstOrDefault();

            if(barber == null)
            {
                return HttpNotFound();
            }

            return View(barber);
        }
        [Authorize(Roles = "Barber")]
        public ActionResult EditBarber(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barber barber = db.Barbers.Find(id);
            if (barber == null)
            {
                return HttpNotFound();
            }
            return View(barber);
        }



        // Helper method to save image and return the virtual path
        private string SaveImageAndGetPath(HttpPostedFileBase imageFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

            string imagePath = "~/Pictures/BarbersPictures/" + fileName;

            fileName = Path.Combine(Server.MapPath("~/Pictures/BarbersPictures/"), fileName);
            imageFile.SaveAs(fileName);

            return imagePath;
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
