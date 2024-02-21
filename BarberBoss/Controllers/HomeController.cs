using BarberBoss.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarberBoss.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Barbers.ToList());
        }

        [AllowAnonymous]
        public ActionResult Services() 
        {
            return View(db.ServiceOffers.ToList());
        }

        [Authorize(Roles = "Admin,Client")]
        public ActionResult ClientAppointmentsList()
        {
            if(User.Identity.IsAuthenticated)
            {
                ViewBag.Message = "Client appointments.";

                // Get the current user's ID
                var currentUserId = User.Identity.GetUserId();

                List<Appointment> appointments = db.Appointments.Include(a => a.ServiceOffer).Include(b => b.Barber).Where(a => a.ApplicationUser.Id == currentUserId).ToList();


                return View(appointments);
            }

            return RedirectToAction("Login", "Account");
            
        }
    }
}