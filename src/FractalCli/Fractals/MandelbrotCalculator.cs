namespace FractalCli.Fractals;

public static class MandelbrotCalculator
{
    public static EscapeResult Calculate(double pointX, double pointY, int maxIterations)
    {
        var zx = 0.0;
        var zy = 0.0;

        for (var i = 0; i < maxIterations; i++)
        {
            var zx2 = zx * zx;
            var zy2 = zy * zy;

            if (zx2 + zy2 > 4.0)
            {
                return new EscapeResult(i, escaped: true, zx, zy);
            }

            zy = 2.0 * zx * zy + pointY;
            zx = zx2 - zy2 + pointX;
        }

        return new EscapeResult(maxIterations, escaped: false, zx, zy);
    }
}
