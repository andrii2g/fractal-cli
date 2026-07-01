using FractalCli.Fractals;

namespace FractalCli.Rendering;

public sealed record RenderRequest(
    FractalType Type,
    string OutputPath,
    int Width,
    int Height,
    double CenterX,
    double CenterY,
    double Scale,
    int MaxIterations,
    PaletteName Palette,
    double JuliaCx,
    double JuliaCy);
