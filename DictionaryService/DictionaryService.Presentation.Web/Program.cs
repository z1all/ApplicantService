using DictionaryService.Infrastructure.Persistence;
using DictionaryService.Infrastructure.ExternalService;
using ApplicantService.Core.Application;
using DictionaryService.Presentation.Web;
using Common.Middlewares.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddInfrastructureExternalServices();
builder.Services.AddPresentationServices();

var app = builder.Build();

// AppDbContext extensions
app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

// Exceptions handler
app.UseExceptionsHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
