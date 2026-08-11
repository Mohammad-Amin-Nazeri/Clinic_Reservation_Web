using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class Patient : BaseEntity
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public string? Description { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public ICollection<ReserveRecord> ReserveRecords { get; set; }
    }

    public enum Gender
    {
        [Display(Name = "آقا")]
        Male,
        [Display(Name = "خانم")]
        Female,
        [Display(Name = "تعیین نشده")]
        UnSpecified
    }
}
