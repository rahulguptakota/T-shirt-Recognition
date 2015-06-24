using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace ProductSearch
{
    public partial class Form1 : Form
    {
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        int i;
        static int sizeSample = 40;
        static int sizeResult = 12;
        static string location="C:\\tshirt recognition\\";
        PictureBox[] pictureBoxes = new PictureBox[sizeResult];
        LinkLabel[] linkLabels = new LinkLabel[sizeResult];
        Label[] labels = new Label[sizeResult];
        string[] productName = new string[sizeSample];
        string[] imageLocation = new string[sizeSample];
        string[] productLink = new string[sizeSample];
        double[] rating = new double[sizeSample];
        string[] prices = new string[sizeSample];


        public Form1()
        {
            InitializeComponent();
            pictureBoxes = this.Controls.OfType<PictureBox>().ToArray();
            linkLabels = this.Controls.OfType<LinkLabel>().ToArray();
            labels[0] = label4; labels[1] = label5; labels[2] = label6; labels[3] = label7; labels[4] = label8; labels[5] = label9; labels[6] = label10; labels[7] = label11; labels[8] = label12; labels[9] = label13; labels[10] = label14; labels[11] = label15;
            Array.Reverse(pictureBoxes);
            Array.Reverse(linkLabels);
            label3.Visible = false;
            progressBar1.Visible = false;
            for(i=0;i<12;i++)
            {
                pictureBoxes[i].Visible = false;
                linkLabels[i].Visible = false;
                labels[i].Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Browse for an image";
            fdlg.InitialDirectory = location;
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fdlg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form = new Form2();
                form.pictureBox1.Image = new Bitmap(@textBox1.Text);
                form.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                form.ShowDialog();
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Please upload an image.", "Error!", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(location+"amazon_data.xlsx");
            MySheet = (Excel.Worksheet)MyBook.Sheets[1];
            for(i=0;i<sizeSample;i++)
            {
                productName[i] = MySheet.get_Range("A" + (i+1).ToString()).Value2;
                imageLocation[i] = location+"images\\"+MySheet.get_Range("B" + (i + 1).ToString()).Value2;
                productLink[i] = MySheet.get_Range("D" + (i + 1).ToString()).Value2;
                prices[i] = MySheet.get_Range("C" + (i + 1).ToString()).Value2;
            }

            progressBar1.Maximum = sizeSample;
            progressBar1.Step = 1;
            progressBar1.Visible = true;

            for (i = 0; i < sizeSample; i++)
            {
                string filename1 = location + "i1.txt";
                string filename2 = location + "i2.txt";
                FileStream fs = new FileStream(filename1, FileMode.Create);
                StreamWriter w = new StreamWriter(fs);
                w.Write(textBox1.Text);
                w.Flush();
                w.Close();
                fs.Close();
                fs = new FileStream(filename2, FileMode.Create);
                w = new StreamWriter(fs);
                w.Write(imageLocation[i]);
                w.Flush();
                w.Close();
                fs.Close();

                Process process = Process.Start(location + "SSIM_new.exe");
                process.WaitForExit();

                StreamReader w1 = new StreamReader(location + "Rate.txt");
                string rate = w1.ReadLine();
                w1.Close();
                rating[i] = Convert.ToDouble(rate);
                progressBar1.PerformStep();
            }


            Array.Sort(rating.ToArray(), productName);
            Array.Sort(rating.ToArray(), imageLocation);
            Array.Sort(rating.ToArray(), productLink);
            Array.Sort(rating.ToArray(), prices);
            Array.Sort(rating);
            Array.Reverse(productName);
            Array.Reverse(imageLocation);
            Array.Reverse(productLink);
            Array.Reverse(prices);
            Array.Reverse(rating);

            label3.Visible = true;
            for(i=0;i<sizeResult;i++)
            {
                pictureBoxes[i].Image = new Bitmap(imageLocation[i]);
                linkLabels[i].Text = productName[i];
                labels[i].Text = prices[i];
                pictureBoxes[i].Visible = true;
                linkLabels[i].Visible = true;
                labels[i].Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[0]);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[0]);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[1]);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[1]);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[2]);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[2]);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[3]);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[3]);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[4]);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[4]);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[5]);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[5]);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[6]);
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[6]);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[7]);
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[7]);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[8]);
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[8]);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[9]);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[9]);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[10]);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[10]);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Process.Start(productLink[11]);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(productLink[11]);
        }
    }
}
