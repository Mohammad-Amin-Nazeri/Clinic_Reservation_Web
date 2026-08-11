using System.ComponentModel.DataAnnotations;
using Clinic.Application.DTOs.Paging;
using Clinic.Data.Entities;

namespace Clinic.Application.DTOs.ReserveRecords
{
    public class FilterRecordsDto : BasePaging
    {
        public string PatientName { get; set; }
        public string PatientNationalId { get; set; }
        public int? PatientId { get; set; }
        public int? ReservationId { get; set; }
        public string? Description { get; set; }
        public int? PaidPrice { get; set; }
        public DateTime? ReservationDate { get; set; }
        public FilterRecordState State { get; set; }
        public FilterPaymentMethod PaymentMethod { get; set; }
        public List<ReserveRecord> Data { get; set; }

        public FilterRecordsDto SetData(List<ReserveRecord> data)
        {
            Data = data;
            return this;
        }

        public FilterRecordsDto SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            AllEntitiesCount = paging.AllEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            AroundCurrentPage = paging.AroundCurrentPage;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            PageCount = paging.PageCount;
            return this;
        }
    }
    public enum FilterRecordState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "رزرو شده")]
        Reserved,
        [Display(Name = "لغو شده")]
        Cancelled,
        [Display(Name = "برگزار شده")]
        Attended
    }

    public enum FilterPaymentMethod
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "نقدی")]
        Cash,
        [Display(Name = "کارت بانکی")]
        CreditCard,
        UnPaid,
    }
}
