
using JwtHomework.Base;
using JwtHomework.DataAccess;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenHelper _tokenHelper;

        public AccountService(IAccountRepository accountRepository, ITokenHelper tokenHelper)
        {
            _accountRepository = accountRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<CustomResponseDto<AccessToken>> CreateAccessToken(Account entity)
        {
            //Kullanıcı varmı kontrol ediyoruz.
            Account account = await _accountRepository.GetByUserAsync(entity.UserName);
            if (account == null)
                return CustomResponseDto<AccessToken>.Fail(404,"User Not Found");

            //LastActivity güncelliyoruz.
            account.LastActivity = DateTime.Now;
            await _accountRepository.UpdateAsync(account);


            //Token oluşturuyoruz.
            AccessToken accessToken = _tokenHelper.CreateToken(account);

            return CustomResponseDto<AccessToken>.Success(200,accessToken);
        }

        public async Task<CustomResponseDto<IEnumerable<Account>>> GetActivesAsync()
        {
            IEnumerable<Account> accounts= await _accountRepository.GetActiveAsync();

            return CustomResponseDto<IEnumerable<Account>>.Success(200, accounts);
        }

        public async Task<CustomResponseDto<IEnumerable<Account>>> GetAllAsync()
        {
            IEnumerable<Account> accounts= await _accountRepository.GetAllAsync();

            return CustomResponseDto<IEnumerable<Account>>.Success(200, accounts);
        }

        public async Task<CustomResponseDto<Account>> GetByIdAsync(int id)
        {
            Account account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                return CustomResponseDto<Account>.Fail(404, $"{typeof(Account).Name}({id}) Not Found ");

            return CustomResponseDto<Account>.Success(200, account);
        }

        public async Task<CustomResponseDto<Account>> GetByUserAsync(string userName)
        {
            Account account = await _accountRepository.GetByUserAsync(userName);

            if(account == null)
                return CustomResponseDto<Account>.Fail(404,"User Not Found");


            return CustomResponseDto<Account>.Success(200, account);
        }

        public async Task<CustomResponseDto<Account>> InsertAsync(Account entity)
        {
            await _accountRepository.AddAsync(entity);

            return CustomResponseDto<Account>.Success(200);
        }

        public async Task<CustomResponseDto<Account>> Login(AccountLoginDto entity)
        {
            Account account = await _accountRepository.GetByUserAsync(entity.UserName);

            if (account == null)
                return CustomResponseDto<Account>.Fail(404, "User Not Found");

            //Kullanıcının girdigi password ile database oluşturulan passwordHash ve passwordSalt kontrol ediyoruz.
            if(!HashingHelper.VerifyPasswordHash(entity.Password,account.PasswordHash,account.PasswordSalt))
                return CustomResponseDto<Account>.Fail(404, "User Password Does Not Match ");

            return CustomResponseDto<Account>.Success(200);
        }

        public async Task<CustomResponseDto<Account>> RegisterAsync(AccountRegisterDto entity)
        {
            //PasswordHash oluşturuyoruz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(entity.Password, out passwordHash, out passwordSalt);

            Account account = new Account
            {
                UserName = entity.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = entity.Email,
                Name = entity.Name,
                LastActivity = DateTime.Now,
            };
            await _accountRepository.AddAsync(account);

            return CustomResponseDto<Account>.Success(200);
        }

        public async Task<CustomResponseDto<Account>> RemoveAsync(Account entity)
        {
            await _accountRepository.DeleteAsync(entity);

            return CustomResponseDto<Account>.Success(200);
        }

        public async  Task<CustomResponseDto<Account>> UpdateAsync(Account entity)
        {
            await _accountRepository.UpdateAsync(entity);

            return CustomResponseDto<Account>.Success(200);
        }
    }
}
