using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ufshop.Services;

namespace ufshop.Data.Seed;
public class SeederIsacShop
{
    readonly DbContext DbContext;
    public SeederIsacShop(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Seed()
    {
        
    }
}
    