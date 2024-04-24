using AdmissioningService.Core.Application;
using AdmissioningService.Infrastructure.Persistence;
using AdmissioningService.Presentation.Web;
using Common.API.Middlewares.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();

var app = builder.Build();

// AppDbContext extensions
app.Services.AddAutoMigration();

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
