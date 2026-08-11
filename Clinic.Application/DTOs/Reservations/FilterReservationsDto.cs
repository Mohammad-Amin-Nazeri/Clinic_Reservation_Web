using System.ComponentModel.DataAnnotations;
using Clinic.Application.DTOs.Paging;
using Clinic.Data.Entities;

namespace Clinic.Application.DTOs.Reservations
{
    public class FilterReservationsDto : BasePaging
    {
        public DateTime? ReserveDate { get; set; }
        public FilterReservationState FilterReservationState { get; set; }
        public List<Reservation> Data { get; set; }
        public List<DateTime> Days { get; set; }

        public FilterReservationsDto SetData(List<Reservation> data)
        {
            Data = data;
            return this;
        }

        public FilterReservationsDto SetPaging(BasePaging paging)
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

    public enum FilterReservationState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "رزرو شده")]
        Reserved,
        [Display(Name = "رزرو نشده")]
        NotReserved
    }
}
