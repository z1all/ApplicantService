using ApplicantService.Core.Application;
using ApplicantService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers() // Перенести в конфигурацию
builder.Services
    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen() Перенести в конфигурацию
builder.Services.AddSwaggerGen(options =>
{
    options.UseDateOnlyTimeOnlyStringConverters();
});

// Service extensions
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// AppDbContext extensions
app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
