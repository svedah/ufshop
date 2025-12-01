namespace ufshop.Helpers;
static internal class PasswordHelper
{

    static private string[] countingwords =
    {
        "Tio", "Elva", "Tolv", "Tretton", "Fjorton", "Femton", "Sexton", "Sjutton", "Arton", "Nitton", "Tjugo", 
    };

    static private string[] adjectives =
    {
        "Glada", "Ledsna", "Torra", "Kalla", "Heta", "Ljusa", "Roliga", "Randiga", 
        "Runda", "Breda", "Tjocka", "Feta", "Raka", "Enkla", "Friska", "Starka", 
        "Stora", "Ilskna", "Vackra", "Lugna", "Tysta", "Pigga", "Unga", "Nya", 
        "Hela", "Mjuka", "Rena", "Lediga", "Billiga", "Rika", "Fulla", "Snabba", 
        "Legitima", "Vilda", "Formella", "Grunda", "Varma", "Korta", "Glada", 
        "Rutiga", "Platta", "Smala", "Tunna", "Krokiga", "Sneda", "Falska", 
        "Sanna", "Sjuka", "Svaga", "Tunga", "Fula", "Hungriga", "Oroliga", 
        "Ljudliga", "Gamla", "Levande", "Trasiga", "Upptagna", "Dyra", "Fattiga", 
        "Skrynkliga", "Tomma", "Farliga", "Olagliga", "Tama", "Informella", "Djupa"
    };

    static private string[] nouns = /*pluralform*/
    {
        "Bilar", "Hus", "Stolar", "Bord", "Lampor", "Vagnar", "Cyklar", "Skor", 
        "Jackor", "Klockor", "Ringar", "Halsband", "Blommor", 
        "Buskar", "Grenar", "Blad", "Frukter", "Bananer", 
        "Apelsiner", "Hundar", "Katter", "Kor", "Grisar", 
        "Kaniner", "Fiskar", "Barn", "Syskon",  
        "Elever", "Doktorer", "Poliser", "Byar", 
        "Berg", "Floder", "Hav", "Filmer", "Serier", "Tidningar", "Artiklar", 
        "Dikter", "Noveller", "Skivor", "Datorer", "Mobiler", "Kameror", 
        "Mikrofoner", "Tangentbord", "Program", "Spel", "Pengar", "Mynt", 
        "Sedlar", "Kort", "Checkar", "Priser", "Ideer", "Tankar", "Planer", 
        "Visioner", "Projekt", "Uppgifter", "Svar", "Bussar",  
        "Rum", "Tak", "Soffor", "Hyllor", "Pennor", "Papper", 
        "Bilder", "Filmer", "Scener", "Akter", "Kapitel", "Texter", "Namn", "Nummer", 
        "Listor", "Regler"
    };

    public static string GetRandomizedPassword()
    {
        int seed = (int)(new DateTime().Ticks);

        Random rnd = new Random(seed);

        int countingwordsindex = rnd.Next(0, countingwords.Length);
        int adjectivesindex = rnd.Next(0, adjectives.Length);
        int nounsindex = rnd.Next(0, nouns.Length);
        int number = rnd.Next(100, 1000);

        string output = countingwords[countingwordsindex] + adjectives[adjectivesindex] + nouns[nounsindex] + number.ToString();
        return output;
    }


}