using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneProject.Models
{
    public class CarrierModel
    {
        public CarrierModel() { }
        public int carrier_id { get; set; }
        public string name { get; set; }
        public string contact_info { get; set; }
        public int capacity { get; set; }

        
    }
}
