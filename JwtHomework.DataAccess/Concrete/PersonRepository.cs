using Dapper;
using JwtHomework.Entities;
using System.Data;

namespace JwtHomework.DataAccess
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DapperHomeworkDbContext _db;

        public PersonRepository(DapperHomeworkDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Person entity)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                await con.ExecuteAsync("insert into  \"People\" ( \"FirstName\", \"LastName\", \"Email\", \"Description\", \"Phone\", \"DateOfBirth\",\"AccountId\",\"CreatedDate\",\"Status\") VALUES (@firstname,@lastname,@email,@description,@phone,@dateofbirth,@accountId,@createddate,@status)",
                    new
                    {
                        firstname = entity.FirstName,
                        lastname = entity.LastName,
                        email = entity.Email,
                        phone = entity.Phone,
                        description = entity.Description,
                        dateofbirth = entity.DateOfBirth,
                        accountId = entity.AccountId,
                        createddate = entity.CreatedDate,
                        status = entity.Status
                    });
            }
        }

        public async Task DeleteAsync(Person entity)
        {
            Person person = await GetByIdAsync(entity.Id);
            person.DeletedDate = DateTime.Now;
            person.Status = DataStatus.Deleted;
            await UpdateAsync(person);
        }

        public async Task<List<Person>> GetActiveAsync()
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                IEnumerable<Person> people = await con.QueryAsync<Person>("select * from  \"People\" where \"Status\" != '2' ");
                return people.ToList();
            }
        }

        public async Task<List<Person>> GetAllAsync()
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                IEnumerable<Person> people = await con.QueryAsync<Person>("select * from  \"People\" ");
                return people.ToList();
            }
        }

        public async Task<IEnumerable<Person>> GetByAccountIdAsync(int id)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                return await con.QueryAsync<Person>("select * from  \"People\" where \"AccountId\" =@accountid ", new { accountid = id });
            }
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Person>("select * from  \"People\" where \"Id\" = @id ", new { id = id });
            }
        }

        public async Task<List<Person>> GetPaginationAsync(int page, int limit)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                IEnumerable<Person> people = await con.QueryAsync<Person>("select * from \"People\" order by \"Id\"  limit  @limit offset  @page",
                    new
                    {
                        limit = limit,
                        page = (page - 1) * limit
                    });
                return people.ToList();
            }
        }

        public async Task UpdateAsync(Person entity)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                //DeletedDate null degilse bir silme işleminin update edildigi anlayıp status'u deleted yapıp pasif delete yapıyoruz.
                if (entity.DeletedDate != null)
                {
                    con.Execute("update \"People\" set  \"FirstName\"=@firstname, \"LastName\"=@lastname, \"Email\"=@email, \"Description\"=@description, \"Phone\"=@phone, \"DateOfBirth\"=@dateofbirth,\"DeletedDate\"=@deleteddate,\"Status\"=@status,\"AccountId\"=@accountId where \"Id\"=@id", new
                    {
                        id = entity.Id,
                        firstname = entity.FirstName,
                        lastname = entity.LastName,
                        email = entity.Email,
                        phone = entity.Phone,
                        description = entity.Description,
                        dateofbirth = entity.DateOfBirth,
                        accountId = entity.AccountId,
                        deleteddate = entity.DeletedDate,
                        status = entity.Status
                    });
                }
                else //DeletedDate boş ise bir update işlemi olucagı için updateddate'ini verip status'u update e çekiyoruz.
                {
                    entity.UpdatedDate = DateTime.Now;
                    entity.Status = DataStatus.Updated;

                    Person updatePerson = await GetByIdAsync(entity.Id);

                    entity.FirstName = updatePerson.FirstName != default ? entity.FirstName : updatePerson.FirstName;
                    entity.LastName = updatePerson.LastName != default ? entity.LastName : updatePerson.LastName;
                    entity.Email = updatePerson.Email != default ? entity.Email : updatePerson.Email;
                    entity.Phone = updatePerson.Phone != default ? entity.Phone : updatePerson.Phone;
                    entity.Description = updatePerson.Description != default ? entity.Description : updatePerson.Description;
                    entity.DateOfBirth = updatePerson.DateOfBirth != default ? entity.DateOfBirth : updatePerson.DateOfBirth;

                    con.Execute("update  \"People\" set \"FirstName\"=@firstname, \"LastName\"=@lastname, \"Email\"=@email, \"Description\"=@description, \"Phone\"=@phone, \"DateOfBirth\"=@dateofbirth,\"UpdatedDate\"=@updateddate,\"Status\"=@status,\"AccountId\"=@accountId  where \"Id\"=@id", new
                    {

                        id = entity.Id,
                        firstname = entity.FirstName,
                        lastname = entity.LastName,
                        email = entity.Email,
                        phone = entity.Phone,
                        description = entity.Description,
                        dateofbirth = entity.DateOfBirth,
                        accountId = entity.AccountId,
                        updateddate = entity.UpdatedDate,
                        status = entity.Status
                    });
                }
            }
        }
    }
}
