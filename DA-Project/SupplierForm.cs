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
    public partial class SupplierForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        public SupplierForm()
        {
            InitializeComponent();
        }

        private void SupplierForm_Load(object sender, EventArgs e)
        {
            RefreshListView();

        }

        public void RefreshListView()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;

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
            foreach (Supplier s in WarehouseEnt.Suppliers)
            {
                string[] WRow = { s.Supplier_ID.ToString(), s.Supplier_Name, s.Supplier_Mobile.ToString(), s.Supplier_Phone.ToString(), s.Supplier_Fax.ToString(), s.Supplier_Email, s.Supplier_website };
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

            Supplier supplier = new Supplier();
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty && textBox5.Text != string.Empty && textBox6.Text != string.Empty && textBox7.Text != string.Empty)
            {
                try
                {
                    Supplier s = WarehouseEnt.Suppliers.Find(int.Parse(textBox1.Text));
                    if (s == null)
                    {

                        supplier.Supplier_ID = int.Parse(textBox1.Text);


                        supplier.Supplier_ID = int.Parse(textBox1.Text);
                        supplier.Supplier_Name = textBox2.Text;
                        supplier.Supplier_Mobile = int.Parse(textBox3.Text);
                        supplier.Supplier_Phone = int.Parse(textBox4.Text);
                        supplier.Supplier_Fax = int.Parse(textBox5.Text);
                        supplier.Supplier_Email = textBox6.Text;
                        supplier.Supplier_website = textBox7.Text;
                        WarehouseEnt.Suppliers.Add(supplier);
                        MessageBox.Show("Supplier Added");
                        WarehouseEnt.SaveChanges();
                        RefreshListView();


                    }
                    else { MessageBox.Show("Supplier Exists"); }
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
                MessageBox.Show("Missing Supplier ID Field");
            }
            else
            {
                try
                {
                    Supplier s = WarehouseEnt.Suppliers.Find(int.Parse(textBox1.Text));
                    if (s != null)
                    {
                        if (textBox2.Text != string.Empty)
                        {
                            s.Supplier_Name = textBox2.Text;

                        }

                        if (textBox3.Text != string.Empty)
                        {
                            s.Supplier_Mobile = int.Parse(textBox3.Text);

                        }

                        if (textBox4.Text != string.Empty)
                        {
                            s.Supplier_Phone = int.Parse(textBox4.Text);
                        }

                        if (textBox5.Text != string.Empty)
                        {
                            s.Supplier_Fax = int.Parse(textBox5.Text);
                        }

                        if (textBox6.Text != string.Empty)
                        {
                            s.Supplier_Email = textBox6.Text;
                        }
                        if (textBox7.Text != string.Empty)
                        {
                            s.Supplier_website = textBox7.Text;
                        }
                        WarehouseEnt.SaveChanges();
                        RefreshListView();
                        MessageBox.Show("Supplier Updated");


                    }
                    else
                    {
                        MessageBox.Show("Supplier not found");
                    }
                }
                catch (FormatException)
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
