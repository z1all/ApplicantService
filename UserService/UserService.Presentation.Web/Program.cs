using UserService.Infrastructure.Identity;
using UserService.Presentation.Web.RPCHandlers;
using Common.API.Middlewares.Extensions;
using Common.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configuration extensions
builder.Services.AddSwaggerConfigure();
builder.Services.AddModalStateConfigure();
builder.Services.AddRPCHandlers();

var app = builder.Build();

// EasyNetQAutoSubscriber extensions
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();