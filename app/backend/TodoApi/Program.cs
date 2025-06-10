using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
// Add DB Context
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

// Routes
app.MapGet("/todos", async (TodoDbContext db) =>
    await db.ToDos.ToListAsync());

app.MapGet("/todos/{id}", async (int id, TodoDbContext db) =>
    await db.ToDos.FindAsync(id) is ToDo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/todos", async (ToDo todo, TodoDbContext db) =>
{
    db.ToDos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (int id, ToDo updatedTodo, TodoDbContext db) =>
{
    var todo = await db.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    todo.Title = updatedTodo.Title;
    todo.IsCompleted = updatedTodo.IsCompleted;
    await db.SaveChangesAsync();
    return Results.Ok(todo);
});

app.MapDelete("/todos/{id}", async (int id, TodoDbContext db) =>
{
    var todo = await db.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.ToDos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

