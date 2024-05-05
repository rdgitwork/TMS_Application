using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneProject.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int? CarrierId { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
