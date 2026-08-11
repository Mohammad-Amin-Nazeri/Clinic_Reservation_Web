using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.ReserveRecords;

namespace Clinic.Application.Services.Interfaces
{
    public interface IRecordService : IAsyncDisposable
    {
        Task<FilterRecordsDto> FilterRecords(FilterRecordsDto filter);
        Task<ReservationRecordDetailDto> ReservationRecordDetail(int id);
        Task<BaseResponse> SubmitReservation(ReserveTimeDto dto);
        Task<EditRecordDto> GetEditRecord(int id);
        Task<BaseResponse> UpdateRecord(EditRecordDto dto);
        Task DeleteRecord(int id);
    }
}
