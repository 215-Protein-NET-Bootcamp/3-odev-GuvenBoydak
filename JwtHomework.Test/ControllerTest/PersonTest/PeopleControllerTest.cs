using AutoMapper;
using JwtHomework.Api.Controllers;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JwtHomework.Test
{
    public class PeopleControllerTest
    {
        private readonly Mock<IPersonService> _personService;
        private readonly IMapper _mapper;
        private readonly PeopleController _peopleController;

        public PeopleControllerTest()
        {
            MapperConfiguration map = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new MapProfile());
            });
            _mapper = map.CreateMapper();
            _personService = new Mock<IPersonService>();
            _peopleController = new PeopleController(_personService.Object, _mapper);
        }


        List<Person> people = new List<Person>()
        {
            new Person(){Id=1, FirstName="güven",LastName="boydak",Email="guven@gmail.com",Phone="5555555555",Description="bu bir test",DateOfBirth=DateTime.Now,AccountId=1 },
             new Person(){Id=2,FirstName="aylin",LastName="boydak",Email="aylin@gmail.com",Phone="5555555554",Description="bu bir test",DateOfBirth=DateTime.Now,AccountId=2 },
              new Person(){Id=3,FirstName="ali",LastName="boydak",Email="ali@gmail.com",Phone="5555555553",Description="bu bir test",DateOfBirth=DateTime.Now,AccountId=3 },
        };


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecutes_Retunr200OkWithPerson(int id)
        {
            Person person = people.FirstOrDefault(x => x.Id == id);

            _personService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(person);

            IActionResult result = await _peopleController.GetById(id);

            _personService.Verify(x => x.GetByIdAsync(id), Times.Once);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(200, objectResult.StatusCode.Value);

            CustomResponseDto<PersonDto> resultType = Assert.IsAssignableFrom<CustomResponseDto<PersonDto>>(objectResult.Value);
        }

        [Fact]
        public async void Update_ActionExecutes_Return204NoContent()
        {
            _personService.Setup(x => x.UpdateAsync(people[0]));

            PersonUpdateDto person = _mapper.Map<PersonUpdateDto>(people[0]);

            IActionResult result = await _peopleController.Update(person);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(204, objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Add_ActionExecutes_Return204NoContent()
        {
            Person person = new Person() { FirstName = "sedat", LastName = "fare", Email = "sedat@gmail.com", Phone = "5555555551", Description = "bu bir test", DateOfBirth = DateTime.Now, AccountId = 1 };

            PersonAddDto personAddDto = _mapper.Map<PersonAddDto>(person);

            _personService.Setup(x => x.InsertAsync(person));

            IActionResult result = await _peopleController.Add(personAddDto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(204, objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Delete_ActionExecutes_Return204NoContent()
        {
            int id = 1;
            Person person = people.FirstOrDefault(x => x.Id == id);
            _personService.Setup(x => x.GetByIdAsync(id));

            _personService.Setup(x => x.RemoveAsync(person));

            IActionResult result = await _peopleController.Delete(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(204, objectResult.StatusCode.Value);
        }
    }
}
