using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.Entities;
using Microsoft.AspNetCore.Mvc;

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

            return CreateActionResult(CustomResponseDto<List<AccountListDto>>.Success(200,accountListDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            Account account = await _accountService.GetByIdAsync(id);

            AccountDto accountDto = _mapper.Map<AccountDto>(account);

            return CreateActionResult(CustomResponseDto<AccountDto>.Success(200, accountDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDto accountRegisterDto)
        {
            //kulanıcı kayıt ediyoruz.Böyle bir Kullanıcı olup olmadıgını Bussines katmanında kontrol ediyoruz.
            Account registerAccount =await _accountService.RegisterAsync(accountRegisterDto);

            //kayıt olan kullanıcı bilgileriyle bir Token yaratıyoruz.
            AccessToken token = await _accountService.CreateAccessTokenAsync(registerAccount);

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200,token));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto accountLoginDto)
        {
            //Kulnıcıyı login ediyoruz.Böyle bir Kullanıcı olup olmadıgını Bussines katmanında kontrol ediyoruz.
            Account account = await _accountService.LoginAsync(accountLoginDto);

            //Login olan kullanıcı bilgileriyle bir Token yaratıyoruz.
            AccessToken token = await _accountService.CreateAccessTokenAsync(account);

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200, token));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccountUpdateDto accountUpdateDto)
        {
            Account account=_mapper.Map<Account>(accountUpdateDto);

            await _accountService.UpdateAsync(account);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Account account =await _accountService.GetByIdAsync(id);

            await _accountService.RemoveAsync(account);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }




    }
}
