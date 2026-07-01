using System.Globalization;
using FractalCli.Fractals;
using FractalCli.Rendering;

namespace FractalCli.Cli;

public static class CommandLineParser
{
    private const int MaxWidth = 10000;
    private const int MaxHeight = 10000;
    private const int MaxIterations = 20000;

    public static ParseResult Parse(string[] args)
    {
        var validation = new ValidationResult();

        if (args.Length == 0)
        {
            validation.Add("Missing command. Use: fractal render <mandelbrot|julia> --output <file.png>");
            return ParseResult.Invalid(validation);
        }

        if (IsHelp(args[0]))
        {
            return ParseResult.Help();
        }

        if (!IsRenderCommand(args[0]))
        {
            validation.Add($"Unknown command '{args[0]}'.");
            return ParseResult.Invalid(validation);
        }

        if (args.Length == 2 && IsHelp(args[1]))
        {
            return ParseResult.Help();
        }

        if (args.Length < 2)
        {
            validation.Add("Missing fractal type. Use 'mandelbrot' or 'julia'.");
            return ParseResult.Invalid(validation);
        }

        if (!TryParseFractalType(args[1], out var type))
        {
            validation.Add($"Unknown fractal type '{args[1]}'. Use 'mandelbrot' or 'julia'.");
            return ParseResult.Invalid(validation);
        }

        if (args.Length == 3 && IsHelp(args[2]))
        {
            return ParseResult.Help();
        }

        var options = new CommandLineOptions();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 2; i < args.Length; i++)
        {
            var name = args[i];
            if (!name.StartsWith("--", StringComparison.Ordinal))
            {
                validation.Add($"Unexpected argument '{name}'.");
                continue;
            }

            if (!seen.Add(name))
            {
                validation.Add($"Duplicate option '{name}'.");
                if (RequiresValue(name) && i + 1 < args.Length)
                {
                    i++;
                }

                continue;
            }

            switch (name)
            {
                case "--help":
                    return ParseResult.Help();
                case "--quiet":
                    options.Quiet = true;
                    break;
                case "--output":
                    options.OutputPath = ReadValue(args, ref i, name, validation);
                    break;
                case "--width":
                    ReadInt(args, ref i, name, validation, value => options.Width = value);
                    break;
                case "--height":
                    ReadInt(args, ref i, name, validation, value => options.Height = value);
                    break;
                case "--center-x":
                    ReadDouble(args, ref i, name, validation, value => options.CenterX = value);
                    break;
                case "--center-y":
                    ReadDouble(args, ref i, name, validation, value => options.CenterY = value);
                    break;
                case "--scale":
                    ReadDouble(args, ref i, name, validation, value => options.Scale = value);
                    break;
                case "--max-iterations":
                    ReadInt(args, ref i, name, validation, value => options.MaxIterations = value);
                    break;
                case "--palette":
                    ReadPalette(args, ref i, validation, value => options.Palette = value);
                    break;
                case "--julia-cx":
                    options.JuliaCxSpecified = true;
                    ReadDouble(args, ref i, name, validation, value => options.JuliaCx = value);
                    break;
                case "--julia-cy":
                    options.JuliaCySpecified = true;
                    ReadDouble(args, ref i, name, validation, value => options.JuliaCy = value);
                    break;
                default:
                    validation.Add($"Unknown option '{name}'.");
                    break;
            }
        }

        Validate(type, options, validation);

        if (!validation.IsValid)
        {
            return ParseResult.Invalid(validation, options.Quiet);
        }

        var centerX = options.CenterX ?? (type == FractalType.Mandelbrot ? -0.5 : 0.0);
        var scale = options.Scale ?? 3.0;
        var request = new RenderRequest(
            type,
            options.OutputPath!,
            options.Width,
            options.Height,
            centerX,
            options.CenterY,
            scale,
            options.MaxIterations,
            options.Palette,
            options.JuliaCx,
            options.JuliaCy);

        return ParseResult.Valid(request, options.Quiet);
    }

    private static bool IsRenderCommand(string value)
    {
        return string.Equals(value, "render", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsHelp(string value)
    {
        return string.Equals(value, "--help", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "-h", StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryParseFractalType(string value, out FractalType type)
    {
        if (string.Equals(value, "mandelbrot", StringComparison.OrdinalIgnoreCase))
        {
            type = FractalType.Mandelbrot;
            return true;
        }

        if (string.Equals(value, "julia", StringComparison.OrdinalIgnoreCase))
        {
            type = FractalType.Julia;
            return true;
        }

        type = default;
        return false;
    }

    private static bool RequiresValue(string option)
    {
        return option is not "--help" and not "--quiet";
    }

    private static string? ReadValue(string[] args, ref int index, string name, ValidationResult validation)
    {
        if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
        {
            validation.Add($"Missing value for option '{name}'.");
            return null;
        }

        index++;
        return args[index];
    }

    private static void ReadInt(string[] args, ref int index, string name, ValidationResult validation, Action<int> assign)
    {
        var value = ReadValue(args, ref index, name, validation);
        if (value is null)
        {
            return;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            validation.Add($"Option '{name}' must be an integer.");
            return;
        }

        assign(parsed);
    }

    private static void ReadDouble(string[] args, ref int index, string name, ValidationResult validation, Action<double> assign)
    {
        var value = ReadValue(args, ref index, name, validation);
        if (value is null)
        {
            return;
        }

        if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) || !double.IsFinite(parsed))
        {
            validation.Add($"Option '{name}' must be a finite number.");
            return;
        }

        assign(parsed);
    }

    private static void ReadPalette(string[] args, ref int index, ValidationResult validation, Action<PaletteName> assign)
    {
        var value = ReadValue(args, ref index, "--palette", validation);
        if (value is null)
        {
            return;
        }

        if (!Enum.TryParse<PaletteName>(value, ignoreCase: true, out var palette))
        {
            validation.Add($"Unknown palette '{value}'. Use classic, fire, ice, or gray.");
            return;
        }

        assign(palette);
    }

    private static void Validate(FractalType type, CommandLineOptions options, ValidationResult validation)
    {
        if (string.IsNullOrWhiteSpace(options.OutputPath))
        {
            validation.Add("Output path is required.");
        }

        if (options.Width is < 1 or > MaxWidth)
        {
            validation.Add($"Width must be between 1 and {MaxWidth}.");
        }

        if (options.Height is < 1 or > MaxHeight)
        {
            validation.Add($"Height must be between 1 and {MaxHeight}.");
        }

        if (options.Scale is <= 0)
        {
            validation.Add("Scale must be greater than 0.");
        }

        if (options.MaxIterations is < 1 or > MaxIterations)
        {
            validation.Add($"Max iterations must be between 1 and {MaxIterations}.");
        }

        if (type == FractalType.Mandelbrot && (options.JuliaCxSpecified || options.JuliaCySpecified))
        {
            validation.Add("Options '--julia-cx' and '--julia-cy' are only valid for Julia renders.");
        }
    }
}
