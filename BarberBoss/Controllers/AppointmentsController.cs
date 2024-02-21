using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BarberBoss.Models;
using Microsoft.AspNet.Identity;

namespace BarberBoss.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Barber);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.BarberId = new SelectList(db.Barbers, "Id", "Name");
            //ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,StartTime,StartTimeString,BookTime,BookTimeString,IsBooked,ServiceId,EndTimeString,ClientId,BarberId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BarberId = new SelectList(db.Barbers, "Id", "Name", appointment.BarberId);
            //ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", appointment.ClientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BarberId = new SelectList(db.Barbers, "Id", "Name", appointment.BarberId);
            //ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", appointment.ClientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,StartTime,StartTimeString,BookTime,BookTimeString,IsBooked,ServiceId,EndTimeString,ClientId,BarberId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BarberId = new SelectList(db.Barbers, "Id", "Name", appointment.BarberId);
            //ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", appointment.ClientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //CUSTOM VIEWS
        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult Calendar()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Calendar");
            }
            return RedirectToAction("Login", "Account");
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult CreateAppointment(string startTime)
        {
            DateTime start = DateTime.Parse(startTime);
            PartialAppointment partial = new PartialAppointment();
            partial.startTime = start;
            partial.startTimeString = start.ToString("yyyy-MM-dd HH:mm:ss");
            partial.bookTime = DateTime.Now;
            partial.bookTimeString = partial.bookTime.ToString("yyyy-MM-dd HH:mm:ss");
            db.PartialAppointments.Add(partial);
            db.SaveChanges();

            return Json(partial.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult BookAppointment(int id)
        {
            
            var partial = db.PartialAppointments.Find(id);

            ViewBag.ServicesIds = new SelectList(db.ServiceOffers, "Id", "Title");

            return View(partial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult BookAppointment(PartialAppointment partialAppointment)
        {
            Appointment appointment = new Appointment();
            Barber barber = db.Barbers.Find(partialAppointment.BarberId);
            

            appointment.StartTime = partialAppointment.startTime;
            appointment.StartTimeString = partialAppointment.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            appointment.BookTime = DateTime.Now;
            appointment.BookTimeString = partialAppointment.bookTime.ToString("yyyy-MM-dd HH:mm:ss");
            appointment.ServiceOffer = db.ServiceOffers.Find(partialAppointment.OfferId);
            appointment.ServiceId = partialAppointment.OfferId;
            appointment.Barber = db.Barbers.Find(partialAppointment.BarberId);
            appointment.BarberId = partialAppointment.BarberId;
            appointment.ApplicationUser = db.Users.Find(User.Identity.GetUserId());
            appointment.IsBooked = true;

            barber.Appointments.Add(appointment);
            

            db.Appointments.Add(appointment);
            db.SaveChanges();
            return RedirectToAction("Calendar");
        }


        //Get JSON DATA


        [AllowAnonymous]
        public JsonResult GetAppointments()
        {
            // Get the current user's ID
            var currentUserId = User.Identity.GetUserId();

            var appointments = db.Appointments.Include(a => a.ServiceOffer).Include(b => b.Barber).Where(a => a.StartTime.CompareTo(DateTime.Now) >= 0 && a.ApplicationUser.Id == currentUserId).ToList();
            List<AppointmentDTO> list = new List<AppointmentDTO>();

            foreach (var app in appointments)
            {
                AppointmentDTO AppointmentDTO = new AppointmentDTO();
                AppointmentDTO.StartTime = app.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                AppointmentDTO.EndTime = app.StartTime.AddMinutes(app.ServiceOffer.Duration).ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                AppointmentDTO.BookTime = app.BookTime != null ? app.BookTime.ToString("yyyy-MM-dd HH:mm:ss") + ".000" : null;
                AppointmentDTO.Price = app.ServiceOffer.Price;
                AppointmentDTO.Title = app.ServiceOffer.Title;
                AppointmentDTO.IsBooked = app.IsBooked;
                AppointmentDTO.Id = app.Id;
                AppointmentDTO.BarberId = app.BarberId;
                AppointmentDTO.BarberName = app.Barber.Name;

                list.Add(AppointmentDTO);
            }

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [AllowAnonymous]
        public JsonResult GetBarbersForService(int serviceId, long selectedTime)
        {
            //// Get the list of barbers offering the selected service
            //var barbersForService = db.Barbers
            //    .Where(barber => barber.ServicesOffered.Any(service => service.Id == serviceId))
            //    .ToList();

            //var barberList = barbersForService.Select(b => new SelectListItem
            //{
            //    Value = b.Id.ToString(),
            //    Text = b.Name
            //}).ToList();

            //return Json(barberList, JsonRequestBehavior.AllowGet);
            // Get the list of barbers offering the selected service

            DateTime selectedDateTime = DateTimeOffset.FromUnixTimeMilliseconds(selectedTime).LocalDateTime;

            var barbersForService = db.Barbers
                .Where(barber =>
                    barber.ServicesOffered.Any(service => service.Id == serviceId)
                )
                .Select(b => new
                {
                    Value = b.Id.ToString(),
                    Text = b.Name,
                    IsAvailable = !b.Appointments.Any(appointment =>
                        appointment.StartTime == selectedDateTime
                    )
                })
                .ToList();

            return Json(barbersForService, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult LeaveReview(int appointmentId, double grade)
        {
            // Find the appointment
            var appointment = db.Appointments.Find(appointmentId);

            // Create a new review
            var review = new Review
            {
                Appointment = appointment,
                Grade = grade
            };

            // Save the review
            db.Reviews.Add(review);
            db.SaveChanges();

            // Optionally, update the appointment's ReviewGrade property
            appointment.ReviewGrade = grade;
            db.SaveChanges();

            return RedirectToAction("ClientAppointmentsList","Home"); // Redirect to the appointments page
        }

        [AllowAnonymous]
        public JsonResult GetAverageReviewGrade()
        {
            var appointmentsWithReviews = db.Appointments.Where(a => a.ReviewGrade.HasValue && a.ReviewGrade > 0).ToList();
            double averageGrade = appointmentsWithReviews.Any() ? appointmentsWithReviews.Average(a => a.ReviewGrade.Value) : 0;

            return Json(new { averageGrade }, JsonRequestBehavior.AllowGet);
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
