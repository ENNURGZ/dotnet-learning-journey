using NUnit.Framework;
using System;

namespace HslColorStruct.Tests;


[TestFixture]
public class HslColorTests
{
    #region Constructor Tests

    [Test]
    public void Constructor_WithValidValues_CreatesColor()
    {
        var color = new HslColor(120, 50, 75);

        Assert.Multiple(() =>
        {
            Assert.That(color.Hue, Is.EqualTo(120));
            Assert.That(color.Saturation, Is.EqualTo(50));
            Assert.That(color.Lightness, Is.EqualTo(75));
        });
    }

    [Test]
    public void Constructor_WithBoundaryValues_CreatesColor()
    {
        var color = new HslColor(0, 100, 0);

        Assert.That(color.Hue, Is.EqualTo(0));
        Assert.That(color.Saturation, Is.EqualTo(100));
        Assert.That(color.Lightness, Is.EqualTo(0));
    }

    [TestCase(-1)]
    [TestCase(361)]
    public void Constructor_WithInvalidHue_Throws(int hue)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new HslColor(hue, 50, 50));
    }

    [TestCase(-1)]
    [TestCase(101)]
    public void Constructor_WithInvalidSaturation_Throws(int saturation)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new HslColor(120, saturation, 50));
    }

    [TestCase(-1)]
    [TestCase(101)]
    public void Constructor_WithInvalidLightness_Throws(int lightness)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new HslColor(120, 50, lightness));
    }

    #endregion

    #region Create Tests

    [Test]
    public void Create_WithValidValues_ReturnsColor()
    {
        var color = HslColor.Create(10, 20, 30);

        Assert.That(color.Hue, Is.EqualTo(10));
        Assert.That(color.Saturation, Is.EqualTo(20));
        Assert.That(color.Lightness, Is.EqualTo(30));
    }

    #endregion

    #region Parse Tests

    [Test]
    public void Parse_WithValidString_ReturnsColor()
    {
        var color = HslColor.Parse("120,50,75");

        Assert.Multiple(() =>
        {
            Assert.That(color.Hue, Is.EqualTo(120));
            Assert.That(color.Saturation, Is.EqualTo(50));
            Assert.That(color.Lightness, Is.EqualTo(75));
        });
    }

    [Test]
    public void Parse_WithNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => HslColor.Parse(null!));
    }

    [Test]
    public void Parse_WithInvalidFormat_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => HslColor.Parse("invalid"));
    }

    [Test]
    public void Parse_WithOutOfRangeValues_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => HslColor.Parse("400,50,50"));
    }

    #endregion

    #region TryParse Tests

    [Test]
    public void TryParse_WithValidString_ReturnsTrue()
    {
        var result = HslColor.TryParse("0,100,100", out var color);

        Assert.That(result, Is.True);
        Assert.That(color.Hue, Is.EqualTo(0));
        Assert.That(color.Saturation, Is.EqualTo(100));
        Assert.That(color.Lightness, Is.EqualTo(100));
    }

    [Test]
    public void TryParse_WithInvalidString_ReturnsFalse()
    {
        var result = HslColor.TryParse("invalid", out var color);

        Assert.That(result, Is.False);
        Assert.That(color, Is.EqualTo(default(HslColor)));
    }

    [Test]
    public void TryParse_WithNull_ReturnsFalse()
    {
        var result = HslColor.TryParse(null!, out var color);

        Assert.That(result, Is.False);
        Assert.That(color, Is.EqualTo(default(HslColor)));
    }

    [Test]
    public void TryParse_WithWhitespace_ReturnsFalse()
    {
        var result = HslColor.TryParse("   ", out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void TryParse_WithOutOfRangeValues_ReturnsFalse()
    {
        var result = HslColor.TryParse("400,50,50", out _);

        Assert.That(result, Is.False);
    }

    #endregion

    #region Equality Tests

    [Test]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var c1 = new HslColor(10, 20, 30);
        var c2 = new HslColor(10, 20, 30);

        Assert.That(c1.Equals(c2), Is.True);
        Assert.That(c1 == c2, Is.True);
        Assert.That(c1 != c2, Is.False);
    }

    [Test]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        var c1 = new HslColor(10, 20, 30);
        var c2 = new HslColor(11, 20, 30);

        Assert.That(c1.Equals(c2), Is.False);
        Assert.That(c1 == c2, Is.False);
        Assert.That(c1 != c2, Is.True);
    }

    [Test]
    public void Equals_WithObject_ReturnsFalse()
    {
        var color = new HslColor(10, 20, 30);

        Assert.That(color.Equals(new object()), Is.False);
    }

    #endregion

    #region HashCode Tests

    [Test]
    public void GetHashCode_ForEqualObjects_ReturnsSameHash()
    {
        var c1 = new HslColor(100, 50, 25);
        var c2 = new HslColor(100, 50, 25);

        Assert.That(c1.GetHashCode(), Is.EqualTo(c2.GetHashCode()));
    }

    #endregion

    #region ToString Tests

    [Test]
    public void ToString_ReturnsCorrectFormat()
    {
        var color = new HslColor(1, 2, 3);

        Assert.That(color.ToString(), Is.EqualTo("1,2,3"));
    }

    [Test]
    public void ToString_ThenParse_RoundTrip_Succeeds()
    {
        var original = new HslColor(120, 60, 70);

        var parsed = HslColor.Parse(original.ToString());

        Assert.That(parsed, Is.EqualTo(original));
    }

    #endregion
}
