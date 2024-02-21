using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace BarberBoss.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Time of appointment")]
        public DateTime StartTime { get; set; }
        public string StartTimeString { get; set; }

        [DisplayName("Time of booking")]
        public DateTime BookTime { get; set; }
        public string BookTimeString { get; set; }

        [DisplayName("Booked")]
        public bool IsBooked { get; set; }

        public int ServiceId { get; set; }
        public ServiceOffer ServiceOffer { get; set; }

        //[DisplayName("End Time")]
        //public DateTime EndTime
        //{
        //    get
        //    {
        //        if (Services != null && Services.Any())
        //        {
        //            // Calculate the total duration of all services
        //            int totalDuration = (int)Services.Sum(s => s.Duration);

        //            // Calculate the end time based on start time and total duration
        //            return StartTime.AddMinutes(totalDuration);
        //        }
        //        else
        //        {
        //            // If no services are booked, end time is the same as start time
        //            return StartTime;
        //        }
        //    }
        //}
        //[DisplayName("Appointment ended")]
        //public string EndTimeString { get; set; }

        //// Foreign key for Client
        //public int ClientId { get; set; }
        //public Client Client { get; set; }

        // Foreign key for Barber
        public int BarberId { get; set; }
        public Barber Barber { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public double? ReviewGrade { get; set; }

        public virtual Review Review { get; set; }

        public Appointment()
        {
            
        }

    }
}