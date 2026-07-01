using FractalCli.Rendering;

namespace FractalCli.Cli;

public sealed record ParseResult(RenderRequest? Request, ValidationResult Validation, bool ShowHelp, bool Quiet)
{
    public static ParseResult Help()
    {
        return new ParseResult(null, new ValidationResult(), ShowHelp: true, Quiet: false);
    }

    public static ParseResult Invalid(ValidationResult validation, bool quiet = false)
    {
        return new ParseResult(null, validation, ShowHelp: false, quiet);
    }

    public static ParseResult Valid(RenderRequest request, bool quiet)
    {
        return new ParseResult(request, new ValidationResult(), ShowHelp: false, quiet);
    }
}
