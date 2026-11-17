using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public class Reward
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public Applicationuser ApplicationUser { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal UsedAmount { get; set; }
        public string? PayPalNumber { get; set; }

        public bool IsClaimed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
