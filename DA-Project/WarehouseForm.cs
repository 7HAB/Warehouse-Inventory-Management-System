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
    public partial class WarehouseForm : Form
    {
        Model1 WarehouseEnt = new Model1();
        public WarehouseForm()
        {
            InitializeComponent();
        }

        private void WarehouseForm_Load(object sender, EventArgs e)
        {
            RefreshListView();
        }
        public void RefreshListView()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = string.Empty;

            listView1.View = View.Details;
            listView1.Clear();
            listView1.Columns.Add("Warehouse ID     ");
            listView1.Columns.Add("Warehouse Name   ");
            listView1.Columns.Add("Manager Name     ");
            listView1.Columns.Add("Warehouse Location   ");
            for (int i = 0; i < 4; i++)
            {
                listView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);



            }
            foreach (Warehouse w in WarehouseEnt.Warehouses)
            {
                string[] WRow = { w.Warehouse_ID.ToString(), w.Warehouse_Name, w.Manager_Name, w.Warehouse_Location };
                var listViewItemWarehouse = new ListViewItem(WRow);
                listView1.Items.Add(listViewItemWarehouse);
                for (int i = 0; i < 4; i++)
                {
                    listView1.Columns[i].Width = -2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            Warehouse warehouse = new Warehouse();
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty)
            {
                try
                {
                    Warehouse w = WarehouseEnt.Warehouses.Find(int.Parse(textBox1.Text));
                    if (w == null)
                    {

                        warehouse.Warehouse_ID = int.Parse(textBox1.Text);


                        warehouse.Warehouse_Name = textBox2.Text;
                        warehouse.Manager_Name = textBox3.Text;
                        warehouse.Warehouse_Location = textBox4.Text;
                        WarehouseEnt.Warehouses.Add(warehouse);
                        MessageBox.Show("Warehouse Added");
                        WarehouseEnt.SaveChanges();
                        RefreshListView();


                    }
                    else { MessageBox.Show("Warehouse Exists"); }
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
                MessageBox.Show("Missing Warehouse ID Field");
            }
            else
            {
                try
                {
                    Warehouse w = WarehouseEnt.Warehouses.Find(int.Parse(textBox1.Text));
                    if (w != null)
                    {
                        if (textBox2.Text != string.Empty)
                        {
                            w.Warehouse_Name = textBox2.Text;

                        }

                        if (textBox3.Text != string.Empty)
                        {
                            w.Manager_Name = textBox3.Text;

                        }

                        if (textBox4.Text != string.Empty)
                        {
                            w.Warehouse_Location = textBox4.Text;

                        }
                        MessageBox.Show("Warehouse Updated");
                        WarehouseEnt.SaveChanges();
                        RefreshListView();


                    }
                    else
                    {
                        MessageBox.Show("Warehouse not found");
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
            MainMenuForm mainmenu = new MainMenuForm();
            mainmenu.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }
    }
}
