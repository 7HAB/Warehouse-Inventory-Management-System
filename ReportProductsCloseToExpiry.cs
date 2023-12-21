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
    public partial class ReportProductsCloseToExpiry : Form
    {
        Model1 WarehouseEnt = new Model1();

        public ReportProductsCloseToExpiry()
        {
            InitializeComponent();
        }
        public void RefreshViewList()
        {

            numericUpDown1.Value = 0;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.Clear();
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

        }
        private void ReportProductsCloseToExpiry_Load(object sender, EventArgs e)
        {
            RefreshViewList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ExpireAfter = (int)numericUpDown1.Value;
            DateTime currentDate = DateTime.Now;
            DateTime expiryDate = currentDate.AddMonths(ExpireAfter);

            bool exists = false;

            if (numericUpDown1.Value >= 0)
            {
                var products = WarehouseEnt.Products.AsEnumerable();

                foreach (var product in products)
                {
                    MessageBox.Show(product.P_Name + "  "+ product.Expiration_date.ToString());
                    int compareMin = DateTime.Compare(currentDate.Date, product.Expiration_date.Value.Date);
                    int compareMax = DateTime.Compare(expiryDate.Date, product.Expiration_date.Value.Date);
                    if ( compareMax >= 0)
                    {
                        

                        Supplier s = WarehouseEnt.Suppliers.Find(product.Supplier_ID);
                        foreach (ProductUnit pu in WarehouseEnt.ProductUnits.Where(p => p.Pcode == product.Pcode))
                        {
                            string[] WRow = { product.Pcode.ToString(), product.P_Name, product.Production_Date.Value.ToString("yyyy-MM-dd"), product.Expiration_date.Value.ToString("yyyy-MM-dd"), product.Supplier_ID.ToString(), s.Supplier_Name, pu.Unit };
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

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm = new MainMenuForm();
            mainMenuForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshViewList();
        }
    }
}
