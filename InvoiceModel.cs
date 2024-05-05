using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneProject.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

        public InvoiceModel() { }

        public InvoiceModel(int invoiceId, int orderId, decimal amount, DateTime issueDate, DateTime dueDate, string status)
        {
            InvoiceId = invoiceId;
            OrderId = orderId;
            Amount = amount;
            IssueDate = issueDate;
            DueDate = dueDate;
            Status = status;
        }
    }
}
