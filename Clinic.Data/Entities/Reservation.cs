namespace Clinic.Data.Entities
{
    public class Reservation : BaseEntity
    {
        public DateTime ReserveDate { get; set; }
        public DateTime EndReserveTime { get; set; }
        public bool Reserved { get; set; }
    }
}
