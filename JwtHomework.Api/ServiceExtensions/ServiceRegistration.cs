using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.DataAccess;

namespace JwtHomework.Api
{
    public static class ServiceRegistration
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<DapperHomeworkDbContext>();
            services.AddScoped<IAccountRepository, AccountRespository>();
            services.AddScoped<IPersonRepository,PersonRepository>();
            services.AddScoped<IPersonService,PersonService>();
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<ITokenHelper,JWTHelper>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

    }
}
