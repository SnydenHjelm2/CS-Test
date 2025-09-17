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
            int pnr = int.Parse(PnrTB.Text);
            string district = DistrictTB.Text;
            int soldArticles = int.Parse(SoldArticlesTB.Text);
            Seller seller = new Seller(name, pnr, district, soldArticles);
            string db = ReadFile();
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            List<Seller> dbList = new List<Seller>(dbArray);
            dbList.Add(seller);
            WriteToFile(JsonConvert.SerializeObject(dbList));
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void ShowStatsBtn_Click(object sender, EventArgs e)
        {
            string db = ReadFile();
            Seller[] dbArray = JsonConvert.DeserializeObject<Seller[]>(db);
            for (int i = 0;  i < dbArray.Length; i++)
            {
                DataLabel.Text += $"Namn: {dbArray[i].Name}\nPersonnummer: {dbArray[i].Pnr}\nDistrikt: {dbArray[i].District}\nSålda Artiklar: {dbArray[i].SoldArticles}\n\n";
            }
        }
    }

    public class Seller
    {
        public string Name { get; set; }
        public int Pnr { get; set; }
        public string District { get; set; }
        public int SoldArticles { get; set; }

        public Seller(string name, int pnr, string district, int soldArticles)
        {
            Name = name;
            Pnr = pnr;
            District = district;
            SoldArticles = soldArticles;
        }
    }
}
