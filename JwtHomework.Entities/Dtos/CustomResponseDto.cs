using System.Text.Json.Serialization;

namespace JwtHomework.Entities
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore]//JsonIgnore ile json dosyalarında bu property ıgnore ediyoruz göstermıyoruz.
        public int StatusCode { get; set; }

        public List<string> Error { get; set; }


        //static Factory Method Dessing Pattern.
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T>() { Data = data, StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T>() { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T>() { StatusCode = statusCode, Error = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T>() { StatusCode = statusCode, Error = new List<string> { error } };
        }
    }
}
