namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Warehouse_Contains
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Warehouse_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pcode { get; set; }

        public int? Transfer_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Added_Date { get; set; }

        public double? Quantity { get; set; }

        public int? Permission_Number { get; set; }

        [StringLength(10)]
        public string Unit { get; set; }

        public int? Dispensed_Flag { get; set; }

        public virtual Product Product { get; set; }

        public virtual Transfer Transfer { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}
