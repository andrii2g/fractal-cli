using FractalCli.Cli;
using FractalCli.Rendering;

namespace FractalCli;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var parseResult = CommandLineParser.Parse(args);

            if (parseResult.ShowHelp)
            {
                Console.WriteLine(HelpText.Full);
                return 0;
            }

            if (!parseResult.Validation.IsValid || parseResult.Request is null)
            {
                Console.Error.WriteLine(parseResult.Validation.ToDisplayText());
                Console.Error.WriteLine();
                Console.Error.WriteLine(HelpText.Short);
                return 1;
            }

            var renderer = new FractalRenderer();
            renderer.Render(parseResult.Request);

            if (!parseResult.Quiet)
            {
                Console.WriteLine($"Wrote: {parseResult.Request.OutputPath}");
            }

            return 0;
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"File error: {ex.Message}");
            return 3;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.Error.WriteLine($"File error: {ex.Message}");
            return 3;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Rendering error: {ex.Message}");
            return 2;
        }
    }
}
