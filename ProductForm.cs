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
    public partial class ProductForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        public ProductForm()
        {
            InitializeComponent();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            RefreshListView();

        }
        public void RefreshListView()
        {
            textBox1.Text = textBox2.Text = textBox5.Text = comboBox2.Text = string.Empty;
            dateTimePicker1.Refresh();
            dateTimePicker2.Refresh();
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox1.Visible = false;
            listView1.View = View.Details;
            listView1.Clear();
            comboBox1.Items.Clear();
            listView1.Columns.Add("Product Code");
            listView1.Columns.Add("Product Name");
            listView1.Columns.Add("Production Date");
            listView1.Columns.Add("Expiration Date");
            listView1.Columns.Add("Supplier ID");
            listView1.Columns.Add("Supplier Name");
            listView1.Columns.Add("Unit");
            for (int i = 0; i < 7; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }



            foreach (Product p in WarehouseEnt.Products)
            {
                Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);



                foreach (ProductUnit pu in WarehouseEnt.ProductUnits.Where(pu => pu.Pcode == p.Pcode))
                {


                    string[] WRow = { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, pu.Unit };
                    var listViewItemWarehouse = new ListViewItem(WRow);
                    listView1.Items.Add(listViewItemWarehouse);
                    for (int i = 0; i < 7; i++)
                    {
                        listView1.Columns[i].Width = -2;
                    }


                }
            }
            foreach (Supplier s in WarehouseEnt.Suppliers)
            {
                comboBox2.Items.Add(s.Supplier_ID + " | " + s.Supplier_Name);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product();


            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && dateTimePicker1.Value.ToString() != string.Empty && dateTimePicker1.Value.ToString() != string.Empty && !(string.IsNullOrEmpty(comboBox2.Text)))
            {
                try
                {

                    Product p = WarehouseEnt.Products.Find(int.Parse(textBox1.Text));
                    if (p == null)
                    {

                        product.Pcode = int.Parse(textBox1.Text);



                        product.Expiration_date = dateTimePicker2.Value;
                        product.Production_Date = dateTimePicker1.Value;
                        product.Pcode = int.Parse(textBox1.Text);
                        product.P_Name = textBox2.Text;
                        string SupplierSelected = comboBox2.Text.Split('|')[0];

                        product.Supplier_ID = int.Parse(SupplierSelected);

                        string[] unit = textBox5.Text.Split(',');
                        for (int i = 0; i < unit.Length; i++)
                        {
                            ProductUnit productUnit = new ProductUnit();

                            productUnit.Pcode = int.Parse(textBox1.Text);
                            productUnit.Unit = unit[i];
                            WarehouseEnt.ProductUnits.Add(productUnit);
                        }

                        WarehouseEnt.Products.Add(product);
                        WarehouseEnt.SaveChanges();
                        MessageBox.Show("Product Added");


                        RefreshListView();



                    }


                    else { MessageBox.Show("Product Exists"); }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Inputs not in correct format");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Missing Product Code Field");
            }
            else
            {
                try
                {

                    Product product = WarehouseEnt.Products.Find(int.Parse(textBox1.Text));

                    if (product != null)
                    {

                        if (textBox2.Text != string.Empty)
                        {
                            product.P_Name = textBox2.Text;

                        }

                        if (dateTimePicker1.Value != product.Production_Date)
                        {

                            product.Production_Date = dateTimePicker1.Value;





                        }

                        if (dateTimePicker2.Value != product.Expiration_date)
                        {
                            product.Expiration_date = dateTimePicker2.Value;




                        }

                        if (!string.IsNullOrEmpty(comboBox1.Text))
                        {
                            if (textBox5.Text != string.Empty)
                            {
                                ProductUnit ProductUnit = WarehouseEnt.ProductUnits.Find(int.Parse(textBox1.Text), comboBox1.Text);
                                WarehouseEnt.ProductUnits.Remove(ProductUnit);
                                string[] unit = textBox5.Text.Split(',');
                                for (int i = 0; i < unit.Length; i++)
                                {
                                    ProductUnit productUnit = new ProductUnit();

                                    productUnit.Pcode = int.Parse(textBox1.Text);
                                    productUnit.Unit = unit[i];
                                    WarehouseEnt.ProductUnits.Add(productUnit);
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(comboBox2.Text))
                        {
                            product.Supplier_ID = int.Parse(comboBox2.Text.Split('|')[0]);
                        }
                        WarehouseEnt.SaveChanges();
                        MessageBox.Show("Product Updated");
                        RefreshListView();

                    }
                    else
                    {
                        MessageBox.Show("Product not found");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show(" Inputs not in correct format");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm   = new MainMenuForm();   
                mainMenuForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

            try
            {
                Product product = WarehouseEnt.Products.Find(int.Parse(textBox1.Text));

                if (product != null)
                {
                    comboBox1.Visible = true;
                    MessageBox.Show("Product Exits\nTo update product please select the unit from the \ncombobox and proceed to change the values\n(multiple units can be added during the update process, simply add the units seperated by a comma");

                    var productUnits = WarehouseEnt.ProductUnits.AsEnumerable();

                    var queryProdUnits = (from pu in productUnits
                                          where pu.Pcode == int.Parse(textBox1.Text)
                                          select pu).ToList();
                    foreach (var pu in queryProdUnits)
                    {
                        comboBox1.Items.Add(pu.Unit);
                    }

                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Product Code is not in valid format");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
