namespace FractalCli.Fractals;

public static class JuliaCalculator
{
    public static EscapeResult Calculate(double pointX, double pointY, double juliaCx, double juliaCy, int maxIterations)
    {
        var zx = pointX;
        var zy = pointY;

        for (var i = 0; i < maxIterations; i++)
        {
            var zx2 = zx * zx;
            var zy2 = zy * zy;

            if (zx2 + zy2 > 4.0)
            {
                return new EscapeResult(i, escaped: true, zx, zy);
            }

            zy = 2.0 * zx * zy + juliaCy;
            zx = zx2 - zy2 + juliaCx;
        }

        return new EscapeResult(maxIterations, escaped: false, zx, zy);
    }
}
