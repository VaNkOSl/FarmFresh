using System.Drawing;
using System.Drawing.Imaging;

namespace EntityDataGenerator;

internal sealed class PhotoGenerator
{
    public byte[] Generate(int width, int height)
    {
#pragma warning disable CA1416
        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.White);

        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Jpeg);

        return stream.ToArray();
#pragma warning restore CA1416
    }
}
