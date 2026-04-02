using System.Text.Json.Serialization; // <--- Добавьте этот using в начало файла

var builder = WebApplication.CreateBuilder(args);

// ИЗМЕНЕННЫЙ БЛОК:
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Эта настройка заставляет игнорировать циклические ссылки при сериализации
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Опционально: игнорировать свойства с null значениями (чтобы JSON был чище)
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    // ... ваш код Swagger ...
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "KeePass.xml");
    if (File.Exists(xmlPath))
    {
        option.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.UseSwagger();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Инструментарий");
});
app.Run();

