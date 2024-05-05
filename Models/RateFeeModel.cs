using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneProject.Models
{
    public  class RateFeeModel
    {
        public int RateId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        // Constructor
        public RateFeeModel() { }

        // Constructor to initialize properties
        public RateFeeModel(int rateId, string description, decimal amount)
        {
            RateId = rateId;
            Description = description;
            Amount = amount;
        }
    }
}
