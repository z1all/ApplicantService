using UserService.Infrastructure.Persistence;
using UserService.Presentation.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationWebService();

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

// 1. Создаем IBus
// 2. Создаем новый интерфейс IServiceBus с методами для отправки сообщений
// 3. Добавить реализацию в infrastructure
// 4. Создать проект common с общими моделями для общения
// 5. Добавить использование реализации в AuthService