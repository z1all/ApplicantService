using UserService.Infrastructure.Identity;
using Common.Configurations.Extensions;
using Common.Middlewares.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services extensions
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configuration extensions
builder.Services.AddSwaggerConfigure();
builder.Services.AddModalStateConfigure();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();