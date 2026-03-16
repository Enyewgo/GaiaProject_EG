//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.EntityFrameworkCore;
using GaiaProject.Models; // מוודא שהשרת מכיר את התיקייה שיצרנו

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGaiaProject",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // הכתובת של ה-React שלך
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- 1. הגדרת שירותים (Services) ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// חיבור ה-DbContext למסד הנתונים
// סעיף: הגדרת תשתית ה-Data Access
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseCors("AllowGaiaProject");

// --- 2. הגדרת ה-Middleware (סדר הפעולות של השרת) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// חשוב עבור ה-React: מאפשר הגשת קבצים סטטיים אם נרצה
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();