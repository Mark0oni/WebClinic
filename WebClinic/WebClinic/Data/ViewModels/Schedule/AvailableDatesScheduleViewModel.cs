using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.Schedule
{
    public class AvailableDatesScheduleViewModel
    {
        public Guid? Id { get; set; }

        public required DateTime Date { get; set; }
 
        public required TimeSpan StartTime { get; set; }

        public required TimeSpan EndTime { get; set; }

        public required string ServiceName { get; set; }

        public required string DoctorFullName { get; set; }

        public required string Experience { get; set; }
        
        public required string PostName { get; set; }
    }
}