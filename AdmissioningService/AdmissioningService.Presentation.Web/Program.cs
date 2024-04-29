using AdmissioningService.Core.Application;
using AdmissioningService.Infrastructure.Persistence;
using AdmissioningService.Presentation.Web;
using AdmissioningService.Presentation.Web.RPCHandlers;
using Common.API.Middlewares.Extensions;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();
builder.Services.AddEasyNetQAutoSubscriber("AdmissionService");
builder.Services.AddRPCHandlers();

var app = builder.Build();

// EasyNetQAutoSubscriber extensions
app.Services.UseEasyNetQAutoSubscriber(Assembly.GetExecutingAssembly());
app.Services.UseRPCHandlers();

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
