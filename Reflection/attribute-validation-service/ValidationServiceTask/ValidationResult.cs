namespace ValidationServiceTask
{
    public class ValidationResult
    {
        public ValidationResult(Type attributeType, string message)
        {
            this.AttributeType = attributeType;
            this.ValidationMessage = message;
        }

        public Type AttributeType { get; init; }

        public string ValidationMessage { get; init; }
    }
}
