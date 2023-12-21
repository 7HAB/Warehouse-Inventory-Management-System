namespace DA_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Transfer_From
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Warehouse_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pcode { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Transfer_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Old_Date { get; set; }

        public double? Old_Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? New_Date { get; set; }

        public double? New_Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual Transfer Transfer { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}
