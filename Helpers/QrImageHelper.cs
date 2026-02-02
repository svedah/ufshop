using Net.Codecrete.QrCodeGenerator;

namespace ufshop.Helpers;
static public class QrImageHelper
{
    public static string MakeEmbeddedImage(string text)
    {
        string dtext = System.Web.HttpUtility.UrlDecode(text);
        int borderWidth = 2;
        var qrCode = QrCode.EncodeText(dtext, QrCode.Ecc.High);
        byte[] png = qrCode.ToPng(4, (int)borderWidth);
        var pngBase64 = Convert.ToBase64String(png);
        string output = "data:image/png;base64," + pngBase64;
        return output;
    }

}