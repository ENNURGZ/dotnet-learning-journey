using ValidationAttributes;

namespace ValidationServiceTask.Tests.AttributeTestClasses;

public class RequiredTestClass
{
    [Required]
    public string StringField = null!;

    [Required(ErrorMessage = "InnerClassProperty is required.")]
    public InnerClass InnerClassProperty { get; set; } = null!;
}

public class InnerClass
{
}
