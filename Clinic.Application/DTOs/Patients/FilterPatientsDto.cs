using System.ComponentModel.DataAnnotations;
using Clinic.Application.DTOs.Paging;
using Clinic.Data.Entities;

namespace Clinic.Application.DTOs.Patients
{
    public class FilterPatientsDto : BasePaging
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public string? Description { get; set; }
        public int? Age { get; set; }
        public FilterGender Gender { get; set; }
        public ListOrder ListOrder { get; set; }
        public List<Patient> Data { get; set; }

        public FilterPatientsDto SetData(List<Patient> data)
        {
            Data = data;
            return this;
        }

        public FilterPatientsDto SetPaging(BasePaging paging)
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

    public enum FilterGender
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "مرد")]
        Male,
        [Display(Name = "زن")]
        Female,
        [Display(Name = "تعیین نشده")]
        UnSpecified
    }

    public enum ListOrder
    {
        [Display(Name = "جدیدترین")]
        Newest,
        [Display(Name = "قدیمی ترین")]
        Oldest,
        [Display(Name = "الفبا")]
        Alphabet
    }
}
