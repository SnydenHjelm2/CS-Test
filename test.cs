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

        private void PrintStatistics(int level, Seller[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                DataLabel.Text += $"Namn: {array[i].Name}\nPersonnummer: {array[i].Pnr}\nDistrikt: {array[i].District}\nSålda Artiklar: {array[i].SoldArticles}\n\n";
                if (i == array.Length - 1)
                {
                    DataLabel.Text += $"{array.Length} Säljare har nått nivå {level}: över 199 Artiklar\n\n";
                }
            }
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
            Array.Sort(dbArray, (a, b) => b.SoldArticles - a.SoldArticles);
            Seller[] level4Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 4);
            PrintStatistics(4, level4Array);
            Seller[] level3Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 3);
            PrintStatistics(3, level3Array);
            Seller[] level2Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 2);
            PrintStatistics(2, level2Array);
            Seller[] level1Array = Array.FindAll(dbArray, x => SellerLevel(x.SoldArticles) == 1);
            PrintStatistics(1, level1Array);
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
