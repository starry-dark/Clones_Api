namespace Models.Dtos
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }


        public BaseResponse<T> Success(string message, int code = 200, T? data = default)
        {
            return new BaseResponse<T>() { Code = code, Status = true, Message = message, Data = data };
        }

        public BaseResponse<T> Error(string message, int code = 500)
        {
            return new BaseResponse<T>() { Code = code, Status = false, Message = message };
        }
    }
}
