using Microsoft.EntityFrameworkCore;
using reservasapp.datos;
using reservasapp.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

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
        if (!context.Servicio.Any())
        {
            var servicios = new List<Servicio>
            {
                new Servicio {Nombre = "Peluqueria", Precio = 9000, Duracion=60},
            };

            await context.Servicio.AddRangeAsync(servicios);
            await context.SaveChangesAsync();
        }
    }
}