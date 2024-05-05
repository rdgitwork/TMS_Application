using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneProject.Models
{
    public class RouteModel
    {
        public RouteModel() { }
        public int route_id { get; set; }
        public string start_point { get; set; }
        public string end_point { get; set; }
        public decimal distance { get; set; }
        public decimal travel_time { get; set; }
    }
}
