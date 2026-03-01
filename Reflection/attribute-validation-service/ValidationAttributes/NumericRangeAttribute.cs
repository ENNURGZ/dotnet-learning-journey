using System.Globalization;

namespace ValidationAttributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class NumericRangeAttribute : ValidationAttribute
    {
        public NumericRangeAttribute(object minimum, object maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public NumericRangeAttribute(object minimum, object maximum, Type type)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.NumericType = type;
        }

        public object Minimum { get; }

        public object Maximum { get; }

        public Type? NumericType { get; }

        public override bool IsValid(object? value)
        {
            double min = Convert.ToDouble(this.Minimum, CultureInfo.InvariantCulture);
            double max = Convert.ToDouble(this.Maximum, CultureInfo.InvariantCulture);

            if (min >= max)
            {
                throw new ArgumentException("Minimum cannot be greater than maximum.");
            }

            if (value == null)
            {
                return true;
            }

            if (!IsNumeric(value))
            {
                return false;
            }

            double number = Convert.ToDouble(value, CultureInfo.InvariantCulture);

            return number >= min && number <= max;
        }

        private static bool IsNumeric(object value)
        {
            return value is byte or sbyte
                or short or ushort
                or int or uint
                or long or ulong
                or float or double
                or decimal;
        }
    }
}
