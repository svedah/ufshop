using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class PaymentService
{
    public readonly BeService beService;
    public PaymentService(BeService srv)
    {
        beService = srv;
    }

}