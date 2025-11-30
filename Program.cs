using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

// --------------------
// 1. יצירת Builder
// --------------------
var builder = WebApplication.CreateBuilder(args);

// --------------------
// 2. הוספת DbContext עם MySQL (מתוקן)
// --------------------
var connStr = builder.Configuration.GetConnectionString("ToDoDB");
if (string.IsNullOrWhiteSpace(connStr))
{
    // אם מחרוזת החיבור לא מוגדרת — נשתמש ב־InMemory כדי למנוע חריגה בזמן ריצה
    Console.WriteLine("Warning: Connection string 'ToDoDB' not found. Using InMemory database for development.");
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseInMemoryDatabase("ToDoDB")
    );
}
else
{
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
    );
}

// --------------------
// 3. הוספת CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------
// 4. הוספת Swagger
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --------------------
// 5. הפעלת Swagger
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --------------------
// 6. הפעלת CORS
// --------------------
app.UseCors();

// --------------------
// 7. Routes ל-Minimal API
// --------------------

// שליפת כל המשימות
app.MapGet("/tasks", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
});

// הוספת משימה חדשה
app.MapPost("/tasks", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item);
});

// עדכון משימה
app.MapPut("/tasks/{id}", async (ToDoDbContext db, int id, Item updatedItem) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null) return Results.NotFound();

    task.Name = updatedItem.Name;
    task.IsComplete = updatedItem.IsComplete;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// מחיקת משימה
app.MapDelete("/tasks/{id}", async (ToDoDbContext db, int id) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null) return Results.NotFound();

    db.Items.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

// --------------------
// 8. הגדרת DbContext ו-Entity
// --------------------
public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options) { }

    public DbSet<Item> Items { get; set; } = null!;
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}