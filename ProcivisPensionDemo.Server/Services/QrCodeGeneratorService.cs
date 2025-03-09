
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.Common;
using ZXing;


public interface IQRCodeService
{
    byte[] GenerateQRCode(string text, int width = 250, int height = 250);
}

public class QRCodeService : IQRCodeService
{
    public byte[] GenerateQRCode(string text, int width = 250, int height = 250)
    {
        var barcodeWriter = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 1
            }
        };

        // Generate the pixel data
        var pixelData = barcodeWriter.Write(text);

        // Create a Bitmap from the pixel data
        using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
        {
            // Lock the bitmap's bits
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                                ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            try
            {
                // Copy the pixel data into the bitmap's buffer
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            // Save the bitmap to a MemoryStream as a PNG
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
