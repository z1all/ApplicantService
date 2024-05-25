using LoggerService.Infrastructure.Persistence;
using LoggerService.Presentation.Web;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServiceExtensions();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Services
app.Services.AddAutoMigration();
app.Services.UsePresentationServiceExtensions();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();