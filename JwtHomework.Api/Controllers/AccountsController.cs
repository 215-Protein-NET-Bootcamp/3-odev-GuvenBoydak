using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JwtHomework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : CustomBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Account> accounts = await _accountService.GetActivesAsync();

            List<AccountListDto> accountListDtos = _mapper.Map<List<AccountListDto>>(accounts);

            return CreateActionResult(CustomResponseDto<List<AccountListDto>>.Success(200,accountListDtos, "İşlem başarılı"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            Account account = await _accountService.GetByIdAsync(id);

            AccountDto accountDto = _mapper.Map<AccountDto>(account);

            return CreateActionResult(CustomResponseDto<AccountDto>.Success(200, accountDto, "İşlem başarılı"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDto accountRegisterDto)
        {
            //kulanıcı kayıt ediyoruz.Böyle bir Kullanıcı olup olmadıgını Bussines katmanında kontrol ediyoruz.
            Account registerAccount =await _accountService.RegisterAsync(accountRegisterDto);

            //kayıt olan kullanıcı bilgileriyle bir Token yaratıyoruz.
            AccessToken token = _accountService.CreateAccessToken(registerAccount);

            Account account=_mapper.Map<Account>(accountRegisterDto);

            //RabbitMQ kullanarak kayıt olan kullanıcı biligilerini kuyruga gönderiyoruz ve Consumer bu bilgileri kuyruktan alıp bir mail gönderme işlemi yapıyor.
            ProducerService.Producer(account);

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200,token, "İşlem başarılı"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto accountLoginDto)
        {
            //Kulnıcıyı login ediyoruz.Böyle bir Kullanıcı olup olmadıgını Bussines katmanında kontrol ediyoruz.
            Account account = await _accountService.LoginAsync(accountLoginDto);

            //Login olan kullanıcı bilgileriyle bir Token yaratıyoruz.
            AccessToken token = _accountService.CreateAccessToken(account);

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200, token, "İşlem başarılı"));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccountUpdateDto accountUpdateDto)
        {
            Account account=_mapper.Map<Account>(accountUpdateDto);

            await _accountService.UpdateAsync(account);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "İşlem başarılı"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Account account =await _accountService.GetByIdAsync(id);

            await _accountService.RemoveAsync(account);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "İşlem başarılı"));
        }

        [HttpPut("changePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(AccountPasswordUpdateDto accountPasswordUpdateDto)
        {
            if(!CheckPassword.CheckingPassword(accountPasswordUpdateDto.NewPassword,accountPasswordUpdateDto.ConfirmPassword))
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(404,"Girilen şifreler uyuşmuyor."));

            //user claimlerinden Accountid'nin degerini alıyoruz.
            string accountId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;

            
            await _accountService.UpdatePasswordAsync(int.Parse(accountId),accountPasswordUpdateDto);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "İşlem başarılı"));
        }
    }
}
