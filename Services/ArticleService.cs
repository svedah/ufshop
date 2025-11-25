using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ArticleService
{
    public readonly BeService beService;
    public ArticleService(BeService srv)
    {
        beService = srv;
    }

    // public bool GetArticle(Guid Id, out ShopItem output)
    // {
    //     //TODO: vad göra om artikel inte finns?
    //     output = new 
    //     return false;
    // }

}