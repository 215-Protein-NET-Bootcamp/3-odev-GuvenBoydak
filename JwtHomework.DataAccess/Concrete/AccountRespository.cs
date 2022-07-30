using Dapper;
using JwtHomework.Entities;
using System.Data;

namespace JwtHomework.DataAccess
{
    public class AccountRespository : IAccountRepository
    {
        private readonly DapperHomeworkDbContext _db;

        public AccountRespository(DapperHomeworkDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Account entity)
        {
            using (IDbConnection con=_db.CreateConnection())
            {
                await con.ExecuteAsync("insert into  \"Accounts\" ( \"UserName\", \"PasswordHash\" , \"PasswordSalt\", \"Name\", \"Email\",\"LastActivity\",\"CreatedDate\",\"Status\") VALUES (@username,@passwordhash,@passwordsalt,@name,@email,@lastactivity,@createddate,@status)",
                    new
                    {
                        username=entity.UserName,
                        passwordhash=entity.PasswordHash, 
                        passwordsalt=entity.PasswordSalt,
                        name=entity.Name,
                        email=entity.Email,
                        lastactivity=entity.LastActivity,
                        createddate=entity.CreatedDate,
                        status=entity.Status
                    });
            }
        }

        public async Task DeleteAsync(Account entity)
        {
            Account account = await GetByIdAsync(entity.Id);
            account.DeletedDate = DateTime.Now;
            account.Status = DataStatus.Deleted;
            await UpdateAsync(account);
        }

        public async Task<List<Account>> GetActiveAsync()
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                IEnumerable<Account> accounts= await con.QueryAsync<Account>("select * from \"Accounts\" where \"Status\" != '2'");
                return accounts.ToList();
            }
        }

        public async Task<List<Account>> GetAllAsync()
        {
            using (IDbConnection con =_db.CreateConnection())
            {
                IEnumerable<Account> accounts= await con.QueryAsync<Account>("select * from \"Accounts\" ");
                return accounts.ToList();
            }
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            using (IDbConnection con=_db.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Account>("select * from  \"Accounts\" where \"Id\" = @id", new { id = id });
            }
        }

        public async Task<Account> GetByUserAsync(string userName)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Account>("select * from  \"Accounts\" where \"UserName\" = @username", new { username = userName });
               
            }
        }

        public async Task UpdateAsync(Account entity)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                //DeletedDate null degilse bir silme işleminin update edildigi anlayıp status'u deleted yapıp pasif delete yapıyoruz.
                if (entity.DeletedDate != null)
                {
                    con.Execute("update \"Accounts\" set \"UserName\" = @username, \"PasswordHash\" = @passwordhash, \"PasswordSalt\" = @passwordsalt, \"Email\" = @email, \"Name\" = @name, \"LastActivity\" = @lastactivity,\"DeletedDate\" = @deleteddate,\"Status\"= @status where \"Id\" = @id", new
                    {
                        id = entity.Id,
                        username = entity.UserName,
                        passwordhash = entity.PasswordHash,
                        passwordsalt =entity.PasswordSalt,
                        email = entity.Email,
                        name = entity.Name,
                        lastactivity = entity.LastActivity,
                        deleteddate = entity.DeletedDate,
                        status = entity.Status
                    });
                }
                else //DeletedDate boş ise bir update işlemi olucagı için updateddate'ini verip status'u update e çekiyoruz.
                {
                    entity.UpdatedDate = DateTime.Now;
                    entity.Status = DataStatus.Updated;

                    Account updateAccount= await GetByIdAsync(entity.Id);

                    entity.UserName = updateAccount.UserName != default ? entity.UserName : updateAccount.UserName;
                    entity.PasswordHash = updateAccount.PasswordHash != default ? entity.PasswordHash : updateAccount.PasswordHash;
                    entity.Email = updateAccount.Email != default ? entity.Email : updateAccount.Email;
                    entity.Name = updateAccount.Name != default ? entity.Name : updateAccount.Name;
                    entity.LastActivity = updateAccount.LastActivity != default ? entity.LastActivity : updateAccount.LastActivity;
                    entity.UpdatedDate = updateAccount.UpdatedDate != default ? entity.UpdatedDate : updateAccount.UpdatedDate;

                    con.Execute("update \"Accounts\" set \"UserName\"= @username, \"PasswordHash\"= @passwordhash, \"PasswordSalt\"= @passwordsalt, \"Email\"= @email, \"Name\"= @name, \"LastActivity\"= @lastactivity,\"UpdatedDate\"= @updateddate,\"Status\"= @status where \"Id\"= @id", new
                    {

                        id = entity.Id,
                        username = entity.UserName,
                        passwordhash = entity.PasswordHash,
                        passwordsalt = entity.PasswordSalt,
                        email = entity.Email,
                        name = entity.Name,
                        lastactivity = entity.LastActivity,
                        updateddate = entity.UpdatedDate,
                        status = entity.Status
                    });
                }
            }
        }
    }
}
