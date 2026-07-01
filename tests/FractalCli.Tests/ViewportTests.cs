using FractalCli.Rendering;

namespace FractalCli.Tests;

public class ViewportTests
{
    [Fact]
    public void DefaultMandelbrotViewportUsesExpectedBounds()
    {
        var viewport = new Viewport(1200, 800, -0.5, 0, 3.0);

        Assert.Equal(-2.0, viewport.XMin, precision: 10);
        Assert.Equal(1.0, viewport.XMax, precision: 10);
        Assert.Equal(-1.0, viewport.YMin, precision: 10);
        Assert.Equal(1.0, viewport.YMax, precision: 10);
    }

    [Fact]
    public void SquareViewportUsesEqualSpans()
    {
        var viewport = new Viewport(1000, 1000, 0, 0, 4.0);

        Assert.Equal(-2.0, viewport.XMin, precision: 10);
        Assert.Equal(2.0, viewport.XMax, precision: 10);
        Assert.Equal(-2.0, viewport.YMin, precision: 10);
        Assert.Equal(2.0, viewport.YMax, precision: 10);
    }

    [Fact]
    public void OnePixelDimensionsDoNotDivideByZero()
    {
        var viewport = new Viewport(1, 1, 1.5, -2.5, 4.0);

        Assert.Equal(1.5, viewport.PixelToX(0));
        Assert.Equal(-2.5, viewport.PixelToY(0));
    }
}
