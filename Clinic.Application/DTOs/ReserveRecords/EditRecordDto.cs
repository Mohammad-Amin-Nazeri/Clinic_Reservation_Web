using Clinic.Data.Entities;

namespace Clinic.Application.DTOs.ReserveRecords
{
    public class EditRecordDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string PaidPrice { get; set; }
        public ReservationState State { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
