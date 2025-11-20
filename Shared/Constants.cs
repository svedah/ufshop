namespace ufshop.Shared;

static public class Constants
{
    static public string DOMAINNAME = "ufshop.nu";
    static public string DEFAULTDOMAIN = "www";


    //storlek för logo
    static public int LOGOIMAGEHEIGHT = 400;
    static public int LOGOIMAGEWIDTH = 400;

    //storlek och kvalitet för varje uppladdad bild
    static public int IMAGEHEIGHT = 800;
    static public int IMAGEWIDTH = 800;
    static public int IMAGEQUALITY = 50;//0-sämst, 100-bäst
    static public string IMAGEPATH = "img" + Path.DirectorySeparatorChar;
    //storlek och kvalitet för varje uppladdad bild (thumbnail)
    static public int THUMBIMAGEWIDTH = 96;
    static public int THUMBIMAGEHEIGHT = 96;
    static public int THUMBIMAGEQUALITY = 75;//0-sämst, 100-bäst
    static public string THUMBIMAGEPATH = "img" + Path.DirectorySeparatorChar + "t" + Path.DirectorySeparatorChar;


    //max storlek för uppladdade filer
    static public int MAXALLOWEDFILESIZE = 10_000_000;

    // default-bild som används när ingen bild finns för vara eller företagslogo
    static public string EMPTYIMAGEFILENAME = "Empty.jpeg";
}
