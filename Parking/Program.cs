using DotEnv.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Parking.API.scr.HostAplication.Extensions;
using Parking.API.scr.HostAplication.Middleware;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Shared.Configurations;

var envVars = new EnvLoader().Load();   
var appsetings = new EnvBinder(envVars).Bind<AppSettings>(); 
var builder = WebApplication.CreateBuilder(args);



/*Database conection*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbContext, AppDbContext>(options =>
    options.UseSqlServer(connectionString)); 


/* Kestrel configuration */
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5050);
    options.Limits.MaxRequestBodySize = 1 * 1024 * 1024; // 1 MB max request body
});





// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services
    .AddSingleton(appsetings)
    .AddInfraestructureServices()
    .RegisterAutoDependencies();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddValidators();

// Rate limiting
builder.Services.AddRateLimiting();



/*
 add all services about authorizacion
 */


builder.Services.AddAuthenticationJwtBearer(appsetings);








var app = builder.Build();

// Security headers
app.UseMiddleware<SecurityHeadersMiddleware>();

// Rate limiting
app.UseRateLimiter();

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

app.UseWebSockets()
   .UsePathBase(new PathString("/api"))
   .UseRouting()
   .UseCors(options =>
   {
       options.WithOrigins(
           "http://localhost:5173",
           "http://localhost:3000",
           "http://187.124.236.172"
       )
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials();
   })
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints => endpoints.MapControllers());



app.Run();
