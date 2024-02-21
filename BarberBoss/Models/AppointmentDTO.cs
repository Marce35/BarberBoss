using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarberBoss.Models
{
    public class AppointmentDTO
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BookTime { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
        public bool IsBooked { get; set; }
        public int Id { get; set; }

        public string UserId { get; set; }
        public int BarberId { get; set; }
        public string BarberName { get; set; }

    }
}