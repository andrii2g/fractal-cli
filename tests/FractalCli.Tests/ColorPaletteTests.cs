using FractalCli.Fractals;
using FractalCli.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace FractalCli.Tests;

public class ColorPaletteTests
{
    [Fact]
    public void InteriorPointMapsToBlack()
    {
        var color = ColorPalette.Map(new EscapeResult(500, escaped: false, 0, 0), 500, PaletteName.Classic);

        Assert.Equal(new Rgba32(0, 0, 0), color);
    }

    [Theory]
    [InlineData(PaletteName.Classic)]
    [InlineData(PaletteName.Fire)]
    [InlineData(PaletteName.Ice)]
    [InlineData(PaletteName.Gray)]
    public void EscapedPointMapsToVisibleColor(PaletteName palette)
    {
        var color = ColorPalette.Map(new EscapeResult(250, escaped: true, 3, 0), 500, palette);

        Assert.NotEqual(new Rgba32(0, 0, 0), color);
    }
}
