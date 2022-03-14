using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }

        private void btnZGraph_Click(object sender, EventArgs e)
        {
            string Max_Graph = textBox3.Text;
            int Graph_max = int.Parse(Max_Graph);
            double[] x_values = new double[Graph_max/2];
            double[] Y_medianCount = new double[Graph_max/2];
            double[] Y_medianQuick = new double[Graph_max/2];
            double time_before;
            double time_after;
            int index = 0;
            for (int i = 3; i <= Graph_max; i+=2)
            {
                x_values[index] = i;
                index++;
            }
            for(int i = 0; i<x_values.Length;i++)
            {
                
                time_before = System.Environment.TickCount;
                Adaptive_median.Adaptive_count(ImageMatrix, (int)x_values[i]);
                time_after = System.Environment.TickCount;
                Y_medianCount[i] = (time_after - time_before);
 
                time_before = System.Environment.TickCount;
                Adaptive_median.Adaptive_quick(ImageMatrix, (int)x_values[i]);
                time_after = System.Environment.TickCount;
                Y_medianQuick[i] = (time_after - time_before);
            }
            //Create a graph and add two curves to it
             ZGraphForm ZGF = new ZGraphForm("Sample Graph", "Window_size", "Time");
            ZGF.add_curve("Counting", x_values, Y_medianCount,Color.Red);
            ZGF.add_curve("Quick", x_values, Y_medianQuick, Color.Blue);
            ZGF.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                
                string sizeText = textBox1.Text;
                byte[,] filtred;
                int size = int.Parse(sizeText);
                int start = System.Environment.TickCount;
                filtred = Adaptive_median.Adaptive_count(ImageMatrix, size);
                int end = System.Environment.TickCount;
                double time = (end - start) / 1000;
            label4.Text = time.ToString();
            label4.Text += ".s";
            ImageOperations.DisplayImage(filtred, pictureBox2);
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            byte[,] filtred;
            string Trim_text = textBox2.Text;
            int trim = int.Parse(Trim_text);
            string size_text = textBox1.Text;
            int size = int.Parse(size_text);
            int start = System.Environment.TickCount;
            filtred = Alpha.imagefilter(ImageMatrix,size,trim);
            int end = System.Environment.TickCount;
            double time = (end - start) / 1000;
            label4.Text = time.ToString();
            label4.Text += ".s";
            ImageOperations.DisplayImage(filtred, pictureBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
                byte[,] filtred;
                string sizeText = textBox1.Text;
                int size = int.Parse(sizeText);
            int start = System.Environment.TickCount;
            filtred = Adaptive_median.Adaptive_quick(ImageMatrix, size);
            int end = System.Environment.TickCount;
            double time = (end - start) / 1000;
            label4.Text = time.ToString();
            label4.Text += ".s";
            ImageOperations.DisplayImage(filtred, pictureBox2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[,] filtred;
            string Trim_text = textBox2.Text;
            int trim = int.Parse(Trim_text);
            string size_text = textBox1.Text;
            int size = int.Parse(size_text);
            int start = System.Environment.TickCount;
            filtred = Alpha.imagefilter1(ImageMatrix, size, trim);
            int end = System.Environment.TickCount;
            double time = (end - start) / 1000;
            label4.Text = time.ToString();
            label4.Text += ".s";
            ImageOperations.DisplayImage(filtred, pictureBox2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string Max_Graph = textBox3.Text;
            int Graph_max = int.Parse(Max_Graph);
            string Trim_text = textBox2.Text;
            int trim = int.Parse(Trim_text);
            double[] x_values = new double[Graph_max/2];
            double[] Y_AlphaCount = new double[Graph_max/2];
            double[] Y_Alphaselect = new double[Graph_max/2];
            double time_before;
            double time_after;
            int index = 0;
            for (int i = 3; i <= Graph_max; i += 2)
            {
                x_values[index] = i;
                index++;
            }
            for (int i = 0; i < x_values.Length; i++)
            {
               
                time_before = System.Environment.TickCount;
                Alpha.imagefilter(ImageMatrix, (int)x_values[i],trim);
                time_after = System.Environment.TickCount;
                Y_AlphaCount[i] = (time_after - time_before);

                time_before = System.Environment.TickCount;
                Alpha.imagefilter1(ImageMatrix, (int)x_values[i],trim);
                time_after = System.Environment.TickCount;
                Y_Alphaselect[i] = (time_after - time_before);
            }
            //Create a graph and add two curves to it
            ZGraphForm ZGF = new ZGraphForm("Sample Graph", "Window_size", "Time");
            ZGF.add_curve("counting", x_values, Y_AlphaCount, Color.Red);
            ZGF.add_curve("select_Kth", x_values, Y_Alphaselect, Color.Blue);
            ZGF.Show();
        }
    }
}