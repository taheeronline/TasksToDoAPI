using Microsoft.EntityFrameworkCore;
using TasksToDo.DAL;
using TasksToDo.DAL.iServices;
using TasksToDo.DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]), 
                    ServiceLifetime.Singleton);

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<iToDoService, ToDoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
