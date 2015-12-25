using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Serialization;
using SignatureRecognition.Core;

namespace SignatureRecognition.WinForms
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        public Signatures Signatures { get; set; }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            openFileDialog.Filter = "xML files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Signatures));
                            XmlReader reader = XmlReader.Create(myStream);

                            Signatures = (Signatures)serializer.Deserialize(reader);
                            myStream.Close();
                            UpdateData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public void UpdateData()
        {
            foreach (var signature in Signatures.SList)
            {
                comboBox1.Items.Add("Подпись " + Signatures.SList.IndexOf(signature));
            }
            if (comboBox1.SelectedIndex == -1 && comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        public void DrawChart(Signature s)
        {
            chart1.Series.Clear();
            Series series1 = new Series("X");
            series1.ChartType = SeriesChartType.Line;
            foreach (var point in s.Points)
            {
                series1.Points.AddY(point.X);
            }
            chart1.Series.Add(series1);

            chart2.Series.Clear();
            Series series2 = new Series("Y");
            series2.ChartType = SeriesChartType.Line;
            foreach (var point in s.Points)
            {
                series2.Points.AddY(point.Y);
            }
            chart2.Series.Add(series2);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(Signatures.SList[comboBox1.SelectedIndex]);
        }
    }
}
