using CommandAPI.Data;
using CommandAPI.Repos;
using CommandAPI.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conn = new NpgsqlConnectionStringBuilder();
conn.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
conn.Username = builder.Configuration["UserID"];
conn.Password = builder.Configuration["Password"];
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CommandDbContext>(opt => opt.UseNpgsql(conn.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
