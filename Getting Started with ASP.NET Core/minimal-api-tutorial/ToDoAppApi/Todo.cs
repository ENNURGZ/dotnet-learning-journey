namespace ToDoAppApi
{
    /// <summary>
    /// Represents a to-do item.
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is complete.
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
