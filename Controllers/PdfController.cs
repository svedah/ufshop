using Microsoft.AspNetCore.Mvc;

// using Net.Codecrete.QrCodeGenerator;

namespace ufshop.Controllers;

public class PDFController : ControllerBase
{
    [HttpGet]
    [Route("[controller]/orderconfirmation/{id?}")]
    public IActionResult OrderConfirmation(Guid id) //id = shopOrderId
    {
        return new FileContentResult(new byte[0], "application/pdf");
    }
    // var html = File.ReadAllText("order.html");
    // byte[] pdf = Freeware.Html2Pdf.Convert(html);
    // File.WriteAllBytes("pdf.pdf", pdf);

}
