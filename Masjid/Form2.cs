using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.ComponentModel;


namespace Masjid
{
    public partial class Form2 : Form
    {
        //notimportant
        //
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        //dictionaries
        //
        Dictionary<string, string> loginfo = new Dictionary<string, string>();
        Dictionary<int, Dictionary<int, double>> years = new Dictionary<int, Dictionary<int, double>>();
        Dictionary<int, double> monthsandvalue = new Dictionary<int, double>();
        //boolean criteria
        bool edited = false;
        bool titleadded = false;
        //form2
        //
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(0, 0);
            load_logins();
            ressourcebox.Hide();
            editlogins.Hide();
            if (!titleadded)
            {
                this.chart1.Titles.Add("جدول القيم المالية حسب الاشهر لكل سنة");
                titleadded = true;
            }
            
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        //buttons
        //
        private void deluserbtn_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (DataGridViewRow R in loginsdata.SelectedRows)
            {
                index = loginsdata.Rows.IndexOf(R);
                break;
            }
            if (MessageBox.Show("تأكيد الحذف؟") == DialogResult.OK)
            {
                foreach (string key in loginfo.Keys)
                {
                    if (key == loginsdata.Rows[index].Cells[1].Value.ToString())
                    {
                        loginfo.Remove(key);
                        break;
                    }
                }

                loginsdata.Rows.RemoveAt(index);
            }

        }
        private void changeloginbtn_Click(object sender, EventArgs e)
        {
            changer_login();
        }
        private void changepassbtn_Click(object sender, EventArgs e)
        {
            changer_pass();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            bool exist = false;
            string L = Interaction.InputBox("إسم المستخدم الجديد");
            if (L != "")
            {

                foreach (string key in Form1.login.Keys)
                {
                    if (L == key)
                    {
                        exist = true; break;
                    }
                }
                if (!exist)
                {
                    string P = Interaction.InputBox("كلمة المرور");
                    if (P != "")
                    {

                        loginsdata.Rows.Add(L, P);
                        loginfo.Add(L, P);
                    }
                }
                else
                    MessageBox.Show("إسم المستخدم الذي أدخلتم موجود مسبقا في قاعدة البيانات", "تنبيه");
            }
        }
        private void button9_Click_1(object sender, EventArgs e)
        {
            savelogins();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ressourcebox.Hide();
            editlogins.Show();
            sum();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button9_Click_2(object sender, EventArgs e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            combosource.Items.Clear();
            combostats.Items.Clear();
            editlogins.Hide();
            ressourcebox.Show();
            for(int o =2016;o<2040;o++)
            {
                combosource.Items.Add(o);
                combostats.Items.Add(o);
            } 
            combosource.SelectedIndex = 0;
            combostats.SelectedIndex = 0;
            charger_years();
            charger_datasource(int.Parse(combosource.Text));
            chart_load(int.Parse(combostats.Text));
            sum();
        }
        private void combosource_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            charger_datasource(int.Parse(combosource.Text));
            sum();
        }
        private void savesourcebtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل تريد حفظ التعديلات؟") == DialogResult.OK)
            {
                saveyearsandvalues(int.Parse(combosource.Text));
                Stream f = new FileStream("yearsdata.bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(f, years);
                f.Close();
            }
        }
        private void clearbtnsource_Click(object sender, EventArgs e)
        {
            datadesource.Rows.Clear();
            for (int i = 1; i <= 12; i++)
            {
                datadesource.Rows.Add(0, i);
            }
            sum();
            edited = true;
        }
        private void editsourcebtn_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("أدخل القيمة الجديدة");

            foreach (DataGridViewRow R in datadesource.SelectedRows)
            {
                R.Cells[0].Value = double.Parse(s);
            }
            sum();
            edited = true;
        }
        //menu strip
        //
        private void أبيضوخلفيةفضيةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.ActiveForm.BackColor = Color.LightGray;
        }
        private void خلفيةسوداءToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.ActiveForm.BackColor = Color.Black;
        }
        private void عاديToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveForm.Font = new Font("Segoe UI Semilight", 10);

        }
        private void أندلسيToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveForm.Font = new Font("Segoe UI Semilight", 10, FontStyle.Bold);
            
            load_logins();
        }
        private void ساعدنيToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open help file
            //Process.Start("chrome.exe", help file path);       
        }
        private void حفظالتعديلاتToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void تسجيلالخروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edited)
            {
                if (MessageBox.Show("هل تريد حفظ التعديلات؟") == DialogResult.OK)
                { 
                    //save 
                }
            }
            logout();
        }
        private void طباعةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialog f = new OpenFileDialog() { Filter = "Text Files|*.txt|All Files|*.*" };
            //string s = "";
            //foreach (string k in loginfo.Keys)
            //{
            //    s += k + loginfo[k] + "\n";
            //}

            //if (f.ShowDialog() == DialogResult.OK)
            //{
            //    string fileName;
            //    fileName = f.FileName;
            //    //var application = new Microsoft.Office.Interop.Word.Application();
            //    //var document = application.Documents.Open(@"D:\ICT.docx");
            //    //read all text into content
            //    content = System.IO.File.ReadAllText(fileName);
            //    //var document = application.Documents.Open(@fileName);
            //    PrintDialog printDlg = new PrintDialog();
            //    PrintDocument printDoc = new PrintDocument();
            //    PrintPreviewDialog prt = new PrintPreviewDialog();
            //    printDoc.DocumentName = fileName;
            //    prt.Document = printDoc;
            //    printDlg.Document = printDoc;
            //    printDlg.AllowSelection = true;
            //    printDlg.AllowSomePages = true;
            //    printDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);

            //    prt.ShowDialog();

            //    //Call ShowDialog
            //    //printDoc.DocumentName = f.FileName;
            //    //printDlg.Document = printDoc;
            //    //printDlg.AllowSelection = true;
            //    //printDlg.AllowSomePages = true;
            //    //prt.Document = printDoc;
            //    //prt.ShowDialog();
            //    ////////////
            //}
            
        }
        //Fontions
        //String content = "";
        //private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    ev.Graphics.DrawString(content, Font, Brushes.Black,
        //                    ev.MarginBounds.Left, 0, new StringFormat());
        //}
        private void sauvegarde_txt()
        { }
        private void sauvegarde_xml()
        { }
        private void logout()
        {
            Form f = new Form1();
            ActiveForm.Hide();
            f.Show();
        }
        //تعديل معلومات الدخول
        private void load_logins()
        {
            loginsdata.Rows.Clear();
            loginsdata.Columns.Clear();
            DataGridViewTextBoxColumn p = new DataGridViewTextBoxColumn();
            p.Name = "User";
            p.Width = loginsdata.Width / 2 - 1;
            p.HeaderCell = new DataGridViewColumnHeaderCell();
            p.HeaderText = "إسم المستخدم";
            DataGridViewTextBoxColumn n = new DataGridViewTextBoxColumn();
            n.Name = "pass";
            n.Width = loginsdata.Width / 2 - 1;
            n.HeaderCell = new DataGridViewColumnHeaderCell();
            n.HeaderText = "كلمة المرور";
            loginsdata.Columns.AddRange(new DataGridViewColumn[] { p, n });
            loginfo =Form1.login;

            foreach (string k in loginfo.Keys)
            {
                loginsdata.Rows.Add(k, Form1.login[k]);
            }
        }  
        private void changer_pass()
        {
            //rechercher f datagrid login w nbdel lih lpass
            int index=0;
            if(loginsdata.SelectedRows.Count!=0)
            {
                foreach(DataGridViewRow R in loginsdata.SelectedRows)
                {
                    index = loginsdata.Rows.IndexOf(R);
                    break;
                }
                //idkhel new pass
                string s = Interaction.InputBox("'"+loginsdata.Rows[index].Cells[1].Value.ToString() + "'" + ":" + "كلمة المرور الحالية", "تغيير معلومات الدخول", "كلمة المرور الجديدة");
                if(s!="") loginsdata.Rows[index].Cells[1].Value = s;
                else MessageBox.Show("لا يمكن اعتماد كلمة مرور فارغة");
            }

            //idkhel new pass
            // string s = Interaction.InputBox(Form1.login[] + ":" + "كلمة المرور الحالية", "تغيير معلومات الدخول", "كلمة المرور الجديدة");

        }
        private void changer_login()
        {
            //rechercher f datagrid login w nbdel lih lpass
            int index = 0;
            if (loginsdata.SelectedRows.Count != 0)
            {
                foreach (DataGridViewRow R in loginsdata.SelectedRows)
                {
                    index = loginsdata.Rows.IndexOf(R);
                    break;
                }
                //idkhel new pass
                string s = Interaction.InputBox("'"+loginsdata.Rows[index].Cells[0].Value.ToString()+"'" + ":" + "إسم المستخدم الحالي", "تغيير معلومات الدخول", "إسم المستخدم الجديد");
                if (s != "") loginsdata.Rows[index].Cells[0].Value = s;
                else MessageBox.Show("لا يمكن اعتماد إسم مستخدم فارغ");

               
            }
        }
        private void savelogins()
        {
            loginfo.Clear();
            foreach (DataGridViewRow R in loginsdata.Rows)
            {
                loginfo.Add(R.Cells[0].Value.ToString(), R.Cells[1].Value.ToString());
            }
            Stream g = new FileStream("login.bin", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(g, Form1.login);
            g.Close();
        }    
        //معاينة الموارد
        private void charger_years() {
            Stream f = new FileStream("yearsdata.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            if(f.Length!=0)
            years = (Dictionary<int, Dictionary<int, double>>)bf.Deserialize(f);
            f.Close();
            
        }
        private void charger_datasource(int year)
        {
            datadesource.Rows.Clear();
          
            bool exist = false;
            foreach(int key in years.Keys)
            {
                if (year == key)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist || years.Count == 0)
            {
                for (int i = 1; i <= 12; i++)
                {
                    datadesource.Rows.Add(0,i);
                }
            }
            else
            {
                foreach(int k in years.Keys)
                {
                    if (k == year)
                    {
                        Dictionary<int, double> dict = years[k];
                        foreach(int a in dict.Keys)
                        {
                            datadesource.Rows.Add(dict[a], a);
                        }
                        break;
                    }

                }
            }
            foreach (DataGridViewRow r in datadesource.Rows)
                r.Height = 38;

        }
        private void saveyearsandvalues(int year)
        {
            bool exit = false;
            monthsandvalue.Clear();
            foreach (DataGridViewRow R in datadesource.Rows)
            {
                double value = int.Parse(R.Cells[0].Value.ToString());
                int month = int.Parse(R.Cells[1].Value.ToString());
                monthsandvalue.Add(month,value);
            }
            foreach (int k in years.Keys)
            {
                if (k == year)
                    {
                      years[k] = monthsandvalue;
                      exit = true;
                      break;
                    }
            }
            if (!exit)
            {
                years.Add(year, monthsandvalue);
            }
            
            edited = false;
        }
        private void savedatabtn1()
        {
            Stream f = new FileStream("yearsdata.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(f, years);
            f.Close();
            edited = false;
        }
        private void valueto0sourcebtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in datadesource.SelectedRows) r.Cells[0].Value = 0;
            sum();
            edited = true;
        }
        private void sum()
        {
            double a = 0;
            foreach (DataGridViewRow r in datadesource.Rows) a += double.Parse(r.Cells[0].Value.ToString());
            total.Text = a.ToString();
        }
        private void datadesource_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void chart_load(int year)
        {
            chart1.Series.Clear();
            //// Data arrays.
            //int[] seriesArray =  { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            //double[] pointsArray = new double[12];
            //this.chart1.Series["Value"].Points.AddXY(a, dict[a]);
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 12; 
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10000;
            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisY.Interval = 1000;
            double total = 0;
            Series s = new Series("Value");
            s.Font = new Font("Arial", 15, FontStyle.Bold);
            s.XValueType = ChartValueType.Int32; 
            s.YValueType = ChartValueType.Double;
            s.ChartType = SeriesChartType.Spline;
            s.LegendText = "القيم المالية بالدرهم";
            foreach (int k in years.Keys)
            {
                if (k == year)
                {
                    Dictionary<int, double> dict = years[k];
                    foreach (int a in dict.Keys)
                    {
                        s.Points.AddXY(a, dict[a]);
                        total += dict[a];
                    }
                    break;
                }

            }

            this.chart1.Series.Add(s);
            total1.Text = total.ToString();
            //// Set palette.
            //this.chart1.Palette = ChartColorPalette.SeaGreen;

            //// Set title.

            //// Add series.
            //for (int i = 0; i < seriesArray.Length; i++)
            //{
            //    // Add series.
            //    Series series = this.chart1.Series.Add(seriesArray[i].ToString());

            //    // Add point.
            //    series.Points.Add(pointsArray[i]);
            //}
        }

        private void combostats_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart_load(int.Parse(combostats.Text));
        }
    }
}
