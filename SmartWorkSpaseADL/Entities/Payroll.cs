using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public class Payroll
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        public Applicationuser Applicationuser { get; set; } = null!;
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal OvertimeTotal { get; set; }
        public decimal RewardsTotal { get; set; }
        public decimal FinesTotal { get; set; }
        public decimal NetSalary { get; set; }
        public bool IsPaid { get; set; }
    }
}
