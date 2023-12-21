using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_Project
{
    public partial class ReportOnWarehouse : Form
    {
        Model1 WarehouseEnt = new Model1();

        public ReportOnWarehouse()
        {
            InitializeComponent();
        }
        public void RefreshViewList()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = string.Empty;
            listView1.View = System.Windows.Forms.View.Details;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            dateTimePicker2.Refresh();
            dateTimePicker1.Refresh();
            listView1.Clear();
            listView1.Columns.Add("Product Code");
            listView1.Columns.Add("Product Name");
            listView1.Columns.Add("Production Date");
            listView1.Columns.Add("Expiration Date");
            listView1.Columns.Add("Supplier ID");
            listView1.Columns.Add("Supplier Name");
            listView1.Columns.Add("Quantity");
            listView1.Columns.Add("Unit");
            
            listView1.Columns.Add("Date Added");
            listView1.Columns.Add("Updated At");
            listView1.Columns.Add("Updated To Quantity");

            for (int i = 0; i < 11; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                comboBox1.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
            }

        }

        private void ReportOnWarehouse_Load(object sender, EventArgs e)
        {
            RefreshViewList();
        }
        public void DisplayRow(string[] WRow)
        {
            var listViewItemWarehouse = new ListViewItem(WRow);
            listView1.Items.Add(listViewItemWarehouse);
            for (int i = 0; i < 11; i++)
            {
                listView1.Columns[i].Width = -2;
            }
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(groupBox1.Enabled && groupBox2.Enabled)
            {
                MessageBox.Show("Select either time range or time period to generate report");
            }
            List<Warehouse_Contains> warehouse_Contains = new List<Warehouse_Contains>();
            if (groupBox1.Enabled && !groupBox2.Enabled)
            {
                DateTime FromDate = dateTimePicker1.Value;
                DateTime ToDate = dateTimePicker2.Value;

                bool exists = false;
                try
                {

                    int warehouseID = int.Parse(comboBox1.Text.Split('|')[0]);
                    if (comboBox1.Text != string.Empty)
                    {
                        Warehouse warehouseSelected = WarehouseEnt.Warehouses.Find(warehouseID);
                        var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                        var queryProductsInWarehouse = (from wc in warehouseContains
                                                        where wc.Warehouse_ID == warehouseID
                                                        select wc).ToList();
                        foreach (var item in queryProductsInWarehouse)
                        {
                            warehouse_Contains.Add(item);

                        }

                        string[] WRow;
                        foreach (var item in warehouse_Contains)
                        {
                            if (item.Transfer_ID == null)
                            {
                                var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                                var queryWarehouseDispense = (from wd in warehouseDispense
                                                              where wd.Warehouse_ID == item.Warehouse_ID
                                                              && wd.Pcode == item.Pcode
                                                              select wd).ToList();
                                if (item.Dispensed_Flag == 1)
                                {
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

                                            WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.Old_Quantity).ToString(), item.Unit, wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Quantity.ToString() };
                                            DisplayRow(WRow);
                                          
                                            exists = true;
                                            updated = true;
                                        }
                                        else if (compareFromOld >= 0 && compareToOld <= 0 && compareDispensedOnSupplyDate != 0 && updated==false)
                                        {
                                            WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.Old_Quantity).ToString(), item.Unit, wDispense.Old_Date.Value.ToString("yyyy-MM-dd"),"---","---" };
                                          
                                            DisplayRow(WRow);

                                            exists = true;
                                        }
                     
                                        else if (compareFromNew >= 0 && compareToNew <= 0 || compareDispensedOnSupplyDate == 0 && updated==false)
                                        {
                                            WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.New_Quantity).ToString(), item.Unit, wDispense.New_Date.Value.ToString("yyyy-MM-dd"),"---","---" };
                                          
                                            exists = true;
                                            DisplayRow(WRow);

                                        }


                                    }
                                }
                            }
                            if(item.Dispensed_Flag == null && item.Transfer_ID ==null)
                            {
                                int compareFromItem = DateTime.Compare(item.Added_Date.Value, FromDate.Date);
                                int compareToItem = DateTime.Compare(item.Added_Date.Value, ToDate.Date);
                                if (compareFromItem >= 0 && compareToItem <= 0)
                                {
                                    Product p = WarehouseEnt.Products.Find(item.Pcode);
                                    Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                    WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, item.Quantity.ToString(), item.Unit, item.Added_Date.Value.ToString("yyyy-MM-dd") ,"---","---"};
                                   
                                    exists = true;
                                    DisplayRow(WRow);

                                }
                            }
                            if(item.Transfer_ID != null)
                            {
                                var transfers = WarehouseEnt.Transfer_From.AsEnumerable();
                                var queryTransfers = (from t in transfers
                                                              where t.Warehouse_ID == item.Warehouse_ID
                                                              && t.Transfer_ID == item.Transfer_ID
                                                              select t).ToList();
                                foreach ( var transfer in queryTransfers )
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

                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (transfer.Old_Quantity).ToString(), item.Unit, transfer.Old_Date.Value.ToString("yyyy-MM-dd"), transfer.New_Date.Value.ToString("yyyy-MM-dd"), transfer.New_Quantity.ToString() }; DisplayRow(WRow);
                                        DisplayRow(WRow);

                                     
                                        exists = true;
                                        updated = true;
                                    }
                                    else if (compareFromOld >= 0 && compareToOld <= 0 && compareDispensedOnSupplyDate != 0 && updated == false)
                                    {
                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (transfer.Old_Quantity).ToString(), item.Unit,transfer.Old_Date.Value.ToString("yyyy-MM-dd"), "---", "---" };
                                        var listViewItemWarehouse = new ListViewItem(WRow);
                                    
                                        DisplayRow(WRow);

                                        exists = true;
                                    }

                                    else if (compareFromNew >= 0 && compareToNew <= 0 || compareDispensedOnSupplyDate == 0 && updated == false)
                                    {
                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (transfer.New_Quantity).ToString(), item.Unit, transfer.New_Date.Value.ToString("yyyy-MM-dd"), "---", "---" };
                                        var listViewItemWarehouse = new ListViewItem(WRow);
                                       
                                        DisplayRow(WRow);

                                        exists = true;
                                    }


                                }
                            }


                        }

                        if (exists == false)
                        {

                            WRow = new string[] { "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---" };
                            
                            DisplayRow(WRow);


                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Inputs not in correct format");
                }
            }
            if (groupBox2.Enabled && !groupBox1.Enabled)
            {
                try
                {
                    int TimePeriod = (int)numericUpDown1.Value;
                    bool exists = false;

                    int warehouseID = int.Parse(comboBox1.Text.Split('|')[0]);
                    if (comboBox1.Text != string.Empty)
                    {
                        Warehouse warehouseSelected = WarehouseEnt.Warehouses.Find(warehouseID);
                        var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                        var queryProductsInWarehouse = (from wc in warehouseContains
                                                        where wc.Warehouse_ID == warehouseID
                                                        select wc).ToList();

                        foreach (var item in queryProductsInWarehouse)
                        {

                            if (item.Dispensed_Flag == null)
                            {
                                DateTime DateAddedAfterTimePeriod = item.Added_Date.Value.AddMonths(TimePeriod);

                                DateTime currentDate = DateTime.Now;
                                int compare = DateTime.Compare(DateAddedAfterTimePeriod, currentDate.Date);
                                int compareProductInWarehouseCurrent = DateTime.Compare(item.Added_Date.Value, currentDate.Date);
                                   if (compare<=0 && compareProductInWarehouseCurrent<=0)
                                {
                                    Product p = WarehouseEnt.Products.Find(item.Pcode);
                                    Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                    string[] WRow = { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (item.Quantity).ToString(), item.Unit, item.Added_Date.Value.ToString("yyyy-MM-dd"),"---","---" };
                                 
                                    DisplayRow(WRow);

                                    exists = true;


                                }
                        }
                            else if (item.Dispensed_Flag == 1 && item.Transfer_ID==null)
                        {

                            var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                                var queryWarehouseDispense = (from wd in warehouseDispense
                                                              where wd.Warehouse_ID == item.Warehouse_ID
                                                              && wd.Pcode == item.Pcode
                                                              select wd).ToList();


                                foreach (var wDispense in queryWarehouseDispense)
                                {
                                    DateTime DateAddedAfterTimePeriodOld = wDispense.Old_Date.Value.AddMonths(TimePeriod);
                                    DateTime DateAddedAfterTimePeriodNew = wDispense.New_Date.Value.AddMonths(TimePeriod);
                                    
                                    DateTime currentDate = DateTime.Now;
                                    int compareProductInWarehouseCurrentOld = DateTime.Compare(wDispense.Old_Date.Value.Date, currentDate.Date);
                                    int compareProductInWarehouseCurrentNew = DateTime.Compare(wDispense.New_Date.Value.Date, currentDate.Date);
                                   
                                    int compareOldandCurrent = DateTime.Compare(DateAddedAfterTimePeriodOld, currentDate.Date);
                                    int compareNewandCurrent = DateTime.Compare(DateAddedAfterTimePeriodNew, currentDate.Date);
                                    int comprareOldandNew = DateTime.Compare(wDispense.Old_Date.Value, wDispense.New_Date.Value);
                                    Product p = WarehouseEnt.Products.Find(wDispense.Pcode);
                                    Supplier s = WarehouseEnt.Suppliers.Find(p.Supplier_ID);
                                    string[] WRow;
                                    if (compareOldandCurrent <=0 && compareProductInWarehouseCurrentOld<=0 && compareNewandCurrent>0 && comprareOldandNew>0 )
                                    {
                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.Old_Quantity).ToString(), item.Unit, wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), "---", "---" };
                                        DisplayRow(WRow);

                                        exists = true;
                                    }
                                    else if (compareOldandCurrent <= 0 && compareProductInWarehouseCurrentOld <= 0 )
                                    {
                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.Old_Quantity).ToString(), item.Unit, wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Quantity.ToString() };
                                        DisplayRow(WRow);

                                        exists = true;
                                    }
                                    else if (DateTime.Compare(wDispense.New_Date.Value.Date, currentDate.AddMonths(TimePeriod).Date)<=0 && compareOldandCurrent<=0)
                                    {
                                        WRow = new string[] { p.Pcode.ToString(), p.P_Name, p.Production_Date.Value.ToString("yyyy-MM-dd"), p.Expiration_date.Value.ToString("yyyy-MM-dd"), p.Supplier_ID.ToString(), s.Supplier_Name, (wDispense.Old_Quantity).ToString(), item.Unit, wDispense.Old_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Date.Value.ToString("yyyy-MM-dd"), wDispense.New_Quantity.ToString() };
                                        DisplayRow(WRow);

                                        exists = true;
                                    }
                                    

                                }
                            }
                          

                        }
                        if (exists == false)
                        {

                            string [] WRow =  { "---", "---", "---", "---", "---", "---", "---", "---", "---","---","---" };
                         
                            DisplayRow(WRow);


                        }
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
            RefreshViewList();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm   = new MainMenuForm();   
            mainMenuForm.ShowDialog();
        }
    }
}
