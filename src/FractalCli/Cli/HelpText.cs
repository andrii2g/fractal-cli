namespace FractalCli.Cli;

public static class HelpText
{
    public const string Short = """
Usage:
  fractal render mandelbrot --output <file.png> [options]
  fractal render julia --output <file.png> [options]

Use --help to show all options.
""";

    public const string Full = """
Fractal CLI

Usage:
  fractal render mandelbrot --output <file.png> [options]
  fractal render julia --output <file.png> [options]

Options:
  --output <path>             Output PNG file path. Required.
  --width <int>               Image width. Default: 1200. Range: 1..10000.
  --height <int>              Image height. Default: 800. Range: 1..10000.
  --center-x <double>         View center X coordinate.
  --center-y <double>         View center Y coordinate. Default: 0.
  --scale <double>            Horizontal viewport scale. Smaller means more zoom. Default: 3.0.
  --max-iterations <int>      Maximum iteration count. Default: 500. Range: 1..20000.
  --palette <name>            classic, fire, ice, gray. Default: classic.
  --julia-cx <double>         Julia constant real part. Julia only. Default: -0.8.
  --julia-cy <double>         Julia constant imaginary part. Julia only. Default: 0.156.
  --quiet                     Suppress success output.
  --help                      Show help.

Examples:
  fractal render mandelbrot --output mandelbrot.png
  fractal render mandelbrot --output zoom.png --center-x -0.743643887037151 --center-y 0.13182590420533 --scale 0.0008 --max-iterations 1200 --palette fire
  fractal render julia --output julia.png --julia-cx -0.8 --julia-cy 0.156 --palette ice
""";
}
