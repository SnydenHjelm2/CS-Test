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
using System.Text.Json;
using Newtonsoft.Json;

namespace Inlämningsuppgift_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string db = ReadFile();
            if (db.Length == 0) { WriteToFile("[]"); }
        }

        private string ReadFile()
        {
            return File.ReadAllText("db.json");
        }

        private int SellerLevel(int soldArticles)
        {
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

        private void WriteToDb(Seller seller)
        {
            string db = ReadFile();
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            List<Seller> dbList = new List<Seller>(dbArray);
            dbList.Add(seller);
            WriteToFile(JsonConvert.SerializeObject(dbList));
            return;
        }

        private bool WriteToFile(string content)
        {
            try
            {
                File.WriteAllText("db.json", content);
                return true;
            } 
            catch
            {
                return false;
            }
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string name = NameTB.Text;
            string pnr = PnrTB.Text;
            string district = DistrictTB.Text;
            int soldArticles = int.Parse(SoldArticlesTB.Text);
            Seller seller = new Seller(name, pnr, district, soldArticles);
            WriteToDb(seller);
            StatusLabel.Text = "Säljare lades till.";

            NameTB.Text = "";
            PnrTB.Text = "";
            DistrictTB.Text = "";
            SoldArticlesTB.Text = "";
            DataLabel.Text = "";
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void ShowStatsBtn_Click(object sender, EventArgs e)
        {
            string db = ReadFile();
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            /*for (int i = 0;  i < dbArray.Length; i++)
            {
                int sellerLevel = SellerLevel(dbArray[i].SoldArticles);
                DataLabel.Text += $"Namn: {dbArray[i].Name}\nPersonnummer: {dbArray[i].Pnr}\nDistrikt: {dbArray[i].District}\nSålda Artiklar: {dbArray[i].SoldArticles}\n\n";
            }*/
            Seller[] level4Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 4);
            for (int i = 0; i < level4Array.Length; i++)
            {
                DataLabel.Text += $"Namn: {level4Array[i].Name}\nPersonnummer: {level4Array[i].Pnr}\nDistrikt: {level4Array[i].District}\nSålda Artiklar: {level4Array[i].SoldArticles}\n\n";
                if (i == level4Array.Length - 1)
                {
                    DataLabel.Text += $"{level4Array.Length} Säljare har nått nivå 4: över 199 Artiklar\n\n";
                }
            }

            Seller[] level3Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 3);
            for (int i = 0; i < level3Array.Length; i++)
            {
                DataLabel.Text += $"Namn: {level3Array[i].Name}\nPersonnummer: {level3Array[i].Pnr}\nDistrikt: {level3Array[i].District}\nSålda Artiklar: {level3Array[i].SoldArticles}\n\n";
                if (i == level3Array.Length - 1)
                {
                    DataLabel.Text += $"{level3Array.Length} Säljare har nått nivå 3: 100-199 Artiklar\n\n";
                }
            }
        }
    }

    public class Seller
    {
        public string Name { get; set; }
        public string Pnr { get; set; }
        public string District { get; set; }
        public int SoldArticles { get; set; }

        public Seller(string name, string pnr, string district, int soldArticles)
        {
            Name = name;
            Pnr = pnr;
            District = district;
            SoldArticles = soldArticles;
        }
    }
}
