using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class UpdatePasswordResponse
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [Required]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Required]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
