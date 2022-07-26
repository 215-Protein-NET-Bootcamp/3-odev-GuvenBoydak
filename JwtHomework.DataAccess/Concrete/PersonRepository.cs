﻿using Dapper;
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
            using (IDbConnection con=_db.CreateConnection())
            {
                await con.ExecuteAsync("insert into  \"People\" ( \"FirstName\", \"LastName\", \"Email\", \"Description\", \"Phone\", \"DateOfBirth\",\"CreatedDate\",\"Status\") VALUES (@firstname,@lastname,@email,@description,@phone,@dateofbirth,@createddate,@status)",
                    new
                    {
                        firstname=entity.FirstName,
                        lastname=entity.LastName, 
                        email=entity.Email,
                        phone=entity.Phone,
                        description=entity.Description,
                        dateofbirth=entity.DateOfBirth,
                        createddate=entity.CreatedDate,
                        status=entity.Status
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

        public async Task<IEnumerable<Person>> GetActiveAsync()
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                return await con.QueryAsync<Person>("select * from  \"People\" where \"Status\" != '2' ");
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            using (IDbConnection con=_db.CreateConnection())
            {
              return  await con.QueryAsync<Person>("select * from  \"People\" ");
            }
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            using (IDbConnection con =_db.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Person>("select * from  \"People\" where \"Id\" =@id ",new {id=id });
            }
        }

        public async Task UpdateAsync(Person entity)
        {
            using (IDbConnection con = _db.CreateConnection())
            {
                //DeletedDate null degilse bir silme işleminin update edildigi anlayıp status'u deleted yapıp pasif delete yapıyoruz.
                if (entity.DeletedDate != null)
                {
                    con.Execute("update \"People\"  \"FirstName\"=@firstname, \"LastName\"=lastname, \"Email\"=@email, \"Description\"=@description, \"Phone\"=@phone, \"DateOfBirth\"=@dateofbirth,\"DeletedDate\"=@deleteddate,\"Status\"=@status where \"Id\"=@id", new
                    {
                        id = entity.Id,
                        firstname = entity.FirstName,
                        lastname = entity.LastName,
                        email = entity.Email,
                        phone = entity.Phone,
                        description = entity.Description,
                        dateofbirth = entity.DateOfBirth,
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
                    entity.UpdatedDate = updatePerson.UpdatedDate != default ? entity.UpdatedDate : updatePerson.UpdatedDate;

                    con.Execute("update  \"People\"  \"FirstName\"=@firstname, \"LastName\"=lastname, \"Email\"=@email, \"Description\"=@description, \"Phone\"=@phone, \"DateOfBirth\"=@dateofbirth,\"UpdatedDate\"=@updateddate,\"Status\"=@status where \"Id\"=@id", new
                    {

                        id = entity.Id,
                        firstname = entity.FirstName,
                        lastname = entity.LastName,
                        email = entity.Email,
                        phone = entity.Phone,
                        description = entity.Description,
                        dateofbirth = entity.DateOfBirth,
                        updateddate = entity.UpdatedDate,
                        status = entity.Status
                    });
                }
            }
        }
    }
}