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
using System.Xml;
using System.Xml.Linq;
using nmonproc;

namespace nmon_proc_windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;

            //MessageBox.Show(dt1.ToString("yyyy-MM-dd"));
            try
            {
                Proc_nmon.Extract(dt1, @"c:\perfdata\");
            }
            catch
            {
                textBox1.Text = "File Not Found.";
                return;
            }
            XDocument xd = 
                XDocument.Load(@"c:\perfdata\nmon_perf_database.xml");
            //textBox1.Text = (xd.ToString());
            int days_cnt = xd.Elements("NMON_RECORDS").Elements().Count();
            textBox1.Text = String.Format("{0} days in database", days_cnt);
        }
        /*
        private void button2_Click(object sender, EventArgs e)
        {//read from xml database, and show graph
            string q_opt = "";
            RadioButton[] rbn = { radioButton1, radioButton3, radioButton4 };
            foreach (RadioButton r in rbn)
            {
                if (r.Checked == true) q_opt = r.Text;
            }
            textBox1.Text = $"{q_opt} is selected.";
            XDocument nmon_db = XDocument.Load
                (@"C:\perfdata\nmon_perf_database.xml");

            IEnumerable<XElement> xe = 
                nmon_db.Elements("NMON_RECORDS").Elements();

            foreach (XElement el in xe)
            {
                chart1.Series["series1"]
                    .Points.AddXY
                    (el.Name.ToString(), 
                    Convert.ToDouble
                    (el.Element($"{q_opt}_MAX").Value));
            }
        }*/
        private void button2_Click(object sender, EventArgs e)
        {//read from xml database, and show graph
            for (int i = 0; i<3; i++)
                chart1.Series[i].Points.Clear();
            string q_opt = "";
            string Xax; double Yax;
            RadioButton[] rbn = { radioButton1, radioButton3, radioButton4 };
            foreach (RadioButton r in rbn)
            {
                if (r.Checked == true) q_opt = r.Text;
            }
            textBox1.Text = $"{q_opt} is selected.";
            XDocument nmon_db = XDocument.Load
                (@"C:\perfdata\nmon_perf_database.xml");

            IEnumerable<XElement> xe =
                nmon_db.Elements("NMON_RECORDS").Elements();

            if (q_opt == "MEMNEW")
            {
                chart1.Series[0].Name = "PROCESS_MAX";
                chart1.Series[1].Name = "FSCACHE_MAX";
                chart1.Series[2].Name = "SYSTEM_MAX";
                chart1.Series[0].IsVisibleInLegend = true;
                chart1.Series[1].IsVisibleInLegend = true;
                chart1.Series[2].IsVisibleInLegend = true;
                foreach (XElement el in xe)
                {
                    Xax = el.Name.ToString();
                    Yax = Convert.ToDouble(el.Element($"{q_opt}").Attribute("PROCESS_MAX").Value);
                    chart1.Series[0].Points.AddXY(Xax, Yax);
                    Yax = Convert.ToDouble(el.Element($"{q_opt}").Attribute("FSCACHE_MAX").Value);
                    chart1.Series[1].Points.AddXY(Xax, Yax);
                    Yax = Convert.ToDouble(el.Element($"{q_opt}").Attribute("SYSTEM_MAX").Value);
                    chart1.Series[2].Points.AddXY(Xax, Yax);
                }
            }
            else if (q_opt == "IOPS" || q_opt == "CPU")
            {
                chart1.Series[0].Name = $"{q_opt}_MAX";
                chart1.Series[0].IsVisibleInLegend = true;
                chart1.Series[1].IsVisibleInLegend = false;
                chart1.Series[2].IsVisibleInLegend = false;
                foreach (XElement el in xe)
                {
                    //MessageBox.Show(chart1.Series[0].Name);
                    Xax = el.Name.ToString();
                    Yax = Convert.ToDouble(el.Element($"{q_opt}_MAX").Value);
                    chart1.Series[0]
                        .Points.AddXY(Xax, Yax);
                    //Debug//File.AppendAllText("log.txt", $"{Xax}\t{Yax}\r\n");

                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
