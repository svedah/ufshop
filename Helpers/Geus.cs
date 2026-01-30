namespace ufshop.Helpers;
using System.Linq;
public static class Geus
{
    static public string Generate(int length = 6)
    {
        Random rnd = new Random();
        var chars = "abcdefghijklmnopqrstuvwxyz";
        return new string
        (
            System.Linq.Enumerable
                    .Repeat(chars, length)
                    .Select(s => s[rnd.Next(s.Length)])
                    .ToArray()
        );
    }
}