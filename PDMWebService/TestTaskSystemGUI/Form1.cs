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
      private  ServiceReference1.ISolidWebService service = null;
        private Random random = null;
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add((int)ServiceReference1.VibroInsertionTypes.Twenty_mm);
            comboBox1.Items.Add((int)ServiceReference1.VibroInsertionTypes.Thirty_mm);

            this.random =  new Random();
            service = new ServiceReference1.SolidWebServiceClient();
            (service as ServiceReference1.SolidWebServiceClient).Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            (service as ServiceReference1.SolidWebServiceClient).Endpoint.Binding.OpenTimeout = new TimeSpan(0, 10, 0);
            (service as ServiceReference1.SolidWebServiceClient).Endpoint.Binding.CloseTimeout = new TimeSpan(0, 10, 0);
            (service as ServiceReference1.SolidWebServiceClient).Endpoint.Binding.SendTimeout = new TimeSpan(0, 10, 0);
        }
       
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
            try
            {
                service = new ServiceReference1.SolidWebServiceClient();
                service.OpenSolidWorks();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                this.service.CreateVibroInsertion(random.Next(213, 3000), random.Next(213, 3000), (ServiceReference1.VibroInsertionTypes) (random.Next(2,3)*10), 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.service.CreateRoof(random.Next(213, 3000), random.Next(213, 3000), (ServiceReference1.RoofTypes) (random.Next(1, 6)), 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
         //   this.service.(random.Next(213, 3000), random.Next(213, 3000), (ServiceReference1.RoofTypes)(random.Next(1, 6)), 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                service.UploadDXF(new Random().Next(30000, 50000));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            }
    }
}
