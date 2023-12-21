using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DA_Project
{
    public partial class PermissionForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        List<Permission_Product> permission_Products = new List<Permission_Product>();
        List<Warehouse_Contains> warehouse_Contains = new List<Warehouse_Contains>();
        List<Warehouse_Dispense> warehouse_Dispenses = new List<Warehouse_Dispense>();
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
            permission_Products.Clear();
            warehouse_Contains.Clear();
            warehouse_Dispenses.Clear();
            textBox1.Text = textBox3.Text = textBox5.Text = string.Empty;
            comboBox1.Text = comboBox2.Text = comboBox3.Text = comboBox4.Text = comboBox5.Text = string.Empty;
            dateTimePicker1.Refresh();
            comboBox1.Enabled = true;
            comboBox5.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox2.Enabled = true;
            textBox1.Enabled = true;
            comboBox5.Items.Clear();
            comboBox4.Items.Clear();
            
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
            listView1.Columns.Add("Date of Permission");

            for (int i = 0; i < 12; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            listView2.View = System.Windows.Forms.View.Details;
            listView2.Clear();

            listView2.Columns.Add("Permission Number");
            listView2.Columns.Add("Warehouse ID");
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
            foreach (Customer c in WarehouseEnt.Customers)
            {
                comboBox4.Items.Add(c.Customer_ID + " | " + c.Customer_Name);
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

                        string[] WRow = { perm.Permission_Number.ToString(), wc.Pcode.ToString(), p.P_Name, wc.Unit, wc.Quantity.ToString(), p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, wc.Warehouse_ID.ToString(), w.Warehouse_Name, wc.Added_Date.Value.ToString("yyyy-MM-dd") };
                        var listViewItemWarehouse = new ListViewItem(WRow);
                        listView1.Items.Add(listViewItemWarehouse);
                        for (int i = 0; i < 12; i++)
                        {
                            listView1.Columns[i].Width = -2;
                        }

                    }
                }
                foreach (Warehouse_Dispense wd in WarehouseEnt.Warehouse_Dispense.Where(wd => wd.Permission_Number == perm.Permission_Number))
                {
                    Customer c = WarehouseEnt.Customers.Find(wd.Customer_ID);
                    Warehouse w = WarehouseEnt.Warehouses.Find(wd.Warehouse_ID);

                    Product p = WarehouseEnt.Products.Find(wd.Pcode);
                    var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                    var queryWarehouseContains = (from w1 in warehouseContains
                                                  where w1.Warehouse_ID == wd.Warehouse_ID
                                                  && w1.Pcode == wd.Pcode
                                                  select w1).ToList();
                    foreach (var item in queryWarehouseContains)
                    {
                        if (perm.Type_of_Permission == "Dispense - صرف")
                        {
                            string[] WRow = { perm.Permission_Number.ToString(), wd.Warehouse_ID.ToString(), p.Pcode.ToString(), p.P_Name, item.Unit, (wd.Old_Quantity-wd.New_Quantity).ToString(), c.Customer_ID.ToString(), c.Customer_Name, wd.Warehouse_ID.ToString(), w.Warehouse_Name, wd.New_Date.Value.ToString("yyyy-MM-dd") };
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


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (comboBox1.Text == "Supply - توريد")
            {
                RefreshListView();
                comboBox4.Visible = false;
                label13.Visible = false;
                label8.Visible = true;
                textBox5.Visible = true;
            }
             if (comboBox1.Text == "Dispense - صرف")
            {
                RefreshListView();
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
                    foreach (Product p in WarehouseEnt.Products.Where(p => p.Pcode == wc.Pcode))
                    {
                        comboBox2.Items.Add(wc.Pcode + " | " + p.P_Name);
                    }
                }
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            try
            {
               string ProductSelected = comboBox2.Text.Split('|')[0];
            comboBox3.Items.Clear();
            Product product = WarehouseEnt.Products.Find(int.Parse(ProductSelected));
               
            int WarehouseSelected = int.Parse(comboBox5.Text.Split('|')[0]);

             int PermissionNumber = int.Parse(textBox1.Text);
                if (comboBox1.Text == "Supply - توريد")
                {
                    Supplier supplier = WarehouseEnt.Suppliers.Find(product.Supplier_ID);

                    textBox5.Text = supplier.Supplier_ID.ToString() + "   |  " + supplier.Supplier_Name;
                    textBox5.Enabled = false;
                }


                if (product != null)
                {

                    var productUnits = WarehouseEnt.ProductUnits.AsEnumerable();

                    var queryProdUnits = (from pu in productUnits
                                          where pu.Pcode == int.Parse(ProductSelected)
                                          select pu).ToList();
                    //Warehouse_Contains wCGetUnit = WarehouseEnt.Warehouse_Contains.Find(WarehouseSelected, int.Parse(ProductSelected));
                   foreach (var pu in queryProdUnits)
                        
                    {
                        if (comboBox1.Text == "Dispense - صرف") {
                            Warehouse_Contains wCGetUnit = WarehouseEnt.Warehouse_Contains.Find(WarehouseSelected, int.Parse(ProductSelected));

                            if (wCGetUnit.Unit == pu.Unit )
                        {

                                comboBox3.Items.Add(pu.Unit);
                                comboBox3.Text = pu.Unit;
                                comboBox3.Enabled = false;
                                break;
                            } }
                        else
                        {
                            comboBox3.Items.Add(pu.Unit);

                        }
                    }

                }

                
                var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                var queryWarehouseContains = (from wc in warehouseContains
                                              where wc.Warehouse_ID == WarehouseSelected
                                              && wc.Pcode == int.Parse(ProductSelected)
                                              && wc.Permission_Number == PermissionNumber
                                              select wc).ToList();
                
                foreach (var wc in queryWarehouseContains)
                {
                    
                    if (wc != null)
                    {
                        button1.Enabled = false;
                    }
                }
            }
            catch { MessageBox.Show("Input Permission Number first"); comboBox2.Text = string.Empty; }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = textBox5.Text = comboBox2.Text = comboBox3.Text = string.Empty;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Permission permission = new Permission();

            if (textBox1.Text != string.Empty && !(string.IsNullOrEmpty(comboBox5.Text)) && dateTimePicker1.Value.ToString() != string.Empty)
            {
                try
                {

                    if (comboBox1.Text == "Supply - توريد")
                    {
                        for (int i = 0; i < permission_Products.Count; i++)
                        {
                            Permission_Product PermProduct = new Permission_Product();
                            Warehouse_Contains WarehouseContains = new Warehouse_Contains();
                            try
                            {
                                PermProduct.Permission_Number = permission_Products[i].Permission_Number;
                                PermProduct.Warehouse_ID = permission_Products[i].Warehouse_ID;
                                PermProduct.Quantity = permission_Products[i].Quantity;
                                PermProduct.Pcode = permission_Products[i].Pcode;
                            
                           
                                WarehouseContains.Warehouse_ID = warehouse_Contains[i].Warehouse_ID;
                                WarehouseContains.Pcode = warehouse_Contains[i].Pcode;
                                WarehouseContains.Quantity = warehouse_Contains[i].Quantity;
                                WarehouseContains.Unit = warehouse_Contains[i].Unit;
                                WarehouseContains.Added_Date = warehouse_Contains[i].Added_Date;
                                WarehouseContains.Permission_Number = warehouse_Contains[i].Permission_Number;



                                permission.Permission_Number = int.Parse(textBox1.Text);
                                permission.Permission_Date = dateTimePicker1.Value;
                                permission.Type_of_Permission = comboBox1.Text;
                                WarehouseEnt.Permissions.Add(permission);
                                WarehouseEnt.Warehouse_Contains.Add(WarehouseContains);
                                WarehouseEnt.Permission_Product.Add(PermProduct);
                                WarehouseEnt.SaveChanges();
                                RefreshListView();        
                            MessageBox.Show("Permission Added");

                            }
                            catch
                            {
                                MessageBox.Show("This product already exists in warehouse => Update values");
                            }

                        }
                    }
                    else if (comboBox1.Text == "Dispense - صرف")
                    {
                       
                        for (int i = 0; i < permission_Products.Count; i++)
                        {
                            Permission_Product PermProduct = new Permission_Product();
                            Warehouse_Dispense warehouse_Dispense = new Warehouse_Dispense();

                             int compareDate = DateTime.Compare(warehouse_Dispenses[i].New_Date.Value.Date, warehouse_Dispenses[i].Old_Date.Value.Date);
                            MessageBox.Show(compareDate.ToString());
                            try
                            {
                                if (compareDate >= 0)
                                {        
                                    
                                        PermProduct.Permission_Number = permission_Products[i].Permission_Number;
                                        PermProduct.Warehouse_ID = permission_Products[i].Warehouse_ID;
                                        PermProduct.Quantity = permission_Products[i].Quantity;
                                        PermProduct.Pcode = permission_Products[i].Pcode;

                                   
                                        warehouse_Dispense.Warehouse_ID = warehouse_Dispenses[i].Warehouse_ID;
                                        warehouse_Dispense.Pcode = warehouse_Dispenses[i].Pcode;
                                        warehouse_Dispense.Permission_Number = warehouse_Dispenses[i].Permission_Number;
                                        warehouse_Dispense.Old_Date = warehouse_Dispenses[i].Old_Date;
                                        warehouse_Dispense.Old_Quantity = warehouse_Dispenses[i].Old_Quantity;
                                        warehouse_Dispense.New_Date = dateTimePicker1.Value;
                                        warehouse_Dispense.New_Quantity = warehouse_Dispenses[i].New_Quantity;

                                        warehouse_Dispense.Customer_ID = warehouse_Dispenses[i].Customer_ID;

                                        permission.Permission_Number = int.Parse(textBox1.Text);
                                        permission.Permission_Date = dateTimePicker1.Value;
                                        permission.Type_of_Permission = comboBox1.Text;
                                        WarehouseEnt.Permissions.Add(permission);
                                        WarehouseEnt.Warehouse_Dispense.Add(warehouse_Dispense);
                                        WarehouseEnt.Permission_Product.Add(PermProduct);


                                        WarehouseEnt.SaveChanges();
                                        RefreshListView();
                                    
                                   
                                }
                                else
                                {
                                    MessageBox.Show($"Choose date after {warehouse_Dispense.Old_Date} to dispense product");
                                }
                            }
                            catch
                            {
                                MessageBox.Show("This product already exists in warehouse => Update values");

                            }
                        }

                       
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Inputs not in correct format");
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

            float TotalQuantity = 0;
            try
            {
                Permission_Product PermProduct = new Permission_Product();
                string ProductSelected = comboBox2.Text.Split('|')[0];
                string WarehouseSelected = comboBox5.Text.Split('|')[0];
                float InputQuantity = float.Parse(textBox3.Text);
                if (comboBox1.Text == "Supply - توريد")
                {
                    Warehouse_Contains WarehouseContains = new Warehouse_Contains();

                    PermProduct.Permission_Number = int.Parse(textBox1.Text);
                    PermProduct.Warehouse_ID = int.Parse(WarehouseSelected);
                    PermProduct.Pcode = int.Parse(ProductSelected);
                    PermProduct.Quantity = InputQuantity;
                    permission_Products.Add(PermProduct);



                    WarehouseContains.Warehouse_ID = warehouseContainsIndex++;
                    WarehouseContains.Warehouse_ID = int.Parse(WarehouseSelected);
                    WarehouseContains.Pcode = int.Parse(ProductSelected);
                    WarehouseContains.Quantity = InputQuantity;
                    WarehouseContains.Unit = comboBox3.Text;
                    WarehouseContains.Added_Date = dateTimePicker1.Value;
                    WarehouseContains.Permission_Number = int.Parse(textBox1.Text);
                    warehouse_Contains.Add(WarehouseContains);
                    comboBox3.Items.Clear();
                    comboBox3.Text = string.Empty;
                    comboBox2.Text = string.Empty;
                    textBox3.Text = string.Empty;

                }
                else if (comboBox1.Text == "Dispense - صرف")
                {

                    var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                    var queryWarehouseContains = (from wc in warehouseContains
                                                  where wc.Warehouse_ID == int.Parse(WarehouseSelected)
                                                  && wc.Pcode == int.Parse(ProductSelected)
                                                  && wc.Unit == comboBox3.Text
                                                  select wc).ToList();
                    foreach (var item in queryWarehouseContains)
                    {
                        button1.Enabled = true;
                        if (item.Dispensed_Flag == 1)
                        {
                            var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                            var queryWarehouseDispense = (from wd in warehouseDispense
                                                          where wd.Warehouse_ID == int.Parse(WarehouseSelected)
                                                          && wd.Pcode == int.Parse(ProductSelected)
                                                          select wd).ToList();
                            foreach (var wDispense in queryWarehouseDispense)
                            {
                                int compareDate = DateTime.Compare(wDispense.New_Date.Value.Date, wDispense.Old_Date.Value.Date);
                                if (compareDate >= 0)
                                {
                                    TotalQuantity += (float)wDispense.New_Quantity;
                                }
                                else
                                {
                                    TotalQuantity += (float)item.Quantity;
                                }
                            }


                        }
                    }
                    if (int.Parse(textBox3.Text) > TotalQuantity)
                    {
                        MessageBox.Show("There is not enough stock in the warehouse");

                    }
                    else

                    {
                        foreach (var item in queryWarehouseContains)
                        {
                            if (item.Transfer_ID == null)
                            {
                                if (InputQuantity <= item.Quantity)
                                {

                                    PermProduct.Permission_Number = int.Parse(textBox1.Text);
                                    PermProduct.Warehouse_ID = int.Parse(WarehouseSelected);
                                    PermProduct.Pcode = int.Parse(ProductSelected);
                                    PermProduct.Quantity = InputQuantity;
                                    permission_Products.Add(PermProduct);


                                    Warehouse_Dispense warehouse_Dispense = new Warehouse_Dispense();
                                    warehouse_Dispense.Warehouse_ID = int.Parse(WarehouseSelected);
                                    warehouse_Dispense.Pcode = int.Parse(ProductSelected);
                                    warehouse_Dispense.Permission_Number = int.Parse(textBox1.Text);
                                    warehouse_Dispense.Customer_ID = int.Parse(comboBox4.Text.Split('|')[0]);
                                    warehouse_Dispense.Old_Quantity = item.Quantity;
                                    warehouse_Dispense.Old_Date = item.Added_Date.Value;
                                    warehouse_Dispense.New_Quantity = item.Quantity - InputQuantity;
                                    warehouse_Dispense.New_Date = dateTimePicker1.Value;
                                    MessageBox.Show(warehouse_Dispense.New_Quantity.ToString());
                                    warehouse_Dispenses.Add(warehouse_Dispense);
                                    comboBox3.Items.Clear();
                                    comboBox3.Text = string.Empty;
                                    comboBox2.Text = string.Empty;
                                    textBox3.Text = string.Empty;

                                    item.Dispensed_Flag = 1;

                                    WarehouseEnt.SaveChanges();

                                }
                            }


                        }
                    }

                }
            }

            catch (FormatException)
            {
                MessageBox.Show("Inputs not in correct format");
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            try
            {

                Permission permission = WarehouseEnt.Permissions.Find(int.Parse(textBox1.Text));
                
                if (permission != null)
                {
                    MessageBox.Show("Permission Exists\nTo update permission,select the Warehouse and the product you wish to change in the permission");
                    comboBox1.Text = permission.Type_of_Permission;
                    comboBox1.Enabled = false;
                    comboBox5.Enabled = false;
                    
                    Warehouse_Contains wContains = (from wc in WarehouseEnt.Warehouse_Contains
                                                    where wc.Permission_Number == permission.Permission_Number
                                                    select wc).FirstOrDefault();

                    Warehouse_Dispense wDispense = (from wd in WarehouseEnt.Warehouse_Dispense
                                                    where wd.Permission_Number == permission.Permission_Number
                                                    select wd).FirstOrDefault();

                    Warehouse warehouse = WarehouseEnt.Warehouses.Find(wDispense.Warehouse_ID);
                    comboBox5.Text = wDispense.Warehouse_ID.ToString() + "  |  "+warehouse.Warehouse_Name;
                    comboBox2.Items.Clear();
                    comboBox4.Items.Clear();
                    if (comboBox1.Text == "Supply - توريد")
                    {
                        foreach (Product p in WarehouseEnt.Products.Where(p => p.Pcode == wContains.Pcode))
                        {
                            comboBox2.Items.Add(p.Pcode + " | " + p.P_Name);
                        }
                    }

                   if (comboBox1.Text == "Dispense - صرف")
                    {
                        foreach (Product p in WarehouseEnt.Products.Where(p => p.Pcode == wDispense.Pcode))
                        {
                            comboBox2.Items.Add(p.Pcode + " | " + p.P_Name);
                        }
                        Customer customer = (from c in WarehouseEnt.Customers
                                         where c.Customer_ID == wDispense.Customer_ID
                                        select c).FirstOrDefault();

                        comboBox4.Items.Add(customer.Customer_ID + " | " + customer.Customer_Name);

                        comboBox4.Enabled = false;

                        comboBox3.Enabled = false;
                    }

                }
            }
            catch 
            {
                MessageBox.Show("Inputs not in valid format");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                Permission permission = WarehouseEnt.Permissions.Find(int.Parse(textBox1.Text));
                Permission_Product PermProduct = new Permission_Product();

                string ProductSelected = comboBox2.Text.Split('|')[0];
                string WarehouseSelected = comboBox5.Text.Split('|')[0];
                if (string.IsNullOrEmpty(comboBox5.Text))
                {
                    MessageBox.Show("warehouse must be selected in order to update values");
                    button2.Enabled = false;

                }
                        if (string.IsNullOrEmpty(comboBox5.Text))
                        {
                            button2.Enabled = false;

                            MessageBox.Show("Product must be selected in order to update vlaues");

                        } 
                        else
                        {

                            comboBox1.Enabled = false;
                            comboBox2.Enabled = false;
                            comboBox5.Enabled = false;
                            textBox1.Enabled = false;
                            button2.Enabled = true;
                            if (permission.Type_of_Permission == "Supply - توريد")
                            {
                                var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                                var queryWarehouseContains = (from wc in warehouseContains
                                                              where wc.Warehouse_ID == int.Parse(WarehouseSelected)
                                                              && wc.Pcode == int.Parse(ProductSelected)
                                                              && wc.Permission_Number == permission.Permission_Number
                                                              select wc).ToList();

                                foreach (var warehouse_Contains in queryWarehouseContains)
                                {
                                    if (warehouse_Contains != null)
                                    {
                                        if (dateTimePicker1.Value.ToString() != string.Empty)
                                         {
                                    if (warehouse_Contains.Dispensed_Flag == 1)
                                    {
                                        var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                                        var queryWarehouseDispense = (from wd in warehouseDispense
                                                                      where wd.Warehouse_ID == warehouse_Contains.Warehouse_ID
                                                                      && wd.Pcode == warehouse_Contains.Pcode
                                                                      select wd).ToList();
                                        foreach (var warehouse_Dispense in queryWarehouseDispense)
                                        {

                                            int compare = DateTime.Compare(warehouse_Contains.Added_Date.Value, dateTimePicker1.Value.Date);
                                            if (compare >= 0)
                                            {
                                                permission.Permission_Date = dateTimePicker1.Value;

                                                warehouse_Contains.Added_Date = dateTimePicker1.Value;
                                            }
                                            else
                                            {
                                                MessageBox.Show($"Product is Dispensed on {warehouse_Dispense.New_Date.Value.ToString("yyyy-MM-dd")}\nselect a date before dispense date");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        permission.Permission_Date = dateTimePicker1.Value;

                                        warehouse_Contains.Added_Date = dateTimePicker1.Value;
                                    }

                                        }

                                        if (textBox3.Text != string.Empty)
                                        {
                                            warehouse_Contains.Quantity = float.Parse(textBox3.Text);
                                            PermProduct.Quantity = float.Parse(textBox3.Text);
                                        }
                                        if (!string.IsNullOrEmpty(comboBox3.Text))
                                        {
                                            warehouse_Contains.Unit = comboBox3.Text;
                                        }


                                        WarehouseEnt.SaveChanges();
                                        RefreshListView();

                                    }
                                }

                                }

                            else if (comboBox1.Text == "Dispense - صرف")
                            {
                        var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();

                        var queryWarehouseDispese = (from wd in warehouseDispense
                                                      where wd.Warehouse_ID == int.Parse(WarehouseSelected)
                                                      && wd.Pcode == int.Parse(ProductSelected)
                                                      && wd.Permission_Number == permission.Permission_Number
                                                      select wd).ToList();
                        foreach (var warehouse_Dispense in queryWarehouseDispese)
                        {
                            if (warehouse_Dispense != null)
                            {

                                if (dateTimePicker1.Value.ToString() != string.Empty)
                                {

                                   

                                    int compareOld= DateTime.Compare(warehouse_Dispense.Old_Date.Value, dateTimePicker1.Value.Date);
                                    int compareNew = DateTime.Compare(warehouse_Dispense.New_Date.Value, dateTimePicker1.Value.Date);

                                    if (compareNew>=0 && compareOld<=0)
                                    {
                                        permission.Permission_Date = dateTimePicker1.Value;

                                        warehouse_Dispense.New_Date = dateTimePicker1.Value;
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Product is Supplied to warehouse on {warehouse_Dispense.Old_Date.Value.ToString("yyyy-MM-dd")}\nselect a date after supply date");
                                    }

                                }

                                if (textBox3.Text != string.Empty)
                                {
                                    float inputQuanitity = float.Parse(textBox3.Text);
                                    if(inputQuanitity > warehouse_Dispense.Old_Quantity)
                                    {
                                        button2.Enabled = false;
                                        MessageBox.Show("There is not enough stock in the warehouse");
                                    }
                                    else
                                    {
                                        button2.Enabled = true;

                                        warehouse_Dispense.Old_Quantity += warehouse_Dispense.New_Quantity - inputQuanitity;
                                        warehouse_Dispense.New_Quantity = inputQuanitity;
                                    }
                                    
                                }

                                WarehouseEnt.SaveChanges();
                                RefreshListView();

                            }
                        }
                    }

                }

                }
            
            catch 
            {
                MessageBox.Show("Inputs not in correct format");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm   = new MainMenuForm();
            mainMenuForm.ShowDialog();
        }
    }
}