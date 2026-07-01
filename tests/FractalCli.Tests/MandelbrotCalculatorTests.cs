using FractalCli.Fractals;

namespace FractalCli.Tests;

public class MandelbrotCalculatorTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 0)]
    public void KnownInteriorPointsDoNotEscape(double cx, double cy)
    {
        var result = MandelbrotCalculator.Calculate(cx, cy, 500);

        Assert.False(result.Escaped);
        Assert.Equal(500, result.Iterations);
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(1, 0)]
    public void KnownExteriorPointsEscape(double cx, double cy)
    {
        var result = MandelbrotCalculator.Calculate(cx, cy, 500);

        Assert.True(result.Escaped);
        Assert.InRange(result.Iterations, 0, 10);
    }
}
