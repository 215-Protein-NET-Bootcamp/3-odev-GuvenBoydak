using AutoMapper;
using JwtHomework.Api.Controllers;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JwtHomework.Test
{
    public class AccountsControlerTest
    {
        private readonly Mock<IAccountService> _accountService;
        private readonly IMapper _mapper;
        private readonly AccountsController _accountsConroller;

        public AccountsControlerTest()
        {
            MapperConfiguration map = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new MapProfile());
            });
            _mapper = map.CreateMapper();
            _accountService = new Mock<IAccountService>();
            _accountsConroller = new AccountsController(_accountService.Object, _mapper);

        }


        List<Account> accounts = new List<Account>
        {
            new Account()
            {
                  Id=1,
                  UserName= "guven",
                  Email="guven@gmail.com",
                  Name="Guven",
                  LastActivity=DateTime.Now,
                  Status=DataStatus.Insearted
            },
             new Account()
            {
                  Id=2,
                  UserName= "aylin",
                  Email="aylin@gmail.com",
                  Name="Aylin",
                  LastActivity=DateTime.Now,
                  Status=DataStatus.Insearted
            },
              new Account()
            {
                  Id=3,
                  UserName= "tugrul",
                  Email="tugrul@gmail.com",
                  Name="Tugrul",
                  LastActivity=DateTime.Now,
                  Status=DataStatus.Insearted
            }

        };


        [Fact]
        public async void GetAll_ActionExecutes_Return200OkWihtAccounts()
        {
            _accountService.Setup(x => x.GetActivesAsync()).ReturnsAsync(accounts);

            IActionResult result = await _accountsConroller.GetAll();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200,objectResult.StatusCode.Value);

            CustomResponseDto<List<AccountListDto>> returnAccountListDto = Assert.IsAssignableFrom <CustomResponseDto<List<AccountListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, returnAccountListDto.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecutes_Return200OktWithAccount(int id)
        {
            Account account = accounts.FirstOrDefault(x => x.Id == id);

            _accountService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(account);

            IActionResult result =await _accountsConroller.GetById(id);

            ObjectResult objectResult=Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200,objectResult.StatusCode.Value);

            CustomResponseDto<AccountDto> returnAccountDto = Assert.IsAssignableFrom<CustomResponseDto<AccountDto>>(objectResult.Value);
        }

        [Fact]
        public async void Update_ActionExecutes_Return204NoContent()
        {
            _accountService.Setup(x => x.UpdateAsync(accounts[0]));

            AccountUpdateDto accountUpdateDto=_mapper.Map<AccountUpdateDto>(accounts[0]);

            IActionResult result =await _accountsConroller.Update(accountUpdateDto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204,objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Register_ActionExecutes_Return200OkWithAccessToken()
        {
            Account account = new Account()
            {
                UserName = "Veli",
                Email = "veli@gmail.com",
                Name = "Veli",
                LastActivity = DateTime.Now,
                Status = DataStatus.Insearted
            };

            AccountRegisterDto accountRegisterDto = _mapper.Map<AccountRegisterDto>(account);

            _accountService.Setup(x => x.RegisterAsync(accountRegisterDto));

            IActionResult result = await _accountsConroller.Register(accountRegisterDto);

            _accountService.Verify(x => x.RegisterAsync(accountRegisterDto),Times.Once);

            ObjectResult objectResult=Assert.IsType<ObjectResult>(result);

            Assert.IsType<CustomResponseDto<AccessToken>>(objectResult.Value);

            Assert.Equal<int>(200,objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Login_ActionExecutes_Return200OkWithAccessToken()
        {
            AccountLoginDto accountLoginDto = new AccountLoginDto() { UserName = "guven", Password = "12345gb" };

            _accountService.Setup(x => x.LoginAsync(accountLoginDto));

            IActionResult result = await _accountsConroller.Login(accountLoginDto);

            _accountService.Verify(x => x.LoginAsync(accountLoginDto));

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Delete_ActionExecutes_Return204NoContent()
        {
            int id = 1;
            Account account = accounts.FirstOrDefault(x=>x.Id==id);

            _accountService.Setup(x => x.GetByIdAsync(id));

            _accountService.Setup(x => x.RemoveAsync(account));

            IActionResult result =await _accountsConroller.Delete(id);

            ObjectResult objectResult=Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }
    }
}
