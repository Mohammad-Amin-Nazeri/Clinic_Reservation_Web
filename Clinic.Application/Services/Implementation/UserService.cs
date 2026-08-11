using System.Resources;
using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Paging;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.DTOs.Users;
using Clinic.Application.Services.Interfaces;
using Clinic.Data.Entities;
using Clinic.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        #region CTOR
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<ReserveRecord> _recordsRepository;
        private readonly IOtpService _otpService;
        private readonly ISmsService _smsService;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Patient> patientRepository, IGenericRepository<ReserveRecord> recordsRepository, IOtpService otpService, ISmsService smsService)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _recordsRepository = recordsRepository;
            _otpService = otpService;
            _smsService = smsService;
        }

        #endregion

        #region Authentication
        public async Task<BaseResponse> Login(LoginDto dto)
        {
            var isMobileExist = await _userRepository.GetAll().AnyAsync(u => u.Mobile == dto.Mobile);
            if (!isMobileExist) return new BaseResponse { ResultStatus = ResultStatus.Error, Message = "شماره موبایل یافت نشد." };

            // Already has otp
            var hasOtp = _otpService.CanRegenrateOtp(dto.Mobile);
            if (!hasOtp) return new BaseResponse { ResultStatus = ResultStatus.Error, Message = "تا دو دقیقه نمیتوانید درخواست کد اعتبارسنجی بدهید." };

            // Send OTP
            var otp = _otpService.GenrateOtp(dto.Mobile);
            try
            {
                await _smsService.SendOtp(dto.Mobile, otp);
            }
            catch (Exception e)
            {
                return new BaseResponse { ResultStatus = ResultStatus.Success, Message = "ارسال sms با خطا مواجه شد." };
            }

            return new BaseResponse { ResultStatus = ResultStatus.Success, Message = "کد اعتبارسنجی با موفقیت ارسال شد." };
        }

        public async Task<BaseResponse> CheckOtp(AuthenticationDto dto)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Mobile == dto.Mobile);
            if (user == null) return new BaseResponse { ResultStatus = ResultStatus.Error, Message = "شماره موبایل یافت نشد." };

            var otp = $"{dto.num1}{dto.num2}{dto.num3}{dto.num4}{dto.num5}";
            var result = _otpService.ValidateOtp(dto.Mobile, otp);

            if (result)
            {
                return new BaseResponse { ResultStatus = ResultStatus.Success, Message = "خوش آمدید!" };
            }
            return new BaseResponse { ResultStatus = ResultStatus.Error, Message = "کد اعتبارسنجی اشتباه است." };
        }

        public async Task ResendOtp(string mobile)
        {
            var otp = _otpService.GenrateOtp(mobile);
            await _smsService.SendOtp(mobile, otp);
        }
        #endregion

        #region User
        public async Task<List<UserDetailDto>> GetUsers()
        {
            var data = await _userRepository.GetAll().Select(u => new UserDetailDto
            {
                LastUpdateDate = u.UpdateDate,
                CreateDate = u.CreateDate,
                FullName = u.FullName,
                Id = u.Id,
                Mobile = u.Mobile
            }).ToListAsync();
            return data;
        }

        public async Task<UserDetailDto> GetUserDetail(int id)
        {
            var user = await _userRepository.GetEntityById(id);
            return new UserDetailDto
            {
                LastUpdateDate = user.UpdateDate,
                CreateDate = user.CreateDate,
                FullName = user.FullName,
                Id = user.Id,
                Mobile = user.Mobile
            };
        }

        public async Task<UserDetailDto> GetUserByMobile(string mobile)
        {
            var user = await _userRepository.GetAll().FirstAsync(u => u.Mobile == mobile);
            return new UserDetailDto
            {
                LastUpdateDate = user.UpdateDate,
                CreateDate = user.CreateDate,
                FullName = user.FullName,
                Id = user.Id,
                Mobile = user.Mobile
            };
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetEntityById(id);
        }

        public async Task<BaseResponse> CreateUser(CreateUserDto dto)
        {
            var duplicateMobile = await _userRepository.GetAll().AnyAsync(u => u.Mobile == dto.Mobile);
            if (duplicateMobile)
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "ادمینی با این شماره موبایل قبلا ایجاد شد."
                };

            var user = new User
            {
                Mobile = dto.Mobile,
                FullName = dto.Fullname
            };

            await _userRepository.Create(user);
            await _userRepository.SaveChanges();

            return new BaseResponse
            {
                Id = user.Id,
                ResultStatus = ResultStatus.Success,
                Message = "ادمین با موفقیت ایجاد شد."
            };
        }

        public async Task<EditUserDto> GetEditUser(int id)
        {
            var user = await _userRepository.GetEntityById(id);
            return new EditUserDto
            {
                Mobile = user.Mobile,
                Fullname = user.FullName,
                Id = id
            };
        }

        public async Task<BaseResponse> EditUser(EditUserDto dto)
        {
            var duplicateMobile = await _userRepository.GetAll().AnyAsync(u => u.Mobile == dto.Mobile && u.Id != dto.Id);
            if (duplicateMobile)
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "ادمینی با این شماره موبایل قبلا ایجاد شد."
                };

            var user = await _userRepository.GetEntityById(dto.Id);
            user.Mobile = dto.Mobile;
            user.FullName = dto.Fullname;

            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            return new BaseResponse
            {
                Id = user.Id,
                ResultStatus = ResultStatus.Success,
                Message = "اطلاعات ادمین با موفقیت ویرایش شد"
            };
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.Delete(id);
            await _userRepository.SaveChanges();
        }
        #endregion

        #region Patients
        public async Task<FilterPatientsDto> FilterPatients(FilterPatientsDto filter)
        {
            var query = _patientRepository.GetAll().OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            switch (filter.Gender)
            {
                case FilterGender.All:
                    break;
                case FilterGender.Male:
                    query = query.Where(p => p.Gender == Gender.Male);
                    break;
                case FilterGender.Female:
                    query = query.Where(p => p.Gender == Gender.Female);
                    break;
                case FilterGender.UnSpecified:
                    query = query.Where(p => p.Gender == Gender.UnSpecified);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (filter.ListOrder)
            {
                case ListOrder.Newest:
                    query = query.OrderByDescending(p => p.CreateDate);
                    break;
                case ListOrder.Oldest:
                    query = query.OrderBy(p => p.CreateDate);
                    break;
                case ListOrder.Alphabet:
                    query = query.OrderBy(p => p.FullName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!string.IsNullOrEmpty(filter.FullName))
                query = query.Where(p => EF.Functions.Like(p.FullName.Trim(), $"%{filter.FullName.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(p => EF.Functions.Like(p.Mobile.Trim(), $"%{filter.Mobile.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.NationalId))
                query = query.Where(p => EF.Functions.Like(p.NationalId.Trim(), $"%{filter.NationalId.Trim()}%"));

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(p => p.Description != null && EF.Functions.Like(p.Description.Trim(), $"%{filter.Description.Trim()}%"));

            if (filter.Age is > 0)
            {
                query = query.Where(p => p.Age == filter.Age.Value);
            }
            #region Paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.AroundCurrentPage);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetData(allEntities).SetPaging(pager);
        }

        public async Task<BaseResponse> CreateGroupPatients(List<CreateGroupPatientsDto> dto)
        {
            var patients = new List<Patient>();
            var errors = new List<string>();

            #region Check Mobiles & NationalIds
            var mobiles = dto.Select(x => x.Mobile).ToList();
            var nationalIds = dto.Select(x => x.NationalId).ToList();

            var existingMobiles = await _patientRepository.GetAll()
                .Where(p => mobiles.Contains(p.Mobile))
                .Select(p => p.Mobile)
                .ToListAsync();

            var existingNationalIds = await _patientRepository.GetAll()
                .Where(p => nationalIds.Contains(p.NationalId))
                .Select(p => p.NationalId)
                .ToListAsync();
            #endregion

            foreach (var item in dto)
            {
                if (existingMobiles.Contains(item.Mobile))
                {
                    errors.Add($"برای {item.FullName} شماره موبایل تکراری است.");
                    continue;
                }

                if (existingNationalIds.Contains(item.NationalId))
                {
                    errors.Add($"برای {item.FullName} کد ملی تکراری است.");
                    continue;
                }

                var patient = new Patient
                {
                    Mobile = item.Mobile,
                    Age = item.Age,
                    FullName = item.FullName,
                    NationalId = item.NationalId,
                    Gender = item.Gender switch
                    {
                        "Male" => Gender.Male,
                        "Female" => Gender.Female,
                        _ => Gender.UnSpecified
                    }
                };

                patients.Add(patient);
            }

            if (!patients.Any())
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "هیچ بیماری اضافه نشد.",
                    Items = errors
                };

            await _patientRepository.CreateRangeEntities(patients);
            await _userRepository.SaveChanges();

            if (errors.Any())
            {
                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Warning,
                    Message = "بیماران با موفقیت اضافه شدند.",
                    Items = errors
                };
            }
            return new BaseResponse
            {
                ResultStatus = ResultStatus.Success,
                Message = "بیماران با موفقیت اضافه شدند.",
            };
        }

        public async Task<PatientDetailDto> PatientDetail(int id)
        {
            var data = await _patientRepository.GetEntityById(id);
            return new PatientDetailDto
            {
                Age = data.Age,
                CreateDate = data.CreateDate,
                Description = data.Description,
                FullName = data.FullName,
                Gender = data.Gender,
                Id = id,
                Mobile = data.Mobile,
                LastUpdateDate = data.UpdateDate,
                ReserveRecords = await _recordsRepository.GetAll()
                    .Include(r => r.Reservation)
                    .Where(r => r.PatientId == id).ToListAsync()
            };
        }

        public async Task<BaseResponse> CreatePatient(CreatePatientDto dto)
        {
            var errors = new List<string>();

            var patient = await _patientRepository.GetAll()
                .Where(p => p.NationalId == dto.NationalId || p.Mobile == dto.Mobile)
                .FirstOrDefaultAsync();

            if (patient is not null)
            {
                if (patient.NationalId == dto.NationalId)
                    errors.Add($"برای {dto.FullName} یک کد ملی تکراری اضافه شده.");

                if (patient.Mobile == dto.Mobile)
                    errors.Add($"برای {dto.FullName} یک شماره موبایل تکراری اضافه شده.");

                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "عملیات با شکست مواجه شد.",
                    Items = errors
                };
            }

            var newPatient = new Patient
            {
                Mobile = dto.Mobile,
                Age = dto.Age,
                Description = dto.Description,
                FullName = dto.FullName,
                NationalId = dto.NationalId,
                Gender = dto.Gender,
            };

            await _patientRepository.Create(newPatient);
            await _patientRepository.SaveChanges();

            return new BaseResponse
            {
                Id = newPatient.Id,
                ResultStatus = ResultStatus.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
        }

        public async Task<EditPatientDto> GetEditPatient(int id)
        {
            var data = await _patientRepository.GetEntityById(id);
            return new EditPatientDto
            {
                Age = data.Age,
                Description = data.Description,
                FullName = data.FullName,
                Gender = data.Gender,
                Id = id,
                Mobile = data.Mobile,
                NationalId = data.NationalId
            };
        }

        public async Task<BaseResponse> EditPatient(EditPatientDto dto)
        {
            var errors = new List<string>();

            var patient = await _patientRepository.GetAll()
                .Where(p => p.Id != dto.Id && (p.NationalId == dto.NationalId || p.Mobile == dto.Mobile))
                .FirstOrDefaultAsync();

            if (patient is not null)
            {
                if (patient.NationalId == dto.NationalId)
                    errors.Add($"برای {dto.FullName} یک کد ملی تکراری اضافه شده.");

                if (patient.Mobile == dto.Mobile)
                    errors.Add($"برای {dto.FullName} یک شماره موبایل تکراری اضافه شده.");

                return new BaseResponse
                {
                    ResultStatus = ResultStatus.Error,
                    Message = "عملیات با شکست مواجه شد.",
                    Items = errors
                };
            }

            var data = await _patientRepository.GetEntityById(dto.Id);

            data.FullName = dto.FullName;
            data.Age = dto.Age;
            data.Mobile = dto.Mobile;
            data.NationalId = dto.NationalId;
            data.Description = dto.Description;
            data.Gender = dto.Gender;

            _patientRepository.Update(data);
            await _patientRepository.SaveChanges();

            return new BaseResponse
            {
                Id = data.Id,
                ResultStatus = ResultStatus.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
        }

        public async Task<BaseResponse> DeletePatient(int id)
        {
            var hasRecord = await _recordsRepository.GetAll().AnyAsync(d => d.PatientId == id);
            if (hasRecord) return new BaseResponse { ResultStatus = ResultStatus.Error, Message = "بیمار دارای سابقه ویزیت می باشد." };

            await _patientRepository.Delete(id);
            await _patientRepository.SaveChanges();

            return new BaseResponse { ResultStatus = ResultStatus.Success, Message = "عملیات با موفقیت انجام شد." };
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
            await _patientRepository.DisposeAsync();
            await _recordsRepository.DisposeAsync();
        }


        #endregion
    }
}
