using FractalCli.Fractals;
using SixLabors.ImageSharp.PixelFormats;

namespace FractalCli.Rendering;

public static class ColorPalette
{
    private static readonly Rgba32 Black = new(0, 0, 0);

    public static Rgba32 Map(EscapeResult escape, int maxIterations, PaletteName palette)
    {
        if (!escape.Escaped)
        {
            return Black;
        }

        var t = Math.Clamp(escape.Iterations / (double)maxIterations, 0.0, 1.0);
        return palette switch
        {
            PaletteName.Classic => Gradient(t, new Rgba32(28, 12, 71), new Rgba32(72, 61, 139), new Rgba32(255, 160, 64)),
            PaletteName.Fire => Gradient(t, new Rgba32(80, 0, 0), new Rgba32(220, 48, 16), new Rgba32(255, 235, 120)),
            PaletteName.Ice => Gradient(t, new Rgba32(5, 20, 75), new Rgba32(0, 190, 220), new Rgba32(245, 255, 255)),
            PaletteName.Gray => Gray(t),
            _ => Gradient(t, new Rgba32(28, 12, 71), new Rgba32(72, 61, 139), new Rgba32(255, 160, 64))
        };
    }

    private static Rgba32 Gray(double t)
    {
        var value = ToByte(255.0 * t);
        return new Rgba32(value, value, value);
    }

    private static Rgba32 Gradient(double t, Rgba32 start, Rgba32 middle, Rgba32 end)
    {
        if (t < 0.5)
        {
            return Lerp(start, middle, t * 2.0);
        }

        return Lerp(middle, end, (t - 0.5) * 2.0);
    }

    private static Rgba32 Lerp(Rgba32 a, Rgba32 b, double t)
    {
        return new Rgba32(
            ToByte(a.R + (b.R - a.R) * t),
            ToByte(a.G + (b.G - a.G) * t),
            ToByte(a.B + (b.B - a.B) * t));
    }

    private static byte ToByte(double value)
    {
        return (byte)Math.Clamp((int)Math.Round(value), 0, 255);
    }
}
