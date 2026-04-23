namespace ToDoAppApi
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "TodoAPI";
                config.Title = "TodoAPI v1";
                config.Version = "v1";
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi(config =>
                {
                    config.DocumentTitle = "TodoAPI";
                    config.Path = "/swagger";
                    config.DocumentPath = "/swagger/{documentName}/swagger.json";
                    config.DocExpansion = "list";
                });
            }

            var todoItems = app.MapGroup("/todoitems");

            todoItems.MapGet("/", GetAllTodos);
            todoItems.MapGet("/complete", GetCompleteTodos);
            todoItems.MapGet("/{id}", GetTodo);
            todoItems.MapPost("/", CreateTodo);
            todoItems.MapPut("/{id}", UpdateTodo);
            todoItems.MapPatch("/{id}", PatchTodo);
            todoItems.MapDelete("/{id}", DeleteTodo);

            app.Run();
        }

        private static async Task<IResult> GetAllTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.ToArrayAsync());
        }

        private static async Task<IResult> GetCompleteTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
        }

        private static async Task<IResult> GetTodo(int id, TodoDb db)
        {
            return await db.Todos.FindAsync(id) is Todo todo
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound();
        }

        private static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        private static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        private static async Task<IResult> PatchTodo(int id, TodoPatchDto inputTodo, TodoDb db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            if (inputTodo.Name is not null)
            {
                todo.Name = inputTodo.Name;
            }

            if (inputTodo.IsComplete is not null)
            {
                todo.IsComplete = inputTodo.IsComplete.Value;
            }

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        private static async Task<IResult> DeleteTodo(int id, TodoDb db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
