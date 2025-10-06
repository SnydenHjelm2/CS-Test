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
            else {  FormPanel.Visible = true; }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pnr { get; set; }

        public Person(string firstName, string lastName, string pnr)
        {
            FirstName = firstName;
            LastName = lastName;
            Pnr = pnr;
        }

        public bool PnrChecker(string pnr)
        {
            return true;
        }
    }
}
