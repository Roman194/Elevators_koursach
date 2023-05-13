using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elevators_koursach
{
    public partial class Report : Form
    {
        static Report report= new Report();
        public Report()
        {
            InitializeComponent();
        }
        public static void ShowBox(string system_time, uint average_waiting_time, uint max_waiting_time)
        {
            report.label4.Text=system_time;
            if (average_waiting_time < 15)
                report.label5.ForeColor = Color.Green;
            else
            {
                if (average_waiting_time > 30)
                    report.label5.ForeColor = Color.Red;
                else
                    report.label5.ForeColor = Color.Orange;
            }
            report.label5.Text=Convert.ToString(average_waiting_time);

            if(max_waiting_time < 15) 
                report.label6.ForeColor=Color.Green;
            else
            {
                if (max_waiting_time > 30)
                    report.label6.ForeColor = Color.Red;
                else
                    report.label6.ForeColor = Color.Brown;
            }
            report.label6.Text = Convert.ToString(max_waiting_time);
            report.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            report.Dispose();
        }
    }
}
