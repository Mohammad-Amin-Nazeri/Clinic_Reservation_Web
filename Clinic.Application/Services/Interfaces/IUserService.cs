using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.DTOs.Users;
using Clinic.Data.Entities;

namespace Clinic.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region Authentication
        Task<BaseResponse> Login(LoginDto dto);
        Task<BaseResponse> CheckOtp(AuthenticationDto dto);
        Task ResendOtp(string mobile);
        #endregion
     
        #region Users
        Task<List<UserDetailDto>> GetUsers();
        Task<UserDetailDto> GetUserDetail(int id);
        Task<UserDetailDto> GetUserByMobile(string mobile);
        Task<User> GetUserById(int id);
        Task<BaseResponse> CreateUser(CreateUserDto dto);
        Task<EditUserDto> GetEditUser(int id);
        Task<BaseResponse> EditUser(EditUserDto dto);
        Task DeleteUser(int id);
        #endregion

        #region Patient
        Task<FilterPatientsDto> FilterPatients(FilterPatientsDto filter);
        Task<BaseResponse> CreateGroupPatients(List<CreateGroupPatientsDto> dto);
        Task<PatientDetailDto> PatientDetail(int id);
        Task<BaseResponse> CreatePatient(CreatePatientDto dto);
        Task<EditPatientDto> GetEditPatient(int id);
        Task<BaseResponse> EditPatient(EditPatientDto dto);
        Task<BaseResponse> DeletePatient(int id);
        #endregion
    }
}
