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
    public partial class PermissionForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        List<Permission_Product> permission_Products = new List<Permission_Product>();
        List<Warehouse_Contains> warehouse_Contains = new List<Warehouse_Contains>();
        public static int warehouseContainsIndex = 0;
        public static int warehouseDispenseIndex = 0;
        public PermissionForm()
        {
            InitializeComponent();
        }

        private void PermissionForm_Load(object sender, EventArgs e)
        {
            RefreshListView();
            comboBox1.Items.Add("Dispense - صرف");
            comboBox1.Items.Add("Supply - توريد");
        }
        public void RefreshListView()
        {
            warehouseContainsIndex = 0;
            warehouseDispenseIndex = 0;
            textBox1.Text = textBox3.Text = textBox4.Text = textBox5.Text = string.Empty;
            comboBox1.Text = comboBox2.Text = comboBox3.Text = comboBox4.Text=comboBox5.Text = string.Empty;
            dateTimePicker1.Refresh();
            comboBox1.Enabled = true;
            textBox4.Enabled = false;
            label12.Enabled = false;
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            listView1.View = System.Windows.Forms.View.Details;
            listView1.Clear();
            listView1.Columns.Add("Permission Number");
            listView1.Columns.Add("Product Code");
            listView1.Columns.Add("Product Name");
            listView1.Columns.Add("Unit");
            listView1.Columns.Add("Quantity");
            listView1.Columns.Add("Production Date");
            listView1.Columns.Add("Expiration Date");
            listView1.Columns.Add("Supplier ID");
            listView1.Columns.Add("Supplier Name");
            listView1.Columns.Add("Warehouse ID");
            listView1.Columns.Add("Warehouse Name");
            listView1.Columns.Add("WarehouseContains ID");
            listView1.Columns.Add("Date of Permission");

            for (int i = 0; i < 13; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            listView2.View = System.Windows.Forms.View.Details;
            listView2.Clear();

            listView2.Columns.Add("Permission Number");
            listView2.Columns.Add("WarehouseContains ID");
            listView2.Columns.Add("Product Code");
            listView2.Columns.Add("Product Name");
            listView2.Columns.Add("Unit");
            listView2.Columns.Add("Quantity");
            listView2.Columns.Add("Customer ID");
            listView2.Columns.Add("Customer Name");
            listView2.Columns.Add("Warehouse ID");
            listView2.Columns.Add("Warehouse Name");
            listView2.Columns.Add("Date of Permission");

            for (int i = 0; i < 11; i++)
            {
                listView2.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
          
            

            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                comboBox5.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
            }
            foreach (Customer c  in WarehouseEnt.Customers)
            {
                comboBox4.Items.Add(c.Customer_Name + " | " + c.Customer_ID);
            }

            

            foreach (Permission perm in WarehouseEnt.Permissions)
            {


                foreach (Warehouse_Contains wc in WarehouseEnt.Warehouse_Contains.Where(wc => wc.Permission_Number == perm.Permission_Number))
                {
                    Product p = WarehouseEnt.Products.Find(wc.Pcode);
                    Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                    Warehouse w = WarehouseEnt.Warehouses.Find(wc.Warehouse_ID);


                    if (perm.Type_of_Permission == "Supply - توريد")
                    {

                        string[] WRow = { perm.Permission_Number.ToString(), wc.Pcode.ToString(), p.P_Name, wc.Unit, wc.Quantity.ToString(), p.Production_date.ToString("yyyy-MM-dd"), p.Expiration_date.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, wc.Warehouse_ID.ToString(), w.Warehouse_Name, wc.WarehouseContains_ID.ToString(), wc.Added_date.Value.ToString("yyyy-MM-dd") };
                        var listViewItemWarehouse = new ListViewItem(WRow);
                        listView1.Items.Add(listViewItemWarehouse);
                        for (int i = 0; i < 13; i++)
                        {
                            listView1.Columns[i].Width = -2;
                        }

                    }

                    else if (perm.Type_of_Permission == "Dispense - صرف")
                    {
                        string[] WRow = { perm.Permission_Number.ToString(), wc.WarehouseContains_ID.ToString(), wc.Pcode.ToString(), p.P_Name, wc.Unit, wc.Quantity.ToString(), p.Supplier_ID.ToString(), s.Supplier_Name, wc.Warehouse_ID.ToString(), w.Warehouse_Name, wc.Added_date.Value.ToString("yyyy-MM-dd") };
                        var listViewItemWarehouse = new ListViewItem(WRow);
                        listView2.Items.Add(listViewItemWarehouse);
                        for (int i = 0; i < 11; i++)
                        {
                            listView1.Columns[i].Width = -2;
                        }
                    }

                }
            }


        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Supply - توريد") {
            comboBox4.Visible = false;
                label13.Visible = false;
                label8.Visible = true;
                textBox5.Visible = true;
            }
            else if (comboBox1.Text == "Dispense - صرف") {
            label8.Visible = false;
                textBox5.Visible = false;
                comboBox4.Visible = true;
                label13.Visible = true;

            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = string.Empty;
            int warehouseID = int.Parse(comboBox5.Text.Split('|')[0]);
            if (comboBox1.Text == "Supply - توريد")
            {
                foreach (Product p in WarehouseEnt.Products)
                {
                    comboBox2.Items.Add(p.Pcode + " | " + p.P_Name);
                }

            }
            else if (comboBox1.Text == "Dispense - صرف")
            {
                
                var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                var queryWarehouseContains = (from wc in warehouseContains
                                      where wc.Warehouse_ID == warehouseID
                                      select wc).ToList();

                foreach (var wc in queryWarehouseContains)
                {
                    foreach (Product p in WarehouseEnt.Products.Where(p=>p.Pcode==wc.Pcode))
                    {
                        comboBox2.Items.Add(wc.Pcode + " | " + p.P_Name);
                    }
                }
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Supply - توريد")
            {
                Product product = WarehouseEnt.Products.Find(int.Parse(comboBox2.Text.Split('|')[0]));
                Supplier supplier = WarehouseEnt.Suppliers.Find(product.Supplier_ID);

                textBox5.Text = supplier.Supplier_ID.ToString() + "   |  " + supplier.Supplier_Name;
                textBox5.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = textBox5.Text = comboBox2.Text = comboBox3.Text = string.Empty;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Permission_Product PermProduct = new Permission_Product();
                Warehouse_Contains WarehouseContains = new Warehouse_Contains();
                string ProductSelected = comboBox2.Text.Split('|')[0];
                int warehouseID = int.Parse(comboBox5.Text.Split('|')[0]);

                if (comboBox1.Text == "Supply - توريد")
                {
                    PermProduct.Permission_Number = int.Parse(textBox1.Text);
                    PermProduct.WarehouseContains_ID = warehouseContainsIndex;


                    PermProduct.Quantity = float.Parse(textBox3.Text);
                    permission_Products.Add(PermProduct);


                    string WarehouseSelected = comboBox5.Text.Split('|')[0];
                    WarehouseContains.WarehouseContains_ID = warehouseContainsIndex++;
                    WarehouseContains.Warehouse_ID = int.Parse(WarehouseSelected);
                    WarehouseContains.Pcode = int.Parse(ProductSelected);
                    WarehouseContains.Quantity = float.Parse(textBox3.Text);
                    WarehouseContains.Unit = comboBox3.Text;
                    WarehouseContains.Added_date = dateTimePicker1.Value;
                    WarehouseContains.Permission_Number = int.Parse(textBox1.Text);
                    warehouse_Contains.Add(WarehouseContains);
                    comboBox3.Items.Clear();
                    comboBox3.Text = string.Empty;
                    comboBox2.Text = string.Empty;
                    textBox3.Text = string.Empty;

                }
                else if (comboBox1.Text == "Dispense - صرف")
                {



                }

            }

            catch (FormatException)
            {
                MessageBox.Show("Inputs not in correct format");
            }
        }
}
