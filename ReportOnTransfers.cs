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
    public partial class ReportOnTransfers : Form
    {
        Model1 WarehouseEnt = new Model1();
        List<int> warehouseIDs = new List<int>();
        public ReportOnTransfers()
        {
            InitializeComponent();
        }
        public void RefreshViewList()
        {
            warehouseIDs.Clear();
            button1.Enabled = false;
            comboBox1.Items.Clear();
            comboBox1.Text = string.Empty;
            listView1.View = System.Windows.Forms.View.Details;
            dateTimePicker1.Refresh();
            dateTimePicker2.Refresh();
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            listView1.Clear();
            listView1.Columns.Add("Transfer ID");
            listView1.Columns.Add("Transfered From");
            listView1.Columns.Add("Transfered To");
            listView1.Columns.Add("Product Code");
            listView1.Columns.Add("Product Name");
            listView1.Columns.Add("Transfered Quantity");
            listView1.Columns.Add("Unit");
            for (int i = 0; i < 7; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                comboBox1.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
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

        private void ReportOnTransfers_Load(object sender, EventArgs e)
        {
            RefreshViewList();

        }

        private void button4_Click_1(object sender, EventArgs e)
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

            try 
            {
                foreach (int WID in warehouseIDs)
                {
                    Warehouse warehouse = WarehouseEnt.Warehouses.Find(WID);

                    var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                    var queryWarehouseContainsTransfers = (from wc in warehouseContains
                                                           where wc.Warehouse_ID == WID
                                                           select wc).ToList();
                    foreach (var wContains in queryWarehouseContainsTransfers)
                    {
                        if (wContains.Transfer_ID != null)
                        {
                            int compareFrom = DateTime.Compare(wContains.Added_Date.Value, FromDate);
                            int compareTo = DateTime.Compare(wContains.Added_Date.Value, ToDate);
                            if (compareFrom >= 0 && compareTo <= 0)
                            {
                                Transfer transfer = WarehouseEnt.Transfers.Find(wContains.Transfer_ID);
                                Warehouse warehouseFrom = WarehouseEnt.Warehouses.Find(transfer.From_ID);
                                Warehouse warehouseTo = WarehouseEnt.Warehouses.Find(transfer.To_ID);
                                
                                Product product = WarehouseEnt.Products.Find(wContains.Pcode);

                                string[] WRow = { transfer.Transfer_ID.ToString(), transfer.From_ID.ToString() + "  |  " + warehouseFrom.Warehouse_Name, transfer.To_ID.ToString() + "  |  " + warehouseTo.Warehouse_Name, product.Pcode.ToString(), product.P_Name, transfer.Quantity.ToString(), wContains.Unit };
                                var listViewItemWarehouse = new ListViewItem(WRow);
                                listView1.Items.Add(listViewItemWarehouse);
                                for (int i = 0; i < 7; i++)
                                {
                                    listView1.Columns[i].Width = -2;
                                }
                                exists = true;
                            }
                        }
                    }
                    if (exists == false)
                    {
                        string[] WRow = { "---", "---", "---", "---", "---", "---", "---" };
                        var listViewItemWarehouse = new ListViewItem(WRow);
                        listView1.Items.Add(listViewItemWarehouse);
                        for (int i = 0; i < 7; i++)
                        {
                            listView1.Columns[i].Width = -2;
                        }


                    }
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm   = new MainMenuForm();
                mainMenuForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshViewList();
        }
    }
}
