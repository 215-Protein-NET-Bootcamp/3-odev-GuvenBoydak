
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

        public  AccessToken CreateAccessToken(Account entity)
        {
            //Token oluşturuyoruz.
            AccessToken accessToken = _tokenHelper.CreateToken(entity);

            return accessToken;
        }

        public async Task<List<Account>> GetActivesAsync()
        {
            return  await _accountRepository.GetActiveAsync();
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            Account account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new InvalidOperationException($"{typeof(Account).Name}({id}) Not Found ");

            return account;
        }

        public async Task<Account> GetByUserAsync(string userName)
        {
            Account account = await _accountRepository.GetByUserAsync(userName);

            if(account == null)
                throw new InvalidOperationException($"{typeof(Account).Name}({userName})User Not Found");
            return account;
        }

        public async Task InsertAsync(Account entity)
        {
          await _accountRepository.AddAsync(entity);
        }

        public async Task<Account> LoginAsync(AccountLoginDto entity)
        {
            Account account = await _accountRepository.GetByUserAsync(entity.UserName);

            if (account == null)
                throw new InvalidOperationException($"{typeof(Account).Name}({entity.UserName}) User Not Found");

            //LastActivity güncelliyoruz.
            account.LastActivity = DateTime.Now;
            await _accountRepository.UpdateAsync(account);

            //Kullanıcının girdigi password ile database oluşturulan passwordHash ve passwordSalt kontrol ediyoruz.
            if (!HashingHelper.VerifyPasswordHash(entity.Password,account.PasswordHash,account.PasswordSalt))
                throw new InvalidOperationException($"{typeof(Account).Name} User Password Does Not Match ");
            return account;
        }

        public async Task<Account> RegisterAsync(AccountRegisterDto entity)
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

            try
            {
                await _accountRepository.AddAsync(account);
            }
            catch (Exception)
            {

                throw new Exception($"Register_Error {typeof(Person).Name}");
            }
            return account;
        }

        public async Task RemoveAsync(Account entity)
        {
            try
            {
                await _accountRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw new Exception($"Delete_Error {typeof(Account).Name}");
            }      
        }

        public async  Task UpdateAsync(Account entity)
        {
            try
            {
                await _accountRepository.UpdateAsync(entity);
            }
            catch (Exception)
            {

                throw new Exception($"Update_Error {typeof(Account).Name}");
            }      
        }

        public async Task UpdatePasswordAsync(int id,AccountPasswordUpdateDto entity)
        {
            Account account =await _accountRepository.GetByIdAsync(id);

            //Kullanıcının girdigi password ile database oluşturulan passwordHash ve passwordSalt kontrol ediyoruz.
            if (!HashingHelper.VerifyPasswordHash(entity.OldPassword, account.PasswordHash, account.PasswordSalt))
                throw new InvalidOperationException($"{typeof(Account).Name} User Password Does Not Match ");

            //PasswordHash oluşturuyoruz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(entity.NewPassword, out passwordHash, out passwordSalt);

            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            await _accountRepository.UpdateAsync(account);
        }
    }
}
