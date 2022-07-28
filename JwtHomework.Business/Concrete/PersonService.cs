using JwtHomework.Base;
using JwtHomework.DataAccess;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;



        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<Person>> GetActivesAsync()
        {
            return await _personRepository.GetActiveAsync();
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await _personRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Person>> GetByAccountIdAsync(int id)
        {
            return await _personRepository.GetByAccountIdAsync(id);
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            Person person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                throw new InvalidOperationException($"{typeof(Person).Name}({id}) Not Found ");
            return person;
        }

        public async Task InsertAsync(Person entity)
        {
            try
            {
                await _personRepository.AddAsync(entity);
            }
            catch (Exception)
            {

                throw new Exception($"Save_Error {typeof(Person).Name}");
            }
            
        }

        public async Task RemoveAsync(Person entity)
        {
            try
            {
                await _personRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw new Exception($"Delete_Error {typeof(Person).Name}");
            }
            
        }

        public async Task UpdateAsync(Person entity)
        {
            try
            {
                await _personRepository.UpdateAsync(entity);
            }
            catch (Exception)
            {

                throw new Exception($"Update_Error {typeof(Person).Name}");
            }
            
        }
    }
}
