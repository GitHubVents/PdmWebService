using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTaskSystemGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add((int)ServiceReference1.VibroInsertionTypes.Twenty_mm);

            comboBox1.Items.Add((int)ServiceReference1.VibroInsertionTypes.Thirty_mm);
            service = new ServiceReference1.SolidWebServiceClient();
        }
        ServiceReference1.ISolidWebService service;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                service = new ServiceReference1.SolidWebServiceClient();
                //(service as ServiceReference1.SolidWebServiceClient).Endpoint.Address = new System.ServiceModel.EndpointAddress(textBox3.Text);        
                service.CreateVibroInsertion(Int32.Parse(textBox2.Text), Int32.Parse(textBox1.Text), (ServiceReference1.VibroInsertionTypes)comboBox1.SelectedItem, 0);
            }
            catch
            {
                MessageBox.Show("The end");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            service = new ServiceReference1.SolidWebServiceClient();
            service.OpenSolidWorks();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Random rnd = new Random();

                service = new ServiceReference1.SolidWebServiceClient();
                service.CreateVibroInsertion(rnd.Next(200, 2000), rnd.Next(200, 2000), (ServiceReference1.VibroInsertionTypes) (rnd.Next(2,3)*10), 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
