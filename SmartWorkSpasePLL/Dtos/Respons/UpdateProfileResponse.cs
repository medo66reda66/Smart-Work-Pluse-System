using Microsoft.AspNetCore.Http;
using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class UpdateProfileResponse
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string Gavernate { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public Gender gender { get; set; }
        [Required]
        public IFormFile ImageUrl { get; set; } 

    }
}
