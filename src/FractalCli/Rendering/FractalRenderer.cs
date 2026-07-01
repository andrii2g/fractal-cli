using FractalCli.Fractals;
using SixLabors.ImageSharp.PixelFormats;

namespace FractalCli.Rendering;

public sealed class FractalRenderer
{
    public void Render(RenderRequest request)
    {
        var pixels = RenderPixels(request);
        PngImageWriter.Write(request.OutputPath, pixels, request.Width, request.Height);
    }

    public Rgba32[] RenderPixels(RenderRequest request)
    {
        var viewport = new Viewport(request.Width, request.Height, request.CenterX, request.CenterY, request.Scale);
        var pixels = new Rgba32[request.Width * request.Height];

        Parallel.For(0, request.Height, y =>
        {
            var cy = viewport.PixelToY(y);
            var rowOffset = y * request.Width;

            for (var x = 0; x < request.Width; x++)
            {
                var cx = viewport.PixelToX(x);
                var escape = request.Type == FractalType.Mandelbrot
                    ? MandelbrotCalculator.Calculate(cx, cy, request.MaxIterations)
                    : JuliaCalculator.Calculate(cx, cy, request.JuliaCx, request.JuliaCy, request.MaxIterations);

                pixels[rowOffset + x] = ColorPalette.Map(escape, request.MaxIterations, request.Palette);
            }
        });

        return pixels;
    }
}
