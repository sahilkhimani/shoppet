using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using shoppetApi.Controllers;
using shoppetApi.Helper;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

builder.Services.AddScoped(typeof(IGenericController<,,>), typeof(GenericController<,,>));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.RegisterDb(builder.Configuration);
builder.Services.JwtAuth(builder.Configuration);
//builder.Services.AuthPolicy();
builder.Services.RegisterUnitOfWork();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
