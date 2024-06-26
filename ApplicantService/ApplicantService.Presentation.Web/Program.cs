using ApplicantService.Core.Application;
using ApplicantService.Infrastructure.Persistence;
using ApplicantService.Presentation.Web;
using ApplicantService.Presentation.Web.RPCHandlers;
using Common.API.Middlewares.Extensions;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();
builder.Services.AddApplicationServices();
builder.Services.AddEasyNetQAutoSubscriber("ApplicantService");
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