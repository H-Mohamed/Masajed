using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Media;

namespace Masjid
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> login = new Dictionary<string, string>();
        
        public Form1()
        {
            InitializeComponent();
         }
        public void load()
        {
            textBox2.Text = "";
            textBox1.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //importer login.bin ver login
            importlogins();
            bool valid = false;
            foreach (string k in login.Keys) {
                if (k == textBox1.Text && login[k] == textBox2.Text)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                label6.Show();
                load();
            }
            else logedin();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(100, 100);
        }
        //fonctions
        private void logedin() {
            Form f = new Form2();
            ActiveForm.Hide();
            f.Show();
        }
        private void importlogins()
        {
            bool exist = false;
            Stream g = new FileStream("login.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            login = (Dictionary<string, string>)bf.Deserialize(g);
            g.Close();
            foreach (string k in login.Keys)
            {
                if (k == "Admin" && login[k] == "Admin")
                {
                    exist = true; break;
                }
            }
            if(!exist) login.Add("Admin", "Admin");

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ActiveForm.FormBorderStyle == FormBorderStyle.Sizable)
            {
                ActiveForm.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                ActiveForm.FormBorderStyle = FormBorderStyle.Sizable;
            } 
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == true)
                textBox2.UseSystemPasswordChar = false;
            else
                textBox2.UseSystemPasswordChar = true;

        }
    }
}
