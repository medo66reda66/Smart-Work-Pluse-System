using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    namespace DAL.Entities
    {
        public class CompanySetting
        {
            public int Id { get; set; }

            // =========================
            // Attendance Settings
            // =========================

            public TimeSpan WorkStartTime { get; set; } = new TimeSpan(8, 0, 0);
            // وقت بداية الدوام الرسمي.

            public TimeSpan WorkEndTime { get; set; } = new TimeSpan(17, 0, 0);
            // وقت نهاية الدوام الرسمي.

            public int BlueWindowMinutes { get; set; } = 60;
            // فترة التأخير البسيط بالدقائق: من 8 إلى قبل 9 مثلًا.

            public int GreenCardsPerEarlyArrival { get; set; } = 1;
            // عدد Green Cards عند الحضور المبكر.

            public int BlueCardsPerLateArrival { get; set; } = 1;
            // عدد Blue Cards عند التأخير البسيط.

            public int BlackCardsPerLateArrival { get; set; } = 1;
            // عدد Black Cards عند التأخير الشديد.

            public int BlackCardsPerAbsence { get; set; } = 3;
            // عدد Black Cards عند الغياب غير المصرح به.

            public int MonthlyApprovedLeaveDays { get; set; } = 3;
            // عدد أيام الإجازة المعتمدة شهريًا.

            // =========================
            // Cards & Rewards Settings
            // =========================

            public int GreenForReward { get; set; } = 65;
            // عدد Green Cards المطلوبة للحصول على مكافأة.

            public int BlueForFine { get; set; } = 60;
            // عدد Blue Cards المطلوبة لتطبيق الخصم.

            public int BlackForFine { get; set; } = 30;
            // عدد Black Cards المطلوبة لإنشاء الغرامة.

            public decimal RewardAmount { get; set; }
            // قيمة المكافأة المالية.

            public decimal BlueFineAmount { get; set; }
            // قيمة الخصم الناتج عن Blue Cards.

            public decimal BlackFineAmount { get; set; }
            // قيمة الغرامة الناتجة عن Black Cards.

            public bool RequireFinePaymentBeforeCheckIn { get; set; } = true;
            // منع تسجيل الحضور عند وجود غرامة مستحقة غير مدفوعة.

            // =========================
            // Overtime Settings
            // =========================

            public bool EnableOvertime { get; set; } = true;
            // تفعيل أو إيقاف نظام العمل الإضافي.

            public decimal OvertimeMultiplier { get; set; } = 1.5m;
            // معامل حساب أجر الساعة الإضافية في الأيام العادية.

            public decimal HolidayOvertimeMultiplier { get; set; } = 2.0m;
            // معامل حساب أجر الساعة الإضافية في الإجازات.

            public decimal OvertimeBaseRatePerHour { get; set; }
            // أجر الساعة الأساسي المستخدم لحساب الإضافي.

            public int WorkingHoursPerDay { get; set; } = 8;
            // عدد ساعات العمل اليومية.

            public int MaxOvertimeMinutes { get; set; } = 240;
            // أقصى مدة مسموح بها للعمل الإضافي يوميًا.
        }
    }

}
