namespace FractalCli.Rendering;

public sealed class Viewport
{
    public Viewport(int width, int height, double centerX, double centerY, double scale)
    {
        Width = width;
        Height = height;
        CenterX = centerX;
        CenterY = centerY;
        Scale = scale;

        var xSpan = scale;
        var ySpan = scale * height / width;

        XMin = centerX - xSpan / 2.0;
        XMax = centerX + xSpan / 2.0;
        YMin = centerY - ySpan / 2.0;
        YMax = centerY + ySpan / 2.0;
    }

    public int Width { get; }
    public int Height { get; }
    public double CenterX { get; }
    public double CenterY { get; }
    public double Scale { get; }
    public double XMin { get; }
    public double XMax { get; }
    public double YMin { get; }
    public double YMax { get; }

    public double PixelToX(int x)
    {
        if (Width == 1)
        {
            return CenterX;
        }

        return XMin + x * (XMax - XMin) / (Width - 1);
    }

    public double PixelToY(int y)
    {
        if (Height == 1)
        {
            return CenterY;
        }

        return YMax - y * (YMax - YMin) / (Height - 1);
    }
}
