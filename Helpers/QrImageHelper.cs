// using Net.Codecrete.QrCodeGenerator;
using SkiaSharp.QrCode.Image;

namespace ufshop.Helpers;
static public class QrImageHelper
{
    public static string MakeEmbeddedImage(string text)
    {
        if (text is null || text.Length == 0)
        {
            text = string.Empty;
        }
        string dtext = System.Web.HttpUtility.UrlDecode(text);
        byte[] data = QRCodeImageBuilder.GetPngBytes(dtext);
        var dataBase64 = Convert.ToBase64String(data);
        string output = "data:image/png;base64," + dataBase64;
        return output;
    }

    // Net.Codecrete.QrCodeGenerator;
    // public static string MakeEmbeddedImage(string text)
    // {
    //     string dtext = System.Web.HttpUtility.UrlDecode(text);
    //     int borderWidth = 2;
    //     var qrCode = QrCode.EncodeText(dtext, QrCode.Ecc.High);
    //     byte[] png = qrCode.ToPng(4, (int)borderWidth);
    //     var pngBase64 = Convert.ToBase64String(png);
    //     string output = "data:image/png;base64," + pngBase64;
    //     return output;
    // }

}