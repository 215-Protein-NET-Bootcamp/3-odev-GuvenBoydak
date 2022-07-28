using FluentValidation.AspNetCore;
using JwtHomework.Api;
using JwtHomework.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//Default FluentValidation Filterini devre dışı bırakıp kendi yazdıgımız ValidatorFilterAttribute u ekliyoruz.
builder.Services.AddControllers(option => option.Filters.Add<ValidatorFilterAttribute>()).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<JwtHomework.Business.PersonAddDtoValidator>());

//Extension Service Injection
builder.Services.AddDependencyInjection();
builder.Services.AddCustomizeSwagger();


//jwt 
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Serilog
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Application is starting.");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1); // Remove Schema on Swagger UI
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patika");
    c.DocumentTitle = "Patika";
});

app.UseHttpsRedirection();

//Exceptionları handler etigimiz middleware
app.UseCustomExeption();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
