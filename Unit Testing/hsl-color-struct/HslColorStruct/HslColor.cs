namespace HslColorStruct;

public readonly struct HslColor : IEquatable<HslColor>
{
    public HslColor(int hue, int saturation, int lightness)
    {
        Validate(hue, nameof(hue), 0, 360);
        Validate(saturation, nameof(saturation), 0, 100);
        Validate(lightness, nameof(lightness), 0, 100);

        this.Hue = hue;
        this.Saturation = saturation;
        this.Lightness = lightness;
    }

    public int Hue { get; init; }

    public int Saturation { get; init; }

    public int Lightness { get; init; }

    public static HslColor Create(int hue, int saturation, int lightness)
    => new HslColor(hue, saturation, lightness);

    public static HslColor Parse(string hslString)
    {
        ArgumentNullException.ThrowIfNull(hslString);

        if (!TryParse(hslString, out var result))
        {
            throw new ArgumentException("Invalid HSL string format.", nameof(hslString));
        }

        return result;
    }

    public static bool TryParse(string hslString, out HslColor hslColor)
    {
        hslColor = default;

        if (string.IsNullOrWhiteSpace(hslString))
        {
            return false;
        }

        var parts = hslString.Split(',');

        if (parts.Length != 3)
        {
            return false;
        }

        if (parts.Any(p => p.Trim() != p))
        {
            return false;
        }

        if (!int.TryParse(parts[0], out var hue) ||
            !int.TryParse(parts[1], out var saturation) ||
            !int.TryParse(parts[2], out var lightness))
        {
            return false;
        }

        try
        {
            hslColor = new HslColor(hue, saturation, lightness);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Equals(HslColor other)
    => this.Hue == other.Hue &&
       this.Saturation == other.Saturation &&
       this.Lightness == other.Lightness;

    public override bool Equals(object? obj)
       => obj is HslColor other && this.Equals(other);

    public override int GetHashCode()
     => HashCode.Combine(this.Hue, this.Saturation, this.Lightness);

    public override string ToString()
    => $"{this.Hue},{this.Saturation},{this.Lightness}";

    public static bool operator ==(HslColor left, HslColor right)
        => left.Equals(right);

    public static bool operator !=(HslColor left, HslColor right)
        => !left.Equals(right);

    private static void Validate(int value, string name, int min, int max)
    {
        if (value < min || value > max)
        {
            throw new ArgumentException(
    $"Value must be between {min} and {max}.",
    name);
        }
    }
}
