using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.Diagnostics.Contracts;

using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class AdministrationUsersService
{
    public readonly BeService beService;
    public AdministrationUsersService(BeService srv)
    {
        beService = srv;
    }

    public IQueryable<IdentityUser> AllUsers()
    {
        return beService.DbContext.Users.AsQueryable();
    }

    public IdentityUser EmptyUser()
    {
        return new IdentityUser
        {
            Id = Guid.Empty.ToString(),
            UserName = "empty",
            NormalizedUserName = "EMPTY",
            Email = "em@il.com",
            NormalizedEmail = "EM@IL.COM",
            PasswordHash = string.Empty,
            SecurityStamp = Guid.Empty.ToString(),
            ConcurrencyStamp = Guid.Empty.ToString(),
        };
    }

    public bool GetUser(Guid id, out IdentityUser user)
    {
        string sid = id.ToString();
        bool output = beService.DbContext.Users.Where(e => e.Id.Equals(sid)).Any();
        if (output)
        {
            user = beService.DbContext.Users.Where(e => e.Id.Equals(sid)).First();
        }
        else
        {
            user = EmptyUser();
        }
        return output;
    }

    public void SetPassword(string id, string password)
    {
        bool exists = beService.DbContext.Users.Where(e => e.Id.Equals(id)).Any();
        if (exists)
        {
            ApplicationUser user = (ApplicationUser)beService.DbContext.Users.Where(e => e.Id.Equals(id)).First();
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, password);
            beService.DbContext.Users.Update(user);
            beService.DbContext.SaveChanges();
        }
    }


}