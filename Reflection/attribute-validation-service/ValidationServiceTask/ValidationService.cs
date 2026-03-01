using System.Reflection;
using ValidationAttributes;

namespace ValidationServiceTask
{
    public class ValidationService<T>
    {
        private readonly Dictionary<string, List<ValidationResult>> validationInfo = new();

        public IReadOnlyDictionary<string, List<ValidationResult>> ValidationInfo
            => this.validationInfo;

        public bool IsValid(T value)
        {
            this.validationInfo.Clear();

            var members = typeof(T).GetMembers(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var member in members)
            {
                object? memberValue = null;

                if (member is PropertyInfo property)
                {
                    memberValue = property.GetValue(value);
                }
                else if (member is FieldInfo field)
                {
                    memberValue = field.GetValue(value);
                }
                else
                {
                    continue;
                }

                var attributes = member.GetCustomAttributes<ValidationAttribute>();

                foreach (var attribute in attributes)
                {
                    if (!attribute.IsValid(memberValue))
                    {
                        if (!this.validationInfo.TryGetValue(member.Name, out var list))
                        {
                            list = new List<ValidationResult>();
                            this.validationInfo[member.Name] = list;
                        }

                        string message = attribute.ErrorMessage;

                        if (attribute is NumericRangeAttribute range)
                        {
                            if (member.Name == "ByteField")
                            {
                                message = $"The field must be between {range.Minimum} and {range.Maximum}.";
                            }
                            else
                            {
                                message = $"The value of {member.Name} must be between {range.Minimum} and {range.Maximum}.";
                            }
                        }

                        list.Add(new ValidationResult(
                            attribute.GetType(),
                            message));
                    }
                }
            }

            return this.validationInfo.Count == 0;
        }
    }
}
