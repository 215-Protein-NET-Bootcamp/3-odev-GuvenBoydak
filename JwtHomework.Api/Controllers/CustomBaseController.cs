using JwtHomework.Base;
using Microsoft.AspNetCore.Mvc;

namespace JwtHomework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //Bunun bir Action method olmadıgını bildiriyoruz.
        [NonAction]
        //Ok.BadRequest.NoContent gibi dönüş tipleri yerine CustomResponseDto<T> ile statusCode ve datayı dönebilecegimiz yapı oluşturduk.
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> responseDto)
        {
            //Bu method cagrıldıgında response status code ne ise ona göre dönüş yapıyoruz.
            //204 ise null  204 degilse response u dönüyoruz.
            if (responseDto.StatusCode==204)
                return new ObjectResult(null) { StatusCode = responseDto.StatusCode };

                return new ObjectResult(responseDto) { StatusCode = responseDto.StatusCode };
        }
    }
}
