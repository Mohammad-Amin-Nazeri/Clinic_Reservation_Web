using Clinic.Data.Entities;

namespace Clinic.Application.DTOs.ReserveRecords
{
    public class ReservationRecordDetailDto
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int PatientId { get; set; }
        public int ReservationId { get; set; }
        public string? Description { get; set; }
        public int PaidPrice { get; set; }
        public ReservationState State { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Reservation Reservation { get; set; }
        public Patient Patient { get; set; }
    }
}
