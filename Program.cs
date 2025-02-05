using shoppetApi.Controllers;
using shoppetApi.Filters;
using shoppetApi.Helper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();
})
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Cors();
builder.Services.AddScoped(typeof(IGenericController<,,>), typeof(GenericController<,,>));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.RegisterDb(builder.Configuration);
builder.Services.JwtAuth(builder.Configuration);
//builder.Services.AuthPolicy();
builder.Services.RegisterUnitOfWork();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("AllowAngularOrigin");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
