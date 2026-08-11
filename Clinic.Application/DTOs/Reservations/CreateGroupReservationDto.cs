namespace Clinic.Application.DTOs.Reservations
{
    public class CreateGroupReservationDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public List<DayOfWeek> VisitDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TimeStepMinutes { get; set; }
    }
}
