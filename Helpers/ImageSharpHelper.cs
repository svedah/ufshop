// dotnet add package SixLabors.ImageSharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ufshop.Shared;

namespace ufshop.Helpers;
public static class ImageSharpHelper
{
    public static bool OpenImageFromArray(byte[] input, out Image image)
    {
        image = Image.Load(input);
        return image is not null;
    }

    public static bool OpenImageFromStream(MemoryStream input, out Image image)
    {
        input.Seek(0, SeekOrigin.Begin);
        var data = input.ToArray();
        return OpenImageFromArray(data, out image);
        // image = Image.Load(data);
        // return image is not null;
    }

    public static MemoryStream SaveImageToStream(Image input, int quality)
    {
        var coder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
        {
            Quality = quality
        };
        var output = new MemoryStream();
        input.Save(output, coder);
        return output;
    }

    public static Image CropToSquare(Image input)
    {
        Image output;
        int cropsize = Math.Min(input.Width, input.Height);
        if (input.Width != input.Height)
        {
            int x = 0;
            int y = 0;
            if (input.Width > input.Height)
            {
                x = (input.Width - input.Height) / 2;
            }
            else if (input.Width < input.Height)
            {
                y = (input.Height - input.Width) / 2;
            }
            var r = new Rectangle(x, y, cropsize, cropsize);

            // input.Mutate(i => i.Crop(r));
            output = input.Clone(i => i.Crop(r));
        }
        else
        {
            output = input;
        }
        return output;
    }

    public static Image Resize(Image input, int width, int height)
    {
        // input.Mutate(i => i.Resize(width, height));
        Image output = input.Clone(i => i.Resize(width, height));
        return output;
    }
}
/*
    internal static void Main(string[] args)
    {
        string[] inputfiles = new string[]
        {
            "portrait.jpg", "landscape.jpg"
        };

        foreach (string name in inputfiles)
        {
            var file = File.ReadAllBytes("input" + Path.DirectorySeparatorChar + name);
            var ms = new MemoryStream(file);

            SixLabors.ImageSharp.Image image;
            if (ImageSharpHelper.OpenImageFromStream(ms, out image))
            {
                var imageCropped = ImageSharpHelper.CropToSquare(image);
                var imageResized = ImageSharpHelper.Resize(imageCropped, 200, 200);
                var cms = ImageSharpHelper.SaveImageToStream(imageCropped).ToArray();
                var rms = ImageSharpHelper.SaveImageToStream(imageResized).ToArray();
                File.WriteAllBytes(name + "-cropped.jpg", cms);
                File.WriteAllBytes(name + "-resized.jpg", rms);
            }

        }

    }
*/