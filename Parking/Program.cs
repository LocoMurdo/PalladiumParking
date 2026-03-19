
using DotEnv.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Parking.API.scr.HostAplication.Extensions;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Shared.Configurations;

var envVars = new EnvLoader().Load();   
var appsetings = new EnvBinder(envVars).Bind<AppSettings>(); 
var builder = WebApplication.CreateBuilder(args);



/*Database conection*/
builder.Services.AddDbContext<DbContext, AppDbContext>(options =>
    options.UseSqlServer("name = DefaultConnection")); 






// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddSingleton(appsetings)
    .AddInfraestructureServices()
    .RegisterAutoDependencies();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddValidators();





/*
 add all services about authorizacion
 */


builder.Services.AddAuthenticationJwtBearer(appsetings);








var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "BarberApi V1"));
    IdentityModelEventSource.ShowPII = true;
}
else
{
    app.UseExceptionHandler("/");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets()
   .UsePathBase(new PathString("/api"))
   .UseRouting()
   .UseCors(options =>
   {
       options.AllowAnyOrigin();
       options.AllowAnyMethod();
       options.AllowAnyHeader();
   })
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints => endpoints.MapControllers());


app.Run();
