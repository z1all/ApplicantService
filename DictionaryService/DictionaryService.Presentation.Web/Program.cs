using DictionaryService.Infrastructure.Persistence;
using DictionaryService.Infrastructure.ExternalService;
using DictionaryService.Core.Application;
using DictionaryService.Presentation.Web;
using DictionaryService.Presentation.Web.RPCHandlers;
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
builder.Services.AddInfrastructureExternalServices();
builder.Services.AddPresentationServices();
builder.Services.AddEasyNetQAutoSubscriber("DictionaryService");
builder.Services.AddRPCHandlers();

var app = builder.Build();

// EasyNetQAutoSubscriber extensions
app.Services.UseEasyNetQAutoSubscriber(Assembly.GetExecutingAssembly());
app.Services.UseRPCHandlers();

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
