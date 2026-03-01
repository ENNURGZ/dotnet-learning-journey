namespace ValidationAttributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true,
        Inherited = true)]
    public abstract class ValidationAttribute : Attribute
    {
        private const string DefaultMessage = "The field is invalid.";

        public string ErrorMessage { get; set; }

        protected ValidationAttribute()
        {
            this.ErrorMessage = DefaultMessage;
        }

        protected ValidationAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage ?? DefaultMessage;
        }

        public abstract bool IsValid(object? value);
    }
}
