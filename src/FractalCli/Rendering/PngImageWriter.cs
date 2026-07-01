using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FractalCli.Rendering;

public static class PngImageWriter
{
    public static void Write(string outputPath, Rgba32[] pixels, int width, int height)
    {
        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var image = Image.LoadPixelData(pixels, width, height);
        image.SaveAsPng(outputPath);
    }
}
