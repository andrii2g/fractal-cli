using FractalCli.Fractals;

namespace FractalCli.Tests;

public class JuliaCalculatorTests
{
    [Fact]
    public void SelectedConstantRunsDeterministically()
    {
        var first = JuliaCalculator.Calculate(0, 0, -0.8, 0.156, 500);
        var second = JuliaCalculator.Calculate(0, 0, -0.8, 0.156, 500);

        Assert.Equal(first, second);
        Assert.InRange(first.Iterations, 1, 500);
    }

    [Fact]
    public void FarPointEscapesQuickly()
    {
        var result = JuliaCalculator.Calculate(2, 2, -0.8, 0.156, 500);

        Assert.True(result.Escaped);
        Assert.InRange(result.Iterations, 0, 2);
    }
}
