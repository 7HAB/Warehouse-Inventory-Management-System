namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductUnits = new HashSet<ProductUnit>();
            Warehouse_Contains = new HashSet<Warehouse_Contains>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pcode { get; set; }

        [StringLength(20)]
        public string P_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Production_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Expiration_date { get; set; }

        public int? Supplier_ID { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductUnit> ProductUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse_Contains> Warehouse_Contains { get; set; }
    }
}
