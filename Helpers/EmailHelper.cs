using System.ComponentModel.DataAnnotations;

namespace ufshop.Helpers;

public class EmailHelper
{
    public bool IsValidEmail(string input)
    {
        return new EmailAddressAttribute().IsValid(input);
    }
}