namespace Reflection.Tests.TypesForTesting;

public class ClassForTesting
{
    public const int StaticIntField = 10;

    private int intField;

    public ClassForTesting()
        : this(0)
    {
    }

    public ClassForTesting(int intValue) => this.intField = intValue;

    public int IntValue
    {
        get => this.intField;
        set => this.intField = value < 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
    }
}
