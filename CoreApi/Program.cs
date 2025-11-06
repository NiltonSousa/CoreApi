using CoreApi.domain.entity;
using CoreApi.domain.repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoRepository>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/todoitems", async (TodoRepository db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoRepository db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoRepository db) =>
    await db.Todos.FindAsync(id)
        is TodoEntity todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (TodoEntity todo, TodoRepository db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, TodoEntity inputTodo, TodoRepository db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoRepository db) =>
{
    if (await db.Todos.FindAsync(id) is TodoEntity todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();