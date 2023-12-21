using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DA_Project
{
    public partial class TransferForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        List<Transfer> transfers = new List<Transfer>();
        Warehouse_Contains wContainsOld = new Warehouse_Contains();
        List<Transfer_From> Transfer_Froms =  new  List<Transfer_From>();
        List<Warehouse_Contains> warehouse_ContainsList = new List<Warehouse_Contains>();

        public TransferForm()
        {
            InitializeComponent();
        }

        public void RefreshListView()
        {
            comboBox1.Text = comboBox2.Text  = textBox3.Text = string.Empty;
            dateTimePicker1.Refresh();
            comboBox2.Enabled = false;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            
            listView1.View = View.Details;
            listView1.Clear();
            comboBox1.Items.Clear();
            listView1.Columns.Add("Transfer ID");
            listView1.Columns.Add("From");
            listView1.Columns.Add("To");

            listView1.Columns.Add("Quantity Transferred");
            for (int i = 0; i < 4; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
            foreach (Transfer_From transferFrom in WarehouseEnt.Transfer_From)
            {
                Transfer transfer = WarehouseEnt.Transfers.Find(transferFrom.Transfer_ID);
                Warehouse wF = (from wFrom in WarehouseEnt.Warehouses
                                where wFrom.Warehouse_ID == transfer.From_ID
                                select wFrom).FirstOrDefault();

                Warehouse wT = (from wTo in WarehouseEnt.Warehouses
                                where wTo.Warehouse_ID == transfer.To_ID
                                select wTo).FirstOrDefault();


                string[] WRow = { transfer.Transfer_ID.ToString(), wF.Warehouse_Name, wT.Warehouse_Name, transfer.Quantity.ToString() };
                var listViewItemWarehouse = new ListViewItem(WRow);
                listView1.Items.Add(listViewItemWarehouse);
                for (int i = 0; i < 4; i++)
                {
                    listView1.Columns[i].Width = -2;

                }



            }

            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                comboBox1.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
            }
        }

        private void TransferForm_Load(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm = new MainMenuForm();
                mainMenuForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            comboBox5.Items.Clear();
            comboBox5.Text = string.Empty;
            int warehouseID = int.Parse(comboBox1.Text.Split('|')[0]);
            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                if (w.Warehouse_ID != warehouseID)
                {
                    comboBox2.Items.Add(w.Warehouse_ID + " | " + w.Warehouse_Name);
                }
            }

            var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

            var queryWarehouseContains = (from wc in warehouseContains
                                          where wc.Warehouse_ID == warehouseID
                                          select wc).ToList();


            foreach (var wc in queryWarehouseContains)
            {
                foreach (Product p in WarehouseEnt.Products.Where(p => p.Pcode == wc.Pcode))
                {
                    comboBox5.Items.Add(wc.Pcode + " | " + p.P_Name);
                }
            }


        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ProductSelected = comboBox5.Text.Split('|')[0];
                comboBox6.Items.Clear();
                Product product = WarehouseEnt.Products.Find(int.Parse(ProductSelected));
                int WarehouseSelected = int.Parse(comboBox1.Text.Split('|')[0]);

                if (product != null)
                {

                    var productUnits = WarehouseEnt.ProductUnits.AsEnumerable();

                    var queryProdUnits = (from pu in productUnits
                                          where pu.Pcode == int.Parse(ProductSelected)
                                          select pu).ToList();
                    Warehouse_Contains wCGetUnit = WarehouseEnt.Warehouse_Contains.Find(WarehouseSelected, int.Parse(ProductSelected));
                    foreach (var pu in queryProdUnits)

                    {
                        if (wCGetUnit.Unit == pu.Unit)
                        {

                            comboBox6.Items.Add(pu.Unit);
                            comboBox6.Text = pu.Unit;
                            

                            break;
                        }
                    }
                }
            }
            catch { }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            float TotalQuantity = 0;

            try
            {

                Transfer transfer = new Transfer();
                Transfer_From transfer_From = new Transfer_From();
                 Warehouse_Contains warehouse_Contains = new Warehouse_Contains();
                int ProductSelected = int.Parse(comboBox5.Text.Split('|')[0]);
                int WarehouseFrom = int.Parse(comboBox1.Text.Split('|')[0]);
                int WarehouseTo = int.Parse(comboBox2.Text.Split('|')[0]);
                float InputQuantity = float.Parse(textBox4.Text);


                var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                var queryWarehouseContains = (from wc in warehouseContains
                                              where wc.Warehouse_ID == WarehouseFrom
                                              && wc.Pcode == ProductSelected
                                              && wc.Unit == comboBox6.Text
                                              select wc).ToList();

                foreach (var item in queryWarehouseContains)
                {
                    if (item.Dispensed_Flag == 1)
                    {
                        var warehouseDispense = WarehouseEnt.Warehouse_Dispense.AsEnumerable();
                        var queryWarehouseDispense = (from wd in warehouseDispense
                                                      where wd.Warehouse_ID == WarehouseFrom
                                                      && wd.Pcode == ProductSelected
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
                    else
                    {
                        TotalQuantity += (float)item.Quantity;
                    }
                }
                    if (InputQuantity > TotalQuantity)
                    {
                        MessageBox.Show("There is not enough stock in the warehouse");

                    }
                
                else
                {
                    foreach (var item in queryWarehouseContains) 
                    {
                        if (InputQuantity <= item.Quantity )
                        {

                            transfer.Transfer_ID = int.Parse(textBox3.Text);
                            transfer.From_ID = WarehouseFrom;
                            transfer.To_ID = WarehouseTo;
                            transfer.Quantity = InputQuantity;
                            transfers.Add(transfer);

                            

                            transfer_From.Warehouse_ID = WarehouseFrom;
                            transfer_From.Transfer_ID = transfer.Transfer_ID;
                            transfer_From.Pcode = ProductSelected;
                            transfer_From.Old_Date = item.Added_Date.Value;
                            transfer_From.Old_Quantity = item.Quantity;
                            transfer_From.New_Date = dateTimePicker1.Value;
                            transfer_From.New_Quantity = item.Quantity - InputQuantity;
                            Transfer_Froms.Add(transfer_From);


                        warehouse_Contains.Warehouse_ID = WarehouseTo;
                        warehouse_Contains.Pcode = ProductSelected;
                        warehouse_Contains.Added_Date = dateTimePicker1.Value;
                        warehouse_Contains.Quantity = InputQuantity;
                        warehouse_Contains.Unit = item.Unit;
                        warehouse_ContainsList.Add(warehouse_Contains);
                        comboBox5.Text = comboBox6.Text = textBox4.Text = string.Empty;
                        //item.Transfer_ID = transfer.Transfer_ID;
                        warehouse_ContainsList.Add(item);




                        }

                    }
                    
                }








        }
            catch { MessageBox.Show("Inputs not in correct format"); }
}

        private void button6_Click(object sender, EventArgs e)
        {
            comboBox5.Text = comboBox6.Text = textBox4.Text = string.Empty;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            Transfer_From transfer_From = new Transfer_From();
            Warehouse_Contains wContains = new Warehouse_Contains();
            try
            {
                if (textBox3.Text != string.Empty && comboBox2.Text != string.Empty && comboBox1.Text != string.Empty)
                {
                    transfer.Transfer_ID = transfers[0].Transfer_ID;
                    transfer.From_ID = transfers[0].From_ID;
                    transfer.To_ID = transfers[0].To_ID;
                    transfer.Quantity = transfers[0].Quantity;
                    WarehouseEnt.Transfers.Add(transfer);
                    for (int i = 0; i < Transfer_Froms.Count; i++)
                    {
                        int compareDate = DateTime.Compare(Transfer_Froms[i].New_Date.Value.Date, Transfer_Froms[i].Old_Date.Value.Date);
                        if (compareDate >= 0)
                        {
                            
                            transfer_From.Warehouse_ID = Transfer_Froms[i].Warehouse_ID;
                            transfer_From.Transfer_ID = Transfer_Froms[i].Transfer_ID;
                            transfer_From.Pcode = Transfer_Froms[i].Pcode;
                            transfer_From.Old_Date = Transfer_Froms[i].Old_Date;
                            transfer_From.Old_Quantity = Transfer_Froms[i].Old_Quantity;
                            transfer_From.New_Date = Transfer_Froms[i].New_Date;
                            transfer_From.New_Quantity = Transfer_Froms[i].New_Quantity;
                            
                            WarehouseEnt.Transfer_From.Add(transfer_From);
                            
                            wContains.Warehouse_ID = warehouse_ContainsList[i].Warehouse_ID;
                            wContains.Pcode = warehouse_ContainsList[i].Pcode;
                            wContains.Unit = warehouse_ContainsList[i].Unit;
                            wContains.Quantity = warehouse_ContainsList[i].Quantity;
                            wContains.Added_Date = warehouse_ContainsList[i].Added_Date;
                            warehouse_ContainsList[i].Transfer_ID = Transfer_Froms[i].Transfer_ID; 
                            WarehouseEnt.Warehouse_Contains.Add(wContains);
                        //var warehouseContains = WarehouseEnt.Warehouse_Contains.AsEnumerable();

                        //var queryWarehouseContains = (from wc in warehouseContains
                        //                              where wc.Warehouse_ID == warehouse_ContainsList[i].Warehouse_ID
                        //                              && wc.Pcode == warehouse_ContainsList[i].Pcode
                        //                              && wc.Unit == warehouse_ContainsList[i].Unit
                        //                              select wc).ToList();
                        //foreach ( var item in queryWarehouseContains)
                        //{
                        //    if (wContains.Warehouse_ID == item.Warehouse_ID && wContains.Pcode == item.Pcode)
                        //    {
                        //        item.Quantity = wContains.Quantity;
                        //        item.
                        //    }
                        //}
                        /*
                         change pk for warehouse_contains 
                         */

                        WarehouseEnt.SaveChanges();
                            MessageBox.Show(warehouse_ContainsList[i].Transfer_ID.ToString());
                           
                            
                            MessageBox.Show("Transfer Added");
                            RefreshListView();
                        }
                        else
                        {
                            MessageBox.Show($"Choose date after {transfer_From.Old_Date} to dispense product");
                        }
                    }
                    
                }
            }


            catch { MessageBox.Show("Inputs not in correct format"); }

        }
    }
}
