namespace FractalCli.Cli;

public sealed class ValidationResult
{
    private readonly List<string> _errors = [];

    public IReadOnlyList<string> Errors => _errors;
    public bool IsValid => _errors.Count == 0;

    public void Add(string message)
    {
        _errors.Add(message);
    }

    public string ToDisplayText()
    {
        if (IsValid)
        {
            return string.Empty;
        }

        return string.Join(Environment.NewLine, _errors.Select(error => $"Error: {error}"));
    }
}
