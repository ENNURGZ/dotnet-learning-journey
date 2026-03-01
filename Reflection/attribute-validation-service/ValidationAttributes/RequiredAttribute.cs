namespace ValidationAttributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute()
            : base("The field is required.")
        {
        }

        public RequiredAttribute(string message)
            : base(message)
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is string str)
            {
                return !string.IsNullOrEmpty(str);
            }

            return true;
        }
    }
}
