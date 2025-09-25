using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using Newtonsoft.Json;

//Neo Göl Dahlgren, neogoldahlgren@gmail.com, L0002B, Inlämningsuppgift 2
namespace Inlämningsuppgift_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Hämtar det som står i "databasen" (.json filen) genom att kalla funktionen ReadFile, sparar returvärdet (en JSON sträng) i variabeln db
            string db = ReadFile();
            //Om databasen är tom (dvs att JSON strängen inte innehåller några tecken) så skrivs "[]" till filen genom funktionen WriteToFile, detta för att filen alltid ska innehålla en array.
            if (db.Length == 0) { WriteToFile("[]"); }
            DataLabel.Text = "wassup";
        }

        //Funktion som används för att skriva ut säljarinformationen till applikationen.Till denna funktion skickas säljarnas nivå och en array av Seller instanser
        private void PrintStatistics(int level, Seller[] array)
        {
            //Loop som körs lika många gånger som arrayen som skickas med som argument är lång.
            for (int i = 0; i < array.Length; i++)
            {
                //DataLabel konkateneras med information om den säljare som iterationen för nuvarande är på. Namn, personnummer, distrikt och Sålda artiklar skrivs ut, med \n för att ha ett radbryt mellan.
                DataLabel.Text += $"Namn: {array[i].Name}\nPersonnummer: {array[i].Pnr}\nDistrikt: {array[i].District}\nSålda Artiklar: {array[i].SoldArticles}\n\n";
                //If-statement som undersöker om loopen är på sin sista iteration. Detta genom att kolla om i är lika med arrayens längd - 1. Detta eftersom loopen bara körs sålänge i är mindre än arrayens längd.
                if (i == array.Length - 1)
                {
                    //DataLabel konkateneras med information om hur många säljare som uppnått den nuvarande nivån. Antalen säljare skrivs ut genom att skriva ut array.Length (hur många instanser som finns i arrayen)
                    //SellerLevelArticlesSold funktionen kallas med level som argument. Detta för att få ut var gränsen av sålda artiklar går för just denna nivån.
                    DataLabel.Text += $"{array.Length} Säljare har nått nivå {level}: {SellerLevelArticlesSold(level)} Artiklar\n\n";
                }
            }
        }

        //Denna funktion används för att läsa det som står i databasfilen och få det returnerat.
        private string ReadFile()
        {
            //ReadAllText metoden körs. Med den skickas filnamnet som argument. Även denna metod kommer jag åt via Using System.IO
            return File.ReadAllText("db.json");
        }

        //Denna funktion räknar ut vilken nivå säljaren har nått, beroende på hur många artiklar denna sålt. Antalet artiklar skickas som argument till funktionen. Säljarens nivå returneras.
        private int SellerLevel(int soldArticles)
        {
            //If-else statement som undersöker hur många artiklar säljaren sålt, och vilken nivå den isåfall uppnått och returnerar en int med rätt nivå.
            if (soldArticles < 50)
            {
                return 1;
            }
            else if (soldArticles < 100)
            {
                return 2;
            }
            else if (soldArticles < 200)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        //Funktion som används för att få en sålda artiklar gräns för en säljarnivå. Funktionen tar emot en int som argument (säljarnivån).
        private string SellerLevelArticlesSold(int level)
        {
            //If-else statement som undersöker vad säljarnivån är, och returnerar var gränsen för sålda artiklar går för just den nivån.
            if (level == 4) { return "över 199"; }
            else if (level == 3) { return "100-199"; }
            else if (level == 2) { return "50-99"; }
            else { return "under 50"; }
        }

        //Denna funktion används för att skriva en Seller instans till databasfilen, då funktionen WriteToFile endast tar emot strängar. Den tar en instans av Seller som argument.
        private void WriteToDb(Seller seller)
        {
            //Först hämtas databasen genom funktionen ReadFile. Detta eftersom när man skriver till en fil via File.WriteAllText skrivs hela filen över. För att inte förlora den data som redan finns i filen, läses den in först
            string db = ReadFile();
            //Här skapas en ny array av Seller instanser. Denna array sparas i dbArray. JsonConvert.DeserializeObject används här för att konvertera db variablen, som bara innehåller en sträng, till en array. Jag har arbetat med JSON i JavaScript tidigare så jag google runt lite för att se om detta även gick att göra i C#. Just denna JSON metod kommer jag åt via Using Newtonsoft.Json. <Seller[]> skrivs till JSON metoden för att den ska veta vilken typ av object den ska konvertera strängen till. I detta fall vill jag konvertera strängen till en array av Seller instanser. 
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            //Eftersom alla arrayer i C# har en fast längd kan jag inte lägga till den nya Seller instansen direkt i dbArray. Därför konverterar jag arrayen till en List. En arrayliknande typ som låter mig lägga till en ny Seller instans. Här skapar jag därför en ny instans av List klassen, och använder <Seller[]> för att berätta vilken typ av data jag vill ha. Dessuttom skickar jag med dbArray.
            List<Seller> dbList = new List<Seller>(dbArray);
            //Nu när arrayen "konverterats" till en List instans, kan jag använda metoden .Add för att lägga till den nya Seller instansen i listan / arrayen.
            dbList.Add(seller);
            //WriteToFile funktionen kallas. Eftersom denna funktion bara tar emot strängar använder jag JsonConvert.SerializeObject för att konvertera dbList till en JSON sträng. Skickar denna JSON sträng som argument.
            WriteToFile(JsonConvert.SerializeObject(dbList));
            //Funktionen returnerar
            return;
        }

        //Funktion som används för att skriva det som skickas med som argument till databasfilen
        private void WriteToFile(string content)
        {
            //WriteAllText metoden körs. Till den skickas filnamnet och värdet på argumentet som argument. Metoden klassen kommer jag åt genom Using System.IO
            File.WriteAllText("db.json", content);
        }

        //Funktion som körs när "Registrera Säljare" knappen trycks på
        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            //try-statement för att fånga upp eventuella errors när funktionen körs
            try
            {
                //Variabler som sparar det som står i de olika TextBoxarna, solArticles konverteras till en int
                string name = NameTB.Text;
                string pnr = PnrTB.Text;
                string district = DistrictTB.Text;
                //Här kan koden kasta ett error, om texten som står i TextBoxen inte går att konvertera till en int
                int soldArticles = int.Parse(SoldArticlesTB.Text);
                //if-else statement som kollar om någon av de andra TextBoxarna är tomma, om de är det skrivs ett meddelande om detta till användaren och funktionen returnerar
                if (name == "")
                {
                    StatusLabel.Text = "En av textfälten var tomma eller innehöll fel datatyp.";
                    return;
                } 
                else if (pnr == "")
                {
                    StatusLabel.Text = "En av textfälten var tomma eller innehöll fel datatyp.";
                    return;
                } 
                else if (district == "")
                {
                    StatusLabel.Text = "En av textfälten var tomma eller innehöll fel datatyp.";
                    return;
                }
                //En ny instans av klassen "Seller" skapas och de fyra variablerna ovan skickas med till klassens constructor
                Seller seller = new Seller(name, pnr, district, soldArticles);
                //WriteToDb funktionen kallas och instansen som skapades ovan skickas med som argument, för att skriva den till db filen
                WriteToDb(seller);
                //StatusLabel uppdateras med ett meddelande för att informera användaren 
                StatusLabel.Text = "Säljare lades till.";

                //Alla TextBoxar och DataLabel "töms" dvs att allas .Text attribut sätts till tomma strängar
                NameTB.Text = "";
                PnrTB.Text = "";
                DistrictTB.Text = "";
                SoldArticlesTB.Text = "";
                DataLabel.Text = "";
            }
            //Om ett error kastas, körs koden i catch-statementet
            catch
            {
                //StatusLabel uppdateras för att informera användaren om att ett error inträffat och funktionen returnerar
                StatusLabel.Text = "En av textfälten var tomma eller innehöll fel datatyp.";
                return;
            }
        }
        //Funktion som körs när "Avsluta" knappen trycks på
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            //Metod som stänger ner applikationen
            Application.Exit(); 
        }

        //Funktion som körs när "Visa Statistik" knappen trycks på
        private void ShowStatsBtn_Click(object sender, EventArgs e)
        {
            //Tömmer DataLabel genom att sätta dens Text attribut till en tom sträng
            DataLabel.Text = "";
            //Hämtar återigen det som står i databasfilen genom funktionen ReadFile. Returvärdet läggs i db variabeln
            string db = ReadFile();
            //Skapar en ny array av Seller instanser "dbArray". Samma JSON metod som ovan används för att konvertera den sträng som finns i db till en array av Seller instanser.
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            //Sort metoden körs på dbArray. Denna metod används för att sortera en array utifrån en callback funktion. Som argument till metoden skickas själva arrayen (dbarray) och callback funktionen. Callbackfunktionen jämför varje Sellerinstans soldArticles attribut mot varandra. För att det störtsa värdet (säljen som sålt mest) ska ligga först i arrayen körs b.soldArticles - a.soldArticles.
            Array.Sort(dbArray, (a, b) => b.SoldArticles - a.SoldArticles);
            //Här skapas en ny array, för att filtrera ut de säljare som nått nivå 4. Här används FindAll metoden för att genomföra detta. FindAll metoden filtrerar ut alla (i detta fall) Sellerinstanser som gör att callbackfunktionen returnerar true. Just i denna callback jämför jag om säljaren "level" är lika med 4, utifrån hur många artiklar de sålt. Jag skickar med attributet .soldArticles till funktionen SellerLevel för att få tillbaka vilken nivå säljaren är.
            Seller[] level4Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 4);
            //PrintStatistic funktionen kallas för att skriva ut alla säljare som nått nivå 4. Integern 4 och arrayen med alla nivå 4-säljare skickas med som argument.
            PrintStatistics(4, level4Array);
            //Nedan repeteras samma operationer som ovan fast med nivå 3, 2, och 1.
            Seller[] level3Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 3);
            PrintStatistics(3, level3Array);
            Seller[] level2Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 2);
            PrintStatistics(2, level2Array);
            Seller[] level1Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 1);
            PrintStatistics(1, level1Array);
        }
    }
    //Klassen "Seller" definieras. Detta är en egen klass jag skapade för att enkelt kunna skapa objekt som ska kunna representra de säljare som registreras
    public class Seller
    {
        //Klassens olika attribut definieras, alla representerar en form av data från TextBoxarna
        public string Name { get; set; }
        public string Pnr { get; set; }
        public string District { get; set; }
        public int SoldArticles { get; set; }

        //Denna funktion är klassens constructor. Denna funktion körs nör en ny instans av klassen skapas, och värdena argumenten som skickas med sätts till attributen
        public Seller(string name, string pnr, string district, int soldArticles)
        {
            Name = name;
            Pnr = pnr;
            District = district;
            SoldArticles = soldArticles;
        }
    }
}
