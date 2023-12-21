namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Warehouse_Dispense
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Permission_Number { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WarehouseContainsID_From { get; set; }

        public double? Old_Quantity { get; set; }

        public double? New_Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Old_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? New_date { get; set; }

        public int? Customer_ID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Permission Permission { get; set; }

        public virtual Warehouse_Contains Warehouse_Contains { get; set; }
    }
}
