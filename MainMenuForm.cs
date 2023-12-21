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
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WarehouseForm warehouseForm = new WarehouseForm();
            warehouseForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        
        {
            SupplierForm supplierForm = new SupplierForm();
            supplierForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProductForm productForm = new ProductForm();
            productForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PermissionForm permissionForm = new PermissionForm();   
            permissionForm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ReportProductsCloseToExpiry reportProductsCloseToExpiry = new ReportProductsCloseToExpiry();
                reportProductsCloseToExpiry.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ReportOnWarehouse reportOnWarehouse = new ReportOnWarehouse();      
            reportOnWarehouse.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReportOnProducts reportOnProducts   = new ReportOnProducts();   
                reportOnProducts.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TransferForm transferForm = new TransferForm();
                transferForm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ReportOnTransfers reportOnTransfers = new ReportOnTransfers();
                reportOnTransfers.ShowDialog();
        }
    }
}
