namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Warehouse_Dispense = new HashSet<Warehouse_Dispense>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Customer_ID { get; set; }

        [StringLength(20)]
        public string Customer_Name { get; set; }

        public int? Phone { get; set; }

        public int? Mobile { get; set; }

        public int? Fax { get; set; }

        [StringLength(20)]
        public string Email { get; set; }

        [StringLength(20)]
        public string website { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse_Dispense> Warehouse_Dispense { get; set; }
    }
}
