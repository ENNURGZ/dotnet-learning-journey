namespace ToDoAppApi
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Database context for the Todo application.
    /// </summary>
    public class TodoDb : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoDb"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public TodoDb(DbContextOptions<TodoDb> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the todos.
        /// </summary>
        public DbSet<Todo> Todos => this.Set<Todo>();
    }
}
