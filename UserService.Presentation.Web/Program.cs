using UserService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.Services.AddAutoMigration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// 1. Создаем IBus
// 2. Создаем новый интерфейс IServiceBus с методами для отправки сообщений
// 3. Добавить реализацию в infrastructure
// 4. Создать проект common с общими моделями для общения
// 5. Добавить использование реализации в AuthService