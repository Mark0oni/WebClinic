namespace WebClinic.Data.Models
{
    public static class AppointmentStatusExtensions
    {
        private static readonly Dictionary<AppointmentStatus, string> StatusNames = new Dictionary<AppointmentStatus, string>
        {
            { AppointmentStatus.Scheduled, "Запланировано" },
            { AppointmentStatus.Completed, "Завершено" },
            { AppointmentStatus.Cancelled, "Отменено" }
        };

        public static string ToRussianString(this AppointmentStatus status)
        {
            return StatusNames.ContainsKey(status) ? StatusNames[status] : status.ToString();
        }
    }
}
