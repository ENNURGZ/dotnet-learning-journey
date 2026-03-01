using ValidationAttributes;

namespace ValidationServiceTask.Tests.AttributeTestClasses;

public class StringLengthTestClass
{
    [Required(ErrorMessage = "String field is required.")]
    [StringLength(10, 25, ErrorMessage = "String field length must be between 10 and 25.")]
    public string NotNullableStringProperty { get; set; } = null!;

    [StringLength(10, 25)]
    public string? NullableStringProperty { get; set; }
}
