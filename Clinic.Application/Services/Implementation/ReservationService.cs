using System.Globalization;
using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Paging;
using Clinic.Application.DTOs.Reservations;
using Clinic.Application.Services.Interfaces;
using Clinic.Application.Utilities;
using Clinic.Data.Entities;
using Clinic.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services.Implementation
{
    public class ReservationService(IGenericRepository<Reservation> reservationRepository,
        IGenericRepository<ReserveRecord> recordRepository) : IReservationService
    {
        public async Task<FilterReservationsDto> FilterReservation(FilterReservationsDto filter)
        {
            var query = reservationRepository.GetAll();

            switch (filter.FilterReservationState)
            {
                case FilterReservationState.All:
                    break;
                case FilterReservationState.Reserved:
                    query = query.Where(r => r.Reserved);
                    break;
                case FilterReservationState.NotReserved:
                    query = query.Where(r => !r.Reserved);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (filter.ReserveDate != null)
            {
                query = query.Where(r => r.ReserveDate.Date == filter.ReserveDate.Value.Date);
            }

            #region Paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.AroundCurrentPage);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetData(allEntities).SetPaging(pager);
        }

        public async Task<List<Reservation>> GetReservationsByDate(DateTime date)
        {
            return await reservationRepository.GetAll().Where(r => r.ReserveDate == date.Date).ToListAsync();
        }

        public async Task<Reservation> GetReservationById(int reservationId)
        {
            return await reservationRepository.GetEntityById(reservationId);
        }

        public async Task<BaseResponse> CreateGroupReservations(CreateGroupReservationDto dto)
        {
            var reservations = new List<Reservation>();
            var persianCalendar = new PersianCalendar();

            #region Validations
            //اعتبارسنجی سال شمسی
            var currentYear = persianCalendar.GetYear(DateTime.Now);
            var currentMonth = persianCalendar.GetMonth(DateTime.Now);

            if (dto.Year < currentYear)
                return new BaseResponse
                { ResultStatus = ResultStatus.Error, Message = "سال انتخابی نمیتواند کوچک تر از سال جاری باشد." };

            if (dto.StartTime >= dto.EndTime)
                return new BaseResponse
                { ResultStatus = ResultStatus.Error, Message = "زمان شروع ویزیت نمیتواند کوچک تر یا برابر با زمان پایان ویزیت باشد." };
            #endregion

            //Find first day of persian current month
            var firsDayOfMonth = persianCalendar.ToDateTime(dto.Year, dto.Month, 1, 0, 0, 0, 0);

            var dayInMonth = persianCalendar.GetDaysInMonth(dto.Year, dto.Month);
            var lastDayOfMonth = persianCalendar.ToDateTime(dto.Year, dto.Month, dayInMonth, 0, 0, 0, 0);

            //Find days of month based on selected days of week
            var validDates = new List<DateTime>();

            for (var date = firsDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                var persianDaysOfWeek = persianCalendar.GetDayOfWeek(date).ConvertToPersianDayOfWeek();
                if (dto.VisitDays.Contains(persianDaysOfWeek))
                    validDates.Add(date);
            }

            //Create Reservation based on validDates
            foreach (var date in validDates)
            {
                var currentStartTime = date.Date + dto.StartTime; // 1405-02-10 16:00
                var dayEndTime = date.Date + dto.EndTime; // 1405-02-10 20:00

                while (currentStartTime < dayEndTime)
                {
                    var endTime = currentStartTime.AddMinutes(dto.TimeStepMinutes);
                    if (endTime > dayEndTime)
                        endTime = dayEndTime;

                    var reservation = new Reservation
                    {
                        ReserveDate = currentStartTime,
                        EndReserveTime = endTime,
                        Reserved = false
                    };

                    reservations.Add(reservation);
                    currentStartTime = endTime;
                }
            }

            await reservationRepository.CreateRangeEntities(reservations);
            await reservationRepository.SaveChanges();

            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = "عملیات با موفقیت انجام شد."
            };
        }

        public async Task<BaseResponse> CreateReservations(CreateReservationDto dto)
        {
            #region Check
            var isAvailable = await reservationRepository.GetAll()
                .AnyAsync(r => r.ReserveDate == dto.ReserveDate);
            if (isAvailable)
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "برای این تاریخ نوبت در سامانه موجود است."
                };
            #endregion

            var endTime = dto.ReserveDate + TimeSpan.FromMinutes(dto.TimeStep);

            var reservation = new Reservation
            {
                Reserved = false,
                ReserveDate = dto.ReserveDate,
                EndReserveTime = endTime
            };

            await reservationRepository.Create(reservation);
            await reservationRepository.SaveChanges();

            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = "نوبت ایجاد شد."
            };
        }

        public async Task ReserveReservation(int id)
        {
            var data = await reservationRepository.GetEntityById(id);
            data.Reserved = true;
            reservationRepository.Update(data);
            await reservationRepository.SaveChanges();
        }

        public async Task NotReserveReservation(int id)
        {
            var data = await reservationRepository.GetEntityById(id);
            data.Reserved = false;
            reservationRepository.Update(data);
            await reservationRepository.SaveChanges();
        }

        public async Task<BaseResponse> DeleteReservation(int id)
        {
            #region Check
            var inUse = await recordRepository.GetAll().AnyAsync(r => r.ReservationId == id);
            if (inUse) return new BaseResponse
            {
                ResultStatus = ResultStatus.Error,
                Message = "یک نوبت برای این تاریخ ویزیت رزرو شده است"
            };
            #endregion

            await reservationRepository.Delete(id);
            await reservationRepository.SaveChanges();

            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = "رزرو با موفقیت حذف شد"
            };
        }

        public async Task<BaseResponse> DeleteGroupReservation(int year , int month)
        {
            var pc = new PersianCalendar();

            //Convert To Persian Date
            var startDate = pc.ToDateTime(year, month, 1 , 0, 0, 0, 0);
            var lastDay = pc.GetDaysInMonth(year, month);
            var endDate = pc.ToDateTime(year, month, lastDay, 23, 59, 59 ,999);

            // Invoke Reservation List Based On Year & Month
            var reservations = await reservationRepository.GetAll()
                .Where(d => d.ReserveDate >= startDate && d.ReserveDate <= endDate)
                .ToListAsync();

            //Invoke ReservationRecord List Based On Year & Month
            var records = await recordRepository.GetAll()
                .Include(d => d.Reservation)
                .Where(d => d.Reservation.ReserveDate >= startDate && d.Reservation.ReserveDate <= endDate) 
                .ToListAsync();

            //Invoke Records Reservation Ids
            var reservationsIdsInRecord = records.Select(r => r.ReservationId)
                .Distinct().ToHashSet();

            // Remove Used Reservations
            var reservationsToDelete =
                reservations.Where(r => !reservationsIdsInRecord.Contains(r.Id)).ToList();

            reservationRepository.DeleteRangeEntities(reservationsToDelete);
            await reservationRepository.SaveChanges();

            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = $"تعداد {reservationsToDelete.Count} رزرو با موفقیت حذف شدند. {reservations.Count - reservationsToDelete.Count} مورد رزرو به دلیل استفاده در یک نوبت ثبت شده امکان حذف نداشتند.",
            };
        }

        public async ValueTask DisposeAsync()
        {
            await reservationRepository.DisposeAsync();
        }

    }
}
