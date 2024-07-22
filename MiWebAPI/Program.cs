//**
using MyWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Registrar EmpleadoData como un servicio singleton
builder.Services.AddSingleton<EmpleadoData>();

//La política "NuevaPolitica" permitirá solicitudes desde cualquier origen, con cualquier método y encabezado
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Añadir Politica
app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
