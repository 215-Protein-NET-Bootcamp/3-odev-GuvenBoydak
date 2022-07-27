using FluentValidation.AspNetCore;
using JwtHomework.Api;
using JwtHomework.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//Default FluentValidation Filterini devre dışı bırakıp kendi yazdıgımız ValidatorFilterAttribute u ekliyoruz.
builder.Services.AddControllers(option => option.Filters.Add<ValidatorFilterAttribute>()).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<JwtHomework.Business.PersonAddDtoValidator>());

//Extension Service Injection
builder.Services.AddDependencyInjection();
builder.Services.AddCustomizeSwagger();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

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

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
