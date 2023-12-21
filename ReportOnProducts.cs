using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_Project
{
    public partial class ReportOnProducts : Form
    {
        Model1 WarehouseEnt = new Model1();
        List<int> warehouseIDs = new List<int>();
        public ReportOnProducts()
        {
            InitializeComponent();
        }

        public void RefreshViewList()
        {
            warehouseIDs.Clear();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            button1.Enabled = false;
            dateTimePicker1.Refresh();
            dateTimePicker2.Refresh();

            comboBox1.Text = string.Empty;
            listView1.View = System.Windows.Forms.View.Details;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            listView1.Clear();
            listView1.Columns.Add("Warehouse ID");
            listView1.Columns.Add("Warehouse Name");
            listView1.Columns.Add("Production Date");
            listView1.Columns.Add("Expiration Date");
            listView1.Columns.Add("Date Added");
            listView1.Columns.Add("Quantity");

            listView1.Columns.Add("Unit");
            
            listView1.Columns.Add("Updated At");
            listView1.Columns.Add("Updated To"); 

            for (int i = 0; i < 9; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                comboBox1.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
            }
            foreach (Product p in WarehouseEnt.Products)
            {
                comboBox2.Items.Add(p.Pcode + " | " + p.P_Name);
            }
        }

        private void ReportOnProducts_Load(object sender, EventArgs e)
        {
            RefreshViewList();
        }
        public void DisplayRow(string[] WRow)
        {
            var listViewItemWarehouse = new ListViewItem(WRow);
            listView1.Items.Add(listViewItemWarehouse);
            for (int i = 0; i < 9; i++)
            {
                listView1.Columns[i].Width = -2;
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if (comboBox1.Text != string.Empty)
            {
                warehouseIDs.Add(int.Parse(comboBox1.Text.Split('|')[0]));
            }
            comboBox1.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime FromDate = dateTimePicker1.Value;
            DateTime ToDate = dateTimePicker2.Value;
            bool exists = false;
            string[] WRow;

            List<Warehouse_Contains> list = new List<Warehouse_Contains>();

            try
            {

                Product product = WarehouseEnt.Products.Find(int.Parse(comboBox2.Text.Split('|')[0]));
                foreach (int WID in warehouseIDs)
                {
                    Warehouse warehouse = WarehouseEnt.Warehouses.Find(WID);

                    var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                    var queryWarehouseContains = (from wc in warehouseContains
                                                           where wc.Warehouse_ID == WID
                                                           select wc).ToList();
             
                    foreach (var wContains in queryWarehouseContains)
                    {
                        if(wContains.Dispensed_Flag == 1 && wContains.Pcode == product.Pcode && wContains.Transfer_ID == null)
                        {
                            var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                            var queryWarehouseDispense = (from wd in warehouseDispense
                                                          where wd.Warehouse_ID == wContains.Warehouse_ID
                                                          && wd.Pcode == wContains.Pcode
                                                          select wd).ToList();
                            foreach (var wDispense in queryWarehouseDispense)
                            {
                                Product p = WarehouseEnt.Products.Find(wDispense.Pcode);
                                Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                int compareDispensedOnSupplyDate = DateTime.Compare(wDispense.Old_Date.Value, wDispense.New_Date.Value);
                                int compareFromOld = DateTime.Compare(wDispense.Old_Date.Value, FromDate.Date);
                                int compareToOld = DateTime.Compare(wDispense.Old_Date.Value, ToDate.Date);

                                int compareFromNew = DateTime.Compare(wDispense.New_Date.Value, FromDate.Date);
                                int compareToNew = DateTime.Compare(wDispense.New_Date.Value, ToDate.Date);
                                bool updated = false;


                                if (compareFromOld >= 0 && compareToOld <= 0 && compareFromNew >= 0 && compareToNew <= 0)
                                {

                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"),   (wDispense.Old_Quantity).ToString(),  wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), wContains.Unit,wDispense.New_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Quantity.ToString() };

                                    DisplayRow(WRow);
                                    exists = true;
                                    updated = true;
                                }
                                if (compareFromOld >= 0 && compareToOld <= 0 && compareDispensedOnSupplyDate == -1 && updated==false)
                                {
                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), (wDispense.Old_Quantity).ToString(), wContains.Unit, "---","---" };
                                    DisplayRow(WRow);
                                    exists = true;
                                }
                                else if (compareFromNew >= 0 && compareToNew <= 0 || compareDispensedOnSupplyDate >= 0 && updated==false)
                                {

                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), wDispense.New_Date.Value.ToString("yyyy-MM-dd"), (wDispense.New_Quantity).ToString(), wContains.Unit , "---", "---" };
                                    DisplayRow(WRow);
                                    exists = true;
                                }

                            }

                        }
                       if (wContains.Dispensed_Flag == null && wContains.Pcode == product.Pcode && wContains.Transfer_ID == null)
                        {

                            if (product.Pcode == wContains.Pcode) { 
                            
                            DateTime currentDate = DateTime.Now;

                            int compareProductInWarehouseFrom = DateTime.Compare(wContains.Added_Date.Value, FromDate.Date);
                            int compareProductInWarehouseTo = DateTime.Compare(wContains.Added_Date.Value, ToDate.Date);

                                if (compareProductInWarehouseFrom >= 0 && compareProductInWarehouseTo <= 0)
                                {
                                    Product p = WarehouseEnt.Products.Find(wContains.Pcode);
                                    Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                    WRow = new string[]{ warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), wContains.Added_Date.Value.ToString("yyyy-MM-dd"), (wContains.Quantity).ToString(), wContains.Unit, "---", "---" };
                                    DisplayRow(WRow);
                                    exists = true;

                                }
                            }
                        }
                        if (wContains.Transfer_ID != null && wContains.Pcode == product.Pcode)
                        {
                            MessageBox.Show(wContains.Warehouse_ID.ToString());
                            var transfers = WarehouseEnt.Transfer_From.AsEnumerable();
                            var queryTransfers = (from t in transfers
                                                  where t.Warehouse_ID == wContains.Warehouse_ID
                                                  && t.Transfer_ID == wContains.Transfer_ID
                                                  select t).ToList();
                            foreach (var transfer in queryTransfers)
                            {
                                Product p = WarehouseEnt.Products.Find(transfer.Pcode);
                                Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                int compareDispensedOnSupplyDate = DateTime.Compare(transfer.Old_Date.Value, transfer.New_Date.Value);
                                int compareFromOld = DateTime.Compare(transfer.Old_Date.Value, FromDate.Date);
                                int compareToOld = DateTime.Compare(transfer.Old_Date.Value, ToDate.Date);

                                int compareFromNew = DateTime.Compare(transfer.New_Date.Value, FromDate.Date);
                                int compareToNew = DateTime.Compare(transfer.New_Date.Value, ToDate.Date);
                                bool updated = false;

                                if (compareFromOld >= 0 && compareToOld <= 0 && compareFromNew >= 0 && compareToNew <= 0)
                                {

                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"),  (transfer.Old_Quantity).ToString(), wContains.Unit, transfer.Old_Date.Value.ToString("yyyy-MM-dd"), transfer.New_Date.Value.ToString("yyyy-MM-dd"), transfer.New_Quantity.ToString() };

                                    var listViewItemWarehouse = new ListViewItem(WRow);
                                    DisplayRow(WRow);
                                    exists = true;
                                    updated = true;
                                }
                                else if (compareFromOld >= 0 && compareToOld <= 0 && compareDispensedOnSupplyDate != 0 && updated == false)
                                {
                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), (transfer.Old_Quantity).ToString(), wContains.Unit, transfer.Old_Date.Value.ToString("yyyy-MM-dd"), "---", "---" };
                                    var listViewItemWarehouse = new ListViewItem(WRow);
                                    DisplayRow(WRow);
                                    exists = true;
                                }

                                else if (compareFromNew >= 0 && compareToNew <= 0 || compareDispensedOnSupplyDate == 0 && updated == false)
                                {
                                    WRow = new string[] { warehouse.Warehouse_ID.ToString(), warehouse.Warehouse_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"),  (transfer.New_Quantity).ToString(), wContains.Unit, transfer.New_Date.Value.ToString("yyyy-MM-dd"), "---", "---" };
                                    var listViewItemWarehouse = new ListViewItem(WRow);
                                    DisplayRow(WRow);
                                    exists = true;
                                }


                            }
                        }

                    }
                    if (exists == false)
                    {
                       WRow = new string[] { "---", "---", "---", "---", "---", "---", "---" ,"---", "---"};
                        var listViewItemWarehouse = new ListViewItem(WRow);
                        listView1.Items.Add(listViewItemWarehouse);
                        for (int i = 0; i < 11; i++)
                        {
                            listView1.Columns[i].Width = -2;
                        }


                    }

                }
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshViewList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm = new MainMenuForm(); 
            mainMenuForm.ShowDialog();
        }
    }
}