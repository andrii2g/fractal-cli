using FractalCli.Cli;
using FractalCli.Fractals;
using FractalCli.Rendering;

namespace FractalCli.Tests;

public class CommandLineParserTests
{
    [Fact]
    public void MissingArgsReturnValidationError()
    {
        var result = CommandLineParser.Parse([]);

        Assert.False(result.Validation.IsValid);
    }

    [Fact]
    public void UnknownFractalTypeFails()
    {
        var result = CommandLineParser.Parse(["render", "burning-ship", "--output", "out.png"]);

        Assert.False(result.Validation.IsValid);
    }

    [Fact]
    public void MissingOutputFails()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot"]);

        Assert.False(result.Validation.IsValid);
        Assert.Contains(result.Validation.Errors, error => error.Contains("Output path", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidMandelbrotCommandSucceeds()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png"]);

        Assert.True(result.Validation.IsValid);
        Assert.NotNull(result.Request);
        Assert.Equal(FractalType.Mandelbrot, result.Request.Type);
        Assert.Equal(-0.5, result.Request.CenterX);
        Assert.Equal(3.0, result.Request.Scale);
    }

    [Fact]
    public void ValidJuliaCommandSucceeds()
    {
        var result = CommandLineParser.Parse(["render", "julia", "--output", "out.png", "--julia-cx", "-0.4", "--julia-cy", "0.6"]);

        Assert.True(result.Validation.IsValid);
        Assert.NotNull(result.Request);
        Assert.Equal(FractalType.Julia, result.Request.Type);
        Assert.Equal(-0.4, result.Request.JuliaCx);
        Assert.Equal(0.6, result.Request.JuliaCy);
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("10000", true)]
    [InlineData("0", false)]
    [InlineData("10001", false)]
    public void WidthBoundaryIsEnforced(string width, bool isValid)
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--width", width]);

        Assert.Equal(isValid, result.Validation.IsValid);
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("10000", true)]
    [InlineData("0", false)]
    [InlineData("10001", false)]
    public void HeightBoundaryIsEnforced(string height, bool isValid)
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--height", height]);

        Assert.Equal(isValid, result.Validation.IsValid);
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("20000", true)]
    [InlineData("0", false)]
    [InlineData("20001", false)]
    public void MaxIterationsBoundaryIsEnforced(string maxIterations, bool isValid)
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--max-iterations", maxIterations]);

        Assert.Equal(isValid, result.Validation.IsValid);
    }

    [Fact]
    public void InvalidScaleFails()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--scale", "0"]);

        Assert.False(result.Validation.IsValid);
    }

    [Fact]
    public void UnknownPaletteFails()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--palette", "neon"]);

        Assert.False(result.Validation.IsValid);
    }

    [Theory]
    [InlineData("--julia-cx")]
    [InlineData("--julia-cy")]
    public void JuliaOptionsFailForMandelbrot(string option)
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", option, "0.1"]);

        Assert.False(result.Validation.IsValid);
    }

    [Fact]
    public void HelpRequestShowsHelp()
    {
        var result = CommandLineParser.Parse(["render", "julia", "--help"]);

        Assert.True(result.ShowHelp);
    }

    [Fact]
    public void DuplicateOptionFails()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "a.png", "--output", "b.png"]);

        Assert.False(result.Validation.IsValid);
    }

    [Fact]
    public void PaletteParsesCaseInsensitively()
    {
        var result = CommandLineParser.Parse(["render", "mandelbrot", "--output", "out.png", "--palette", "ICE"]);

        Assert.True(result.Validation.IsValid);
        Assert.Equal(PaletteName.Ice, result.Request!.Palette);
    }
}
