using System.Text;
using ufshop.Shared;
namespace ufshop.Helpers;

public class DomainExtract
{
    private readonly string Input;
    public readonly string TopLevelDomain;
    public readonly string Domain;
    public readonly string Prefix;
    public readonly int Port;

    public string FullDefaultDomain
    {
        get
        {
            return Constants.DEFAULTDOMAINPREFIX + "." + Constants.DOMAINNAME;
        }
    }

    public string DebugOutput
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine("Input: " + Input);
            sb.AppendLine("Port: " + Port);
            sb.AppendLine("Prefix: " + Prefix);
            sb.AppendLine("Domain: " + Domain);
            sb.AppendLine("TLD: " + TopLevelDomain);
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public DomainExtract(string input)
    {
        Input = input;
        Port = ExtractPort(input); // OK
        ExtractPrefixDomainTld(input, out Prefix, out Domain, out TopLevelDomain);
    }

    private int ExtractPort(string input)
    {
        int output = -1;
        if (input is not null && input.Contains(':'))
        {
            string[] split = input.Split(':');
            string portnumber = split[split.Length - 1];
            int.TryParse(portnumber, out output);
        }
        return output;
    }

    private void ExtractPrefixDomainTld(string input, out string prefix, out string domain, out string tld)
    {
        prefix = string.Empty;
        domain = string.Empty;
        tld = string.Empty;

        if (input is not null && input.Contains("."))
        {
            string[] split = input.Split(".");
            if (split.Length >= 2)
            {
                tld = CleanTld(split[split.Length - 1]).ToLower();
                domain = split[split.Length - 2].ToLower();

                if (split.Length > 2)
                {
                    prefix = split[split.Length - 3].ToLower();
                }
                else
                {
                    prefix = Constants.DEFAULTDOMAINPREFIX;
                }
            }
        }
        else
        {
            //localhost lands here
            // prefix = Constants.DEFAULTDOMAINPREFIX;
            // domain = Constants.DOMAINNAME.Split('.')[0];
            // tld = Constants.DOMAINNAME.Split('.')[1];
        }
    }

    private string CleanTld(string input)
    {
        string output = string.Empty;
        if (input.Contains(":"))
        {
            string[] splitted = input.Split(":");
            output = splitted[0];
        }
        else
        {
            output = input;
        }
        return output;
    }



}