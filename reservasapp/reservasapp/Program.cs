using Microsoft.EntityFrameworkCore;
using reservasapp.datos;
using reservasapp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync(); // Aplica cualquier migración pendiente
    await ApplicationDbContextSeed.SeedAsync(dbContext); // Insertar datos de ejemplo si es necesario
}
app.Run();
public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        DateTime fechaEspecifica = new DateTime(2027, 12, 30); // 30 de diciembre de 2027

        // Si no hay productos, agregar algunos datos iniciales
        if (!context.Servicio.Any())
        {
            var servicios = new List<Servicio>
            {
                new Servicio {Nombre = "Peluqueria", Precio = 9000, Duracion=new TimeSpan(1,0,00)},
            };

            await context.Servicio.AddRangeAsync(servicios);
            await context.SaveChangesAsync();
        }
    }
}