// using Microsoft.AspNetCore.Mvc;
// using ufshop.Services;
// using ufshop.Shared;

// namespace ufshop.Helpers;

// static public class DomainHelper
// {

//     ///<summary>
//     /// Extraherar och returnerar subdomän
//     /// Exempel: www.example.com -> www 
//     ///</summary>
//     static public string ExtractSubDomain(IHttpContextAccessor httpContextAccessor)
//     {
//         string domain = string.Empty;
//         string output = string.Empty;

//         if (httpContextAccessor.HttpContext is not null)
//         {
//             domain = httpContextAccessor.HttpContext.Request.Host.Host;
//             output = domain.Replace("." + Constants.DOMAINNAME, string.Empty)
//                                 .Replace(Constants.DOMAINNAME, string.Empty);
//         }

//         if (output.Length == 0)
//         {
//             output = Constants.DEFAULTDOMAIN;
//         }

//         return output.ToLower();
//     }

// }
