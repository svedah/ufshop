using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ufshop.Services;
using ufshop.Data.Models;

namespace ufshop.Data.Seed;
public class SeederUsers
{
    private readonly DbContext DbContext;
    public SeederUsers(DbContext dbContext)
    {
        DbContext = dbContext;
    }


}
    