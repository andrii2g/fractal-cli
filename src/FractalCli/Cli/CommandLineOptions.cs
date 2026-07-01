using FractalCli.Rendering;

namespace FractalCli.Cli;

public sealed class CommandLineOptions
{
    public string? OutputPath { get; set; }
    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 800;
    public double? CenterX { get; set; }
    public double CenterY { get; set; } = 0.0;
    public double? Scale { get; set; }
    public int MaxIterations { get; set; } = 500;
    public PaletteName Palette { get; set; } = PaletteName.Classic;
    public double JuliaCx { get; set; } = -0.8;
    public double JuliaCy { get; set; } = 0.156;
    public bool JuliaCxSpecified { get; set; }
    public bool JuliaCySpecified { get; set; }
    public bool Quiet { get; set; }
}
