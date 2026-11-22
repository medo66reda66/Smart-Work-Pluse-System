using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public class Notification 
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public Applicationuser ApplicationUser { get; set; } = null!;
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? RelatedUserId { get; set; } = string.Empty;
        public Applicationuser RelatedUser { get; set; } = null!;
    }
}
