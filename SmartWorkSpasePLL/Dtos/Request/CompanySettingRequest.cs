using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Request
{
    public class CompanySettingRequest
    {
        [Required]
        public TimeSpan WorkStartTime { get; set; }= new TimeSpan(8, 0, 0);
        // وقت بداية الدوام الرسمي.
        [Required]
        public TimeSpan WorkEndTime { get; set; }= new TimeSpan(17, 0, 0);
        // وقت نهاية الدوام الرسمي.
        
        [Required]
        public int BlueWindowMinutes { get; set; }
        // فترة التأخير البسيط بالدقائق: من 8 إلى قبل 9 مثلًا.
        [Required]
        public int GreenCardsPerEarlyArrival { get; set; } = 1;
        // عدد Green Cards عند الحضور المبكر.
        [Required]
        public int BlueCardsPerLateArrival { get; set; } = 1;
        // عدد Blue Cards عند التأخير البسيط.
        [Required]
        public int BlackCardsPerLateArrival { get; set; } = 1;
        // عدد Black Cards عند التأخير الشديد.
        [Required]
        public int BlackCardsPerAbsence { get; set; } = 3;
        // عدد Black Cards عند الغياب غير المصرح به.
        [Required]
        public int MonthlyApprovedLeaveDays { get; set; } = 3;
        // عدد أيام الإجازة المعتمدة شهريًا.

        // =========================
        // Cards & Rewards Settings
        // =========================
        [Required]
        public int GreenForReward { get; set; } = 65;
        // عدد Green Cards المطلوبة للحصول على مكافأة.
        
        [Required]
        public int BlueForFine { get; set; } = 40;
        // عدد Blue Cards المطلوبة لتطبيق الخصم.
        
        [Required]
        public int BlackForFine { get; set; } = 30;
        // عدد Black Cards المطلوبة لإنشاء الغرامة.
        
        [Required]
        public decimal RewardAmount { get; set; } = 700;
        // قيمة المكافأة المالية.

        [Required]
        public decimal BlueFineAmount { get; set; } = 1000;
        // قيمة الخصم الناتج عن Blue Cards.
        
        [Required]
        public decimal BlackFineAmount { get; set; } = 1500;
        // قيمة الغرامة الناتجة عن Black Cards.
        

        public bool RequireFinePaymentBeforeCheckIn { get; set; } = true;
        // منع تسجيل الحضور عند وجود غرامة مستحقة غير مدفوعة.

        // =========================
        // Overtime Settings
        // =========================
        [Required]
        public bool EnableOvertime { get; set; }
        // تفعيل أو إيقاف نظام العمل الإضافي.

        [Required]
        public decimal OvertimeMultiplier { get; set; } = 1.5m;
        // معامل حساب أجر الساعة الإضافية في الأيام العادية.

        [Required]
        public decimal HolidayOvertimeMultiplier { get; set; } = 2.0m;
        // معامل حساب أجر الساعة الإضافية في الإجازات.


        public decimal OvertimeBaseRatePerHour { get; set; }
        // أجر الساعة الأساسي المستخدم لحساب الإضافي.

        [Required]
        public int WorkingHoursPerDay { get; set; }= 8;
        // عدد ساعات العمل اليومية.

        [Required]
        public int MaxOvertimeMinutes { get; set; }= 240;
    }
}
