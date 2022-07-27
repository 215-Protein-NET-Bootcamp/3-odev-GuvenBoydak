using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtHomework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PeopleController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Person> people = await _personService.GetActivesAsync();

            List<PersonListDto> peopleListDto = _mapper.Map<List<PersonListDto>>(people);

            return Ok(peopleListDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           Person person = await _personService.GetByIdAsync(id);
            if (person == null)
                return BadRequest();

            PersonDto personDto=_mapper.Map<PersonDto>(person);

            return Ok(personDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonAddDto personAddDto)
        {
            Person person = _mapper.Map<Person>(personAddDto);

            await _personService.InsertAsync(person);

            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Update(PersonUpdateDto personUpdateDto)
        {
            Person person = _mapper.Map<Person>(personUpdateDto);

            await _personService.UpdateAsync(person);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Person person = await _personService.GetByIdAsync(id);

            Person deletePerson = _mapper.Map<Person>(person);

            await _personService.RemoveAsync(deletePerson);

            return Ok();

        }
    }
}
