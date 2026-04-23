namespace ToDoAppApi
{
    /// <summary>
    /// Data transfer object for patching a to-do item.
    /// </summary>
    public class TodoPatchDto
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is complete.
        /// </summary>
        public bool? IsComplete { get; set; }
    }
}
