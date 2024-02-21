using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarberBoss.Models
{
    public class Barber
    {
       
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        public string InstagramProfile { get; set; }

        public string FacebookProfile { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<ServiceOffer> ServicesOffered { get; set; }

        public Barber()
        {
            Appointments = new List<Appointment>();
            ServicesOffered = new List<ServiceOffer>();
        }
    }
}