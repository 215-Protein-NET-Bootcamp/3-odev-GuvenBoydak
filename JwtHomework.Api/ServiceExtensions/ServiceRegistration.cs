using AutoMapper;
using JwtHomework.Base;
using JwtHomework.Business;
using JwtHomework.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

        public static void AddJwtBearerAuthentication(this IServiceCollection services,IConfiguration configuration)
        {

            //Yetkilendirme olarak jwtBearer kullanılcagını belirtıyoruz.
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, 
                    ValidIssuer = configuration.GetSection("Issuer").ToString(),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("SecurityKey").ToString())),
                    ValidAudience = configuration.GetSection("Audience").ToString(),
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
        }

    }
}
