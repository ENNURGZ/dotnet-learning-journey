using ValidationAttributes;

namespace ValidationServiceTask.Tests.AttributeTestClasses;

public class NumericRangeTestClass
{
    [NumericRange(1, 10)]
    public byte ByteField;

    [NumericRange(-10L, 10L, ErrorMessage = "The value of LongField must be between -10 and 10.")]
    public long LongField;

    [NumericRange(-8, 10, ErrorMessage = "The value of SbyteField must be between -8 and 10.")]
    public sbyte SbyteField;

    [NumericRange(-100, 10, ErrorMessage = "The value of ShortField must be between -100 and 10.")]
    public short ShortField;

    [NumericRange(5, 10, ErrorMessage = "The value of UshortProperty must be between 5 and 10.")]
    public ushort UshortProperty { get; set; }

    [NumericRange(-123, 123, ErrorMessage = "The value of IntegerProperty must be between -123 and 123.")]
    public int IntegerProperty { get; set; }

    [NumericRange(1u, 100u, ErrorMessage = "The value of UintProperty must be between 1 and 100.")]
    public uint UintProperty { get; set; }

    [NumericRange(10L, 1000L, ErrorMessage = "The value of UlongProperty must be between 10 and 1000.")]
    public ulong UlongProperty { get; set; }

    [NumericRange(1.5f, 10.5f, ErrorMessage = "The value of FloatProperty must be between 1.5 and 10.5.")]
    public float FloatProperty { get; set; }

    [NumericRange(
        100.5,
        105.5,
        ErrorMessage = "The value of DoubleProperty must be between 100.5 and 105.5.")]
    public double DoubleProperty { get; set; }

    [NumericRange(10, 1000, typeof(decimal), ErrorMessage = "The value of DecimalProperty must be between 10 and 1000.")]
    public decimal DecimalProperty { get; set; }
}
