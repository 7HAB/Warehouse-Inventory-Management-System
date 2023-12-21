using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_Project
{
    public partial class CustomerForm : Form
    {
        Model1 WarehouseEnt = new Model1();

        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            RefreshListView();
        }
        public void RefreshListView()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = string.Empty;

            listView1.View = View.Details;
            listView1.Clear();
            listView1.Columns.Add(" ID     ");
            listView1.Columns.Add(" Name   ");
            listView1.Columns.Add("Mobile     ");
            listView1.Columns.Add("Phone   ");
            listView1.Columns.Add("Fax   ");
            listView1.Columns.Add("Email   ");
            listView1.Columns.Add("Website   ");



            for (int i = 0; i < 7; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }

            foreach (Customer c in WarehouseEnt.Customers)
            {
                string[] WRow = { c.Customer_ID.ToString(), c.Customer_Name, c.Mobile.ToString(), c.Phone.ToString(), c.Fax.ToString(), c.Email,c.website };
                var listViewItemWarehouse = new ListViewItem(WRow);
                listView1.Items.Add(listViewItemWarehouse);
                for (int i = 0; i < 7; i++)
                {
                    listView1.Columns[i].Width = -2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty && textBox5.Text != string.Empty && textBox6.Text != string.Empty && textBox7.Text !=string.Empty)
            {
                try
                {
                    Customer c = WarehouseEnt.Customers.Find(int.Parse(textBox1.Text));
                    if (c == null)
                    {

                        customer.Customer_ID = int.Parse(textBox1.Text);


                        customer.Customer_ID = int.Parse(textBox1.Text);
                        customer.Customer_Name = textBox2.Text;
                        customer.Mobile = int.Parse(textBox3.Text);
                        customer.Phone = int.Parse(textBox4.Text);
                        customer.Fax = int.Parse(textBox5.Text);
                        customer.Email = textBox6.Text;
                        customer.website = textBox7.Text;
                        WarehouseEnt.Customers.Add(customer);
                        MessageBox.Show("Customer Added");
                        WarehouseEnt.SaveChanges();
                        RefreshListView();


                    }
                    else { MessageBox.Show("Customer Exists"); }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Inputs not in correct format");
                }
            }
            else { MessageBox.Show("Missing data fields"); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Missing Customer ID Field");
            }
            else
            {
                try
                {
                    Customer c = WarehouseEnt.Customers.Find(int.Parse(textBox1.Text));
                    if (c != null)
                    {
                        if (textBox2.Text != string.Empty)
                        {
                            c.Customer_Name = textBox2.Text;

                        }

                        if (textBox3.Text != string.Empty)
                        {
                            c.Mobile = int.Parse(textBox3.Text);

                        }

                        if (textBox4.Text != string.Empty)
                        {
                            c.Phone = int.Parse(textBox4.Text);
                        }

                        if (textBox5.Text != string.Empty)
                        {
                            c.Fax = int.Parse(textBox5.Text);
                        }

                        if (textBox6.Text != string.Empty)
                        {
                            c.Email = textBox6.Text;
                        }
                        if (textBox7.Text != string.Empty)
                        {
                            c.website = textBox6.Text;
                        }

                        WarehouseEnt.SaveChanges();
                        RefreshListView();


                    }
                    else
                    {
                        MessageBox.Show("Customer not found");
                    }
                }
                catch 
                {
                    MessageBox.Show("Inputs not in correct format");

                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm = new MainMenuForm();
                mainMenuForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }
    }
}
