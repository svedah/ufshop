using Microsoft.AspNetCore.Mvc;

using Net.Codecrete.QrCodeGenerator;

namespace ufshop.Controllers;

public class QRController : ControllerBase
{
    public QRController()
    {
    }

    // public ActionResult<byte[]> GeneratePng([FromQuery(Name = "text")] string text,
    //         [FromQuery(Name = "ecc")] int? ecc, [FromQuery(Name = "border")] int? borderWidth)
    [Route("/QR/PNG/{text}")]
    public ActionResult<byte[]> Png(string text = "")
    {
        string dtext = System.Web.HttpUtility.UrlDecode(text);

        int borderWidth = 2;


        var qrCode = QrCode.EncodeText(dtext, QrCode.Ecc.High);
        byte[] png = qrCode.ToPng(4, (int)borderWidth);
        return new FileContentResult(png, "image/png");
    }

}
