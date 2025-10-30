using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//Neo Göl Dahlgren, neogoldahlgren@gmail.com, L0002B, Inlämningsuppgift 3
namespace Inlämningsuppgift_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RegisterPersonBtn_Click(object sender, EventArgs e)
        {
            if (FormPanel.Visible) { FormPanel.Visible = false; }
            else {  FormPanel.Visible = true; ResultLabel.Text = "Resultat:"; }
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            ResultLabel.Text = "Resultat:";
            string firstName = FirstNameTB.Text;
            string lastName = LastNameTB.Text;
            string pnr = PnrTB.Text;

            Person person = new Person(firstName, lastName, pnr);
            if (!person.PnrChecker(person.Pnr)) { ResultLabel.Text += "\nPersonnummer inte OK, försök igen"; }
            else 
            { 
                ResultLabel.Text += $"\nFörnamn: {person.FirstName}\nEfternamn: {person.LastName}\nPersonnummer: {person.Pnr}\nKön: {person.Gender}";
                FirstNameTB.Text = "";
                LastNameTB.Text = "";
                PnrTB.Text = "";
            }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pnr { get; set; }
        public string Gender { get; set; }

        public Person(string firstName, string lastName, string pnr)
        {
            FirstName = firstName;
            LastName = lastName;
            Pnr = pnr;
            Gender = GenderChecker(Pnr);
        }

        public string GenderChecker(string pnr)
        {
            int genderNumber = int.Parse(pnr[8].ToString());
            if (genderNumber % 2 == 0) { return "Kvinna"; }
            else { return "Man"; }
        }

        public bool PnrChecker(string pnr)
        {
            if (pnr.Length != 10) { return false; }

            char[] numbersArr = pnr.ToCharArray();
            int[] intArr = new int[numbersArr.Length];
            for (int i = 0; i < numbersArr.Length; i++)
            {
                intArr[i] = int.Parse(numbersArr[i].ToString());
            }

            int[] controlNumbers = new int[intArr.Length];
            int multiInt = 2;
            for (int i = 0; i < intArr.Length;i++)
            {
                int multiProduct = intArr[i] * multiInt;
                if (multiProduct > 9)
                {
                    string stringifiedProduct = multiProduct.ToString();
                    char[] charArr = stringifiedProduct.ToCharArray();
                    int[] smallIntArr = new int[charArr.Length];
                    smallIntArr[0] = int.Parse(charArr[0].ToString());
                    smallIntArr[1] = int.Parse(charArr[1].ToString());

                    multiProduct = smallIntArr[0] + smallIntArr[1];
                }

                controlNumbers[i] = multiProduct;

                if (multiInt == 2) { multiInt = 1; }
                else { multiInt = 2; }
            }

            int controlNumbersSum = 0;
            for (int i = 0; i < controlNumbers.Length; i++)
            {
                controlNumbersSum += controlNumbers[i];
            }

            Debug.WriteLine(controlNumbersSum);
            if (controlNumbersSum % 10 == 0) { return true; }
            else { return false; }
        }
    }
}


//hej