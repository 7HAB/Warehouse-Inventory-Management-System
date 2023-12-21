namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Warehouse_Contains
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Warehouse_Contains()
        {
            Permission_Product = new HashSet<Permission_Product>();
            TransferProductFroms = new HashSet<TransferProductFrom>();
            Warehouse_Dispense = new HashSet<Warehouse_Dispense>();
        }

        public int? Warehouse_ID { get; set; }

        public int? Pcode { get; set; }

        public double? Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Added_date { get; set; }

        public int? Transfer_ID { get; set; }

        [StringLength(10)]
        public string Unit { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WarehouseContains_ID { get; set; }

        public int? Permission_Number { get; set; }

        public virtual Permission Permission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Permission_Product> Permission_Product { get; set; }

        public virtual Product Product { get; set; }

        public virtual TransferProduct TransferProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferProductFrom> TransferProductFroms { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse_Dispense> Warehouse_Dispense { get; set; }
    }
}
