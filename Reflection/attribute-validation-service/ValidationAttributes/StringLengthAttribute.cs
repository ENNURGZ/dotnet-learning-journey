namespace ValidationAttributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class StringLengthAttribute : ValidationAttribute
    {
        public StringLengthAttribute(int min, int max)
            : base($"The string length must be greater than or equal {min} and less than or equal {max}.")
        {
            this.MinimumLength = min;
            this.MaximumLength = max;
        }

        public int MinimumLength { get; }

        public int MaximumLength { get; }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string str)
            {
                return false;
            }

            return str.Length >= this.MinimumLength &&
                   str.Length <= this.MaximumLength;
        }
    }
}
