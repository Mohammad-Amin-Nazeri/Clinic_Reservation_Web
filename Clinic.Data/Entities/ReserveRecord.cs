using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class ReserveRecord : BaseEntity
    {
        public int PatientId { get; set; }
        public int ReservationId { get; set; }
        public string? Description { get; set; }
        public int PaidPrice { get; set; }
        public ReservationState State { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Reservation Reservation { get; set; }
        public Patient Patient { get; set; }
    }

    public enum ReservationState
    {
        [Display(Name = "رزرو شده")]
        Reserved,
        [Display(Name = "لغو شده")]
        Cancelled,
        [Display(Name = "برگزار شده")]
        Attended
    }

    public enum PaymentMethod
    {
        [Display(Name = "پرداخت نشده")]
        UnPaid,
        [Display(Name = "نقدی")]
        Cash,
        [Display(Name = "کارت اعتباری")]
        CreditCard,
        [Display(Name = "پرداخت آنلاین (کارت به کارت)")]
        OnlineTransfer
    }
}
