using System.Reflection;

using ContoroPizza.Interfaces;

using ContosoPizza.Data;
using ContosoPizza.Services;

using Microsoft.EntityFrameworkCore;

using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// *************** DATABASE ***************
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Try to use PostgreSQL server
try
{
    using var testConn = new NpgsqlConnection(connectionString);
    testConn.Open(); // throws if DB is unavailable
    testConn.Close();

    builder.Services.AddDbContext<ContosoPizzaDbContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("Using PostgreSQL database.");
}
catch
{
    builder.Services.AddDbContext<ContosoPizzaDbContext>(options =>
        options.UseInMemoryDatabase("ContosoPizzaDbDev"));
    Console.WriteLine("PostgreSQL unavailable. Falling back to In-Memory database.");
}


// *** In Memory Database ***
// builder.Services.AddDbContext<ContosoPizzaDbContext>(options => options.UseInMemoryDatabase("items"));

// *** Persistent database (using Db file) ***
// var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";
// builder.Services.AddSqlite<ContosoPizzaDbContext>(connectionString);

// *** Postgres sql docker container ***
// builder.Services.AddDbContext<ContosoPizzaDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped<IPizzaService, PizzaService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "Contoso Pizza API",
                Description = "A simple web API with CRUD operations to get familiar with ASP.NET Core",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

var app = builder.Build();

// Apply migrations automatically in Development env

using var scope = app.Services.CreateScope();
var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

if (env.IsDevelopment())
{
    var db = scope.ServiceProvider.GetRequiredService<ContosoPizzaDbContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
        Console.WriteLine("Applying database migrations...");
    }
    else
    {
        Console.WriteLine("Skipping migrations — using non-relational database.");
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
