using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace ufshop.Services;

public class OrderPDFService
{
    public readonly BeService beService;
    public OrderPDFService(BeService srv)
    {
        beService = srv;
    }

    public string BuildPdfTemplate(Guid Id)//shopOrder Id
    {
        string output = string.Empty;
        Shop shop;
        bool shopOrderExists = beService.DbContext.ShopOrders.Where(e=>e.Id.Equals(Id)).Any();
        if (shopOrderExists && new ShopService(beService).GetShop(beService.DomainPrefix, out shop))
        {
            // ShopOrder shopOrder = new ShopOrderService(beService).Get(Id);
            ShopOrder shopOrder = shop.Orders.Where(e => e.Id.Equals(Id)).First();
            output = BuildHtml(shop, shopOrder);            
        }
        return output;
    }

    private string BuildHtml(Shop shop, ShopOrder shopOrder)
    {
        StringBuilder sb = new StringBuilder();
        // CustomerInfo customerInfo = shopOrder.CustomerInfo;
        BuildHtml_HeaderBodyStart(ref sb);
        BuildHtml_HeaderTag(ref sb, shop.Settings.Title, shop.Settings.LogoImage.Filename);
        BuildHtml_InfoSection(ref sb, shop, shopOrder);
        BuildHtml_ProductTable(ref sb, shopOrder);
        BuildHtml_Summary(ref sb, shop, shopOrder);
        BuildHtml_Footer(ref sb, shop);
        return sb.ToString();
    }

    private void BuildHtml_Footer(ref StringBuilder sb, Shop shop)
    {
        sb.AppendLine("<footer>");
        sb.AppendLine("<div>Tack för din beställning<br />");
        sb.AppendLine("Vid frågor, kontakta oss på ");
        sb.AppendLine(shop.Settings.ContactInfo.Email);//email
        sb.AppendLine(" eller ");
        sb.AppendLine(shop.Settings.ContactInfo.MobileNumber);//telefonnummer
        sb.AppendLine("</footer>");
        sb.AppendLine("</body></html>");
    }

    private int CalculateShopOrderSum(ShopOrder shopOrder)
    {
        int output = 0;
        foreach(CartItem cartItem in shopOrder.Cart.CartItems)
        {
            output += (cartItem.Amount * cartItem.Price);
        }
        return output;
    }

    private void BuildHtml_Summary(ref StringBuilder sb, Shop shop, ShopOrder shopOrder)
    {
        int shopOrderSum = CalculateShopOrderSum(shopOrder);
        sb.AppendLine("<div class=\"summary\">");

        sb.AppendLine("<div class=\"summary-row\">");
        sb.AppendLine("<span>Summa</span>");
        sb.AppendLine("<span>" + shopOrderSum + " SEK</span>");//Summa
        sb.AppendLine("</div>");

        sb.AppendLine("<div class=\"summary-row\">");
        sb.AppendLine("<span>Frakt</span>");
        sb.AppendLine("<span>" + shop.Settings.BaseShippingPrice + " SEK</span>");//Basfraktpris
        sb.AppendLine("</div>");

        sb.AppendLine("<div class=\"summary-row total\">");
        sb.AppendLine("<span>Att betala</span>");
        sb.AppendLine("<span>" + (shopOrderSum + shop.Settings.BaseShippingPrice) + " SEK</span>");//Totalpris
        sb.AppendLine("</div>");

        sb.AppendLine("</div>");
    }

    private void BuildHtml_ProductTable(ref StringBuilder sb, ShopOrder shopOrder)
    {
        sb.AppendLine("<h2>Beställda produkter</h2>");
        sb.AppendLine("<table><thead><tr><th>Produkt</th><th class=\"center\">Antal</th><th class=\"center\">Pris</th><th class=\"right\">Summa</th></tr></thead>");
        sb.AppendLine("<tbody>");
        //TODO: Lista produkter

        foreach(CartItem cartItem in shopOrder.Cart.CartItems)
        {
            sb.AppendLine("<tr>");

            sb.AppendLine("<td>");
            sb.AppendLine(cartItem.Title);
            sb.AppendLine("</td>");

            sb.AppendLine("<td class=\"center\">");
            sb.AppendLine(cartItem.Amount.ToString());
            sb.AppendLine("</td>");

            sb.AppendLine("<td class=\"center\">");
            sb.AppendLine(cartItem.Price.ToString() + " SEK");
            sb.AppendLine("</td>");

            sb.AppendLine("<td class=\"right\">");
            sb.AppendLine((cartItem.Price * cartItem.Amount).ToString() + " SEK");
            sb.AppendLine("</td>");

            sb.AppendLine("</tr>");
        }


        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");
    }

    private void BuildHtml_InfoSection(ref StringBuilder sb, Shop shop, ShopOrder shopOrder)
    {
        sb.AppendLine("<div class=\"info-section\">");
        sb.AppendLine("<div class=\"info-box\">");
        sb.AppendLine("<strong>Kunduppgifter</strong>");
        sb.AppendLine(shopOrder.CustomerInfo.FirstName + " " + shopOrder.CustomerInfo.LastName + "<br />");//Kundnamn
        sb.AppendLine(shopOrder.CustomerInfo.StreetName + " " + shopOrder.CustomerInfo.StreetNo + "<br />");//Leveransadress
        sb.AppendLine(shopOrder.CustomerInfo.ZipCode + " " + shopOrder.CustomerInfo.City + "<br />");//Zip och Stad
        sb.AppendLine("E-post: " + shopOrder.CustomerInfo.Email + "<br />");
        sb.AppendLine("Telefon: " + shopOrder.CustomerInfo.Phone);
        sb.AppendLine("</div>");
        sb.AppendLine("<div class=\"info-box\">");
        sb.AppendLine("<strong>Orderinformation</strong>");
        sb.AppendLine("Order ID: " + ShopOrderIdToShortShopOrderId(shopOrder.Id) + "<br />");//Order ID
        sb.AppendLine("Orderdatum: " + shopOrder.Created + "<br />");//Orderdatum
        sb.AppendLine("Betalsätt: Swish");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");
    }

    private string ShopOrderIdToShortShopOrderId(Guid id)
    {
        string sid = id.ToString().Split('-')[0].ToUpper();
        return sid;
    }

    private void BuildHtml_HeaderBodyStart(ref StringBuilder sb)
    {
        string[] input = File.ReadAllLines(beService.wwwroot + "/ordertemplate/headbody.html");
        foreach(string line in input)
        {
            sb.AppendLine(line);
        }
    }

    private void BuildHtml_HeaderTag(ref StringBuilder sb, string shopTitle, string shopImageFilename)
    {
        sb.AppendLine("<header>");
        sb.AppendLine("<div class=\"company-info\">");
        sb.AppendLine("<strong>" + shopTitle + "</strong><br />");
        sb.AppendLine("</div>");
        sb.AppendLine("<img src=\"https://www.ufshop.nu/img/" + shopImageFilename + ".jpeg\" />");
        // sb.AppendLine("<img src=\"https://www.ufshop.nu/img/WebbHelp_Logo.jpeg\" />");
        // sb.AppendLine("<p>" + shopImageFilename + "</p>");
        sb.AppendLine("</header>");
        sb.AppendLine("<h1>Orderbekräftelse</h1>");
    }


}