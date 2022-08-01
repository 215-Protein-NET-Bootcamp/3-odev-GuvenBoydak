using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Person, PersonListDto>().ReverseMap();
            CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<Person, PersonAddDto>().ReverseMap();
            CreateMap<Person, PersonUpdateDto>().ReverseMap();

            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, AccountListDto>().ReverseMap();
            CreateMap<Account, AccountUpdateDto>().ReverseMap();
            CreateMap<Account, AccountRegisterDto>().ReverseMap();

        }
    }
}
