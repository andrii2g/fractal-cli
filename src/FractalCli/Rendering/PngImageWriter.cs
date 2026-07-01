using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FractalCli.Rendering;

public static class PngImageWriter
{
    public static void Write(string outputPath, Rgba32[] pixels, int width, int height)
    {
        using var image = Image.LoadPixelData(pixels, width, height);
        image.SaveAsPng(outputPath);
    }
}
