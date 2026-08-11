namespace Clinic.Application.DTOs.Common
{
    public class BaseResponse
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public ResultStatus ResultStatus { get; set; }

        //Items to show in MVC
        public List<string>? Items { get; set; }
    }

    public enum ResultStatus
    {
        Error,
        Success,
        Warning,
        Info
    }
}
