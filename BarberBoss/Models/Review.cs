using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarberBoss.Models
{
    public class Review
    {
        [ForeignKey("Appointment")]
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Grade must be between 1 and 5")]
        public double Grade { get; set; }

        // Additional review-related properties can be added here
        public virtual Appointment Appointment { get; set; }

        public Review()
        {
            
        }
    }
}