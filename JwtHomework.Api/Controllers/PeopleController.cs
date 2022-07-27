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
    public class PeopleController : CustomBaseController
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

            return CreateActionResult(CustomResponseDto<List<PersonListDto>>.Success(200, peopleListDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
           Person person = await _personService.GetByIdAsync(id);
            if (person == null)
                return BadRequest();

            PersonDto personDto=_mapper.Map<PersonDto>(person);

            return CreateActionResult(CustomResponseDto<PersonDto>.Success(200, personDto));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]PersonAddDto personAddDto)
        {
            Person person = _mapper.Map<Person>(personAddDto);

            await _personService.InsertAsync(person);

            //Client a data dönmiyecegimiz yerde NoContentDto kullanarak statusCode dönüyoruz.
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody]PersonUpdateDto personUpdateDto)
        {
            Person person = _mapper.Map<Person>(personUpdateDto);

            await _personService.UpdateAsync(person);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            Person person = await _personService.GetByIdAsync(id);

            Person deletePerson = _mapper.Map<Person>(person);

            await _personService.RemoveAsync(deletePerson);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
