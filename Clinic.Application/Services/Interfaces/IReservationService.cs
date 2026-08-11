using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Reservations;
using Clinic.Data.Entities;

namespace Clinic.Application.Services.Interfaces
{
    public interface IReservationService : IAsyncDisposable
    {
        Task<FilterReservationsDto> FilterReservation(FilterReservationsDto filter);
        Task<List<Reservation>> GetReservationsByDate(DateTime date);
        Task<Reservation> GetReservationById(int reservationId);
        Task<BaseResponse> CreateGroupReservations(CreateGroupReservationDto dto);
        Task<BaseResponse> CreateReservations(CreateReservationDto dto);
        Task ReserveReservation(int id);
        Task NotReserveReservation(int id);
        Task<BaseResponse> DeleteReservation(int id);
        Task<BaseResponse> DeleteGroupReservation(int year, int month);
    }
}
