using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum Type
    {
        Blackcard = 1,
        Bluecard = 2,
    }
    public enum FineStatus
    {
        Unpaid = 1,
        Paid = 2,
        Overdue = 3
    }
    public class Fine
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public Applicationuser ApplicationUser { get; set; } = null!;
        public Type Type { get; set; } 
        public decimal Amount { get; set; }
        public bool BlocksCheckIn { get; set; }
        public string Status { get; set; } 
        public string? Method { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool PaidAt { get; set; }
    }
}
