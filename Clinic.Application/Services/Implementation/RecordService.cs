using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Paging;
using Clinic.Application.DTOs.ReserveRecords;
using Clinic.Application.Services.Interfaces;
using Clinic.Data.Entities;
using Clinic.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services.Implementation
{
    public class RecordService : IRecordService
    {
        #region Ctor
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<ReserveRecord> _recordRepository;
        private readonly IGenericRepository<Reservation> _reservationRepository;
        public RecordService(IGenericRepository<Patient> patientRepository, IGenericRepository<ReserveRecord> recordRepository, IGenericRepository<Reservation> reservationRepository)
        {
            _patientRepository = patientRepository;
            _recordRepository = recordRepository;
            _reservationRepository = reservationRepository;
        }
        #endregion

        public async Task<FilterRecordsDto> FilterRecords(FilterRecordsDto filter)
        {
            var query = _recordRepository.GetAll()
                .Include(r => r.Patient)
                .Include(r => r.Reservation)
                .OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            #region Switch
            switch (filter.PaymentMethod)
            {
                case FilterPaymentMethod.All:
                    break;
                case FilterPaymentMethod.Cash:
                    query = query.Where(r => r.PaymentMethod == PaymentMethod.Cash);
                    break;
                case FilterPaymentMethod.CreditCard:
                    query = query.Where(r => r.PaymentMethod == PaymentMethod.CreditCard);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (filter.State)
            {
                case FilterRecordState.All:
                    break;
                case FilterRecordState.Reserved:
                    query = query.Where(r => r.State == ReservationState.Reserved);
                    break;
                case FilterRecordState.Cancelled:
                    query = query.Where(r => r.State == ReservationState.Cancelled);
                    break;
                case FilterRecordState.Attended:
                    query = query.Where(r => r.State == ReservationState.Attended);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            #endregion

            #region Filter
            if (!string.IsNullOrEmpty(filter.PatientName))
                query = query.Where(p => EF.Functions.Like(p.Patient.FullName.Trim(), $"%{filter.PatientName.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.PatientNationalId))
                query = query.Where(p => EF.Functions.Like(p.Patient.NationalId.Trim(), $"%{filter.PatientNationalId.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(p => EF.Functions.Like(p.Description.Trim(), $"%{filter.Description.Trim()}%"));

            if (filter.PatientId is > 0)
            {
                query = query.Where(p => p.PatientId == filter.PatientId.Value);
            }

            if (filter.ReservationDate is not null)
            {
                query = query.Where(p => p.Reservation.ReserveDate.Date == filter.ReservationDate.Value);
            }

            if (filter.ReservationId is > 0)
            {
                query = query.Where(p => p.ReservationId == filter.ReservationId.Value);
            }

            if (filter.PaidPrice is > 0)
            {
                query = query.Where(p => p.PaidPrice == filter.PaidPrice.Value);
            }

            #endregion

            #region Paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.AroundCurrentPage);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetData(allEntities).SetPaging(pager);
        }

        public async Task<ReservationRecordDetailDto> ReservationRecordDetail(int id)
        {
            var data = await _recordRepository.GetEntityById(id);
            return new ReservationRecordDetailDto
            {
                CreateDate = data.CreateDate,
                Description = data.Description,
                Id = data.Id,
                PatientId = data.PatientId,
                LastUpdateDate = data.UpdateDate,
                State = data.State,
                ReservationId = data.ReservationId,
                PaymentMethod = data.PaymentMethod,
                PaidPrice = data.PaidPrice,
                Reservation = await _reservationRepository.GetEntityById(data.ReservationId),
                Patient = await _patientRepository.GetEntityById(data.PatientId)
            };
        }

        public async Task<BaseResponse> SubmitReservation(ReserveTimeDto dto)
        {
            #region Check Reservation
            var time = await _reservationRepository.GetEntityById(dto.ReservationId);
            if (time.Reserved)
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "این نوبت رزرو شده است."
                };
            #endregion

            #region Patient
            var patient = await _patientRepository.GetAll().FirstOrDefaultAsync(p => p.Mobile == dto.PatientMobile);
            if (patient is null)
            {
                var newPatient = new Patient
                {
                    Age = 0,
                    Gender = Gender.UnSpecified,
                    Mobile = dto.PatientMobile,
                    FullName = dto.PatientName,
                    NationalId = dto.NationalId ?? "تعیین نشده"
                };
                await _patientRepository.Create(newPatient);
                await _patientRepository.SaveChanges();
                patient = newPatient;
            }

            #endregion

            var reservation = new ReserveRecord
            {
               PatientId = patient.Id,
               State = ReservationState.Reserved,
               PaymentMethod = PaymentMethod.UnPaid,
               ReservationId = dto.ReservationId,
               PaidPrice = 0,
            };

            await _recordRepository.Create(reservation);
            await _recordRepository.SaveChanges();

            #region Change Reservation State
            time.Reserved = true;
            _reservationRepository.Update(time);
            await _reservationRepository.SaveChanges();
            #endregion

            return new BaseResponse
            {
                Id = reservation.Id,
                ResultStatus = ResultStatus.Success,
                Message = "نوبت با موفقیت ثبت شد."
            };
        }

        public async Task<EditRecordDto> GetEditRecord(int id)
        {
            var data = await _recordRepository.GetEntityById(id);
            return new EditRecordDto
            {
                PaymentMethod = data.PaymentMethod,
                Description = data.Description,
                Id = id,
                State = data.State,
                PaidPrice = data.PaidPrice.ToString()
            };
        }

        public async Task<BaseResponse> UpdateRecord(EditRecordDto dto)
        {
            var data = await _recordRepository.GetEntityById(dto.Id);

            var price = int.Parse(dto.PaidPrice.Replace(",", ""));

            data.PaymentMethod = dto.PaymentMethod;
            data.Description = dto.Description;
            data.PaidPrice = price;
            data.State = dto.State;

            _recordRepository.Update(data);
            await _recordRepository.SaveChanges();

            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
        }

        public async Task DeleteRecord(int id)
        {
            await _recordRepository.Delete(id);
            await _recordRepository.SaveChanges();
        }

        public async ValueTask DisposeAsync()
        {
            await _recordRepository.DisposeAsync();
            await _patientRepository.DisposeAsync();
            await _reservationRepository.DisposeAsync();
        }
    }
}
