using shoppetApi.Controllers;
using shoppetApi.Helper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericController<,,>), typeof(GenericController<,,>));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.RegisterDb(builder.Configuration);
builder.Services.JwtAuth(builder.Configuration);
builder.Services.AuthPolicy();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
