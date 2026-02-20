using Microsoft.AspNetCore.Mvc;
using ufshop.Data.Models;
using ufshop.Services;

// using Net.Codecrete.QrCodeGenerator;

namespace ufshop.Controllers;

public class PDFController : ControllerBase
{
    private readonly BeService beService;
    public PDFController(BeService srv)
    {
        beService = srv;
    }

    [HttpGet]
    [Route("[controller]/orderconfirmation/{id?}")]
    public IActionResult OrderConfirmation(Guid id) //id = shopOrderId
    {
        IActionResult output = NotFound();
        ShopOrderService shopOrderService = new ShopOrderService(beService);
        bool shopOrderExists = shopOrderService.Exists(id);
        if (shopOrderExists)
        {
            OrderPDFService orderPDFService = new OrderPDFService(beService);
            string pdfTemplate = orderPDFService.BuildPdfTemplate(id);
            if (!string.IsNullOrWhiteSpace(pdfTemplate))
            {
                byte[] pdfData = Freeware.Html2Pdf.Convert(pdfTemplate);
                output = new FileContentResult(pdfData, "application/pdf");
            }
        }
        return output;
        // return new FileContentResult(new byte[0], "application/pdf");
    }

}
