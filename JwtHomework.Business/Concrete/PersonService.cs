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

        public async Task<CustomResponseDto<IEnumerable<Person>>> GetActivesAsync()
        {
            IEnumerable < Person > people= await _personRepository.GetActiveAsync();

            return CustomResponseDto<IEnumerable<Person>>.Success(200, people);
        }

        public async Task<CustomResponseDto<IEnumerable<Person>>> GetAllAsync()
        {
            IEnumerable<Person> people= await _personRepository.GetAllAsync();

            return CustomResponseDto<IEnumerable<Person>>.Success(200, people);
        }

        public async Task<CustomResponseDto<Person>> GetByIdAsync(int id)
        {
            Person person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return CustomResponseDto<Person>.Fail(404, $"{typeof(Person).Name}({id}) Not Found ");


            return CustomResponseDto<Person>.Success(200,person);
        }

        public async Task<CustomResponseDto<Person>> InsertAsync(Person entity)
        {
            await _personRepository.AddAsync(entity);

            return CustomResponseDto<Person>.Success(200);
        }

        public async Task<CustomResponseDto<Person>> RemoveAsync(Person entity)
        {
            await _personRepository.DeleteAsync(entity);

            return CustomResponseDto<Person>.Success(200);
        }

        public async Task<CustomResponseDto<Person>> UpdateAsync(Person entity)
        {
            await _personRepository.UpdateAsync(entity);

            return CustomResponseDto<Person>.Success(200);
        }
    }
}
