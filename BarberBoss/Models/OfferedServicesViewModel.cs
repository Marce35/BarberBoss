using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarberBoss.Models
{
    public class OfferedServicesViewModel
    {
        public int SelectedServiceId { get; set; }
        public List<ServiceOffer> OfferedServices { get; set; }
        public List<ServiceOffer> AvailableServices { get; set; }
    }
}