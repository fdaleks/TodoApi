using Microsoft.EntityFrameworkCore;
using TodoApi;

// create web application builder
var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

// build the application
var app = builder.Build();

// configure the http requests pipeline
app.MapGet("/todoitems", async (TodoDb db) => 
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => 
    await db.Todos.FindAsync(id));

app.MapPost("/todoitems", async (TodoItem item, TodoDb db) => 
{
    db.Todos.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.MapPut("/todoitems/{id}", async (int id, TodoItem inputItem, TodoDb db) => 
{
    var item = await db.Todos.FindAsync(id);
    if (item == null) return Results.NotFound();
    item.Name = inputItem.Name;
    item.IsCompleted = inputItem.IsCompleted;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) => 
{
    if (await db.Todos.FindAsync(id) is TodoItem item)
    {
        db.Todos.Remove(item);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// run the application
app.Run();
