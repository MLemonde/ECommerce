namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int ProductID { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }


        //public int Id { get; set; }

        //public int? Quantity { get; set; }

        //public DateTime? DateCreated { get; set; }

        //[StringLength(10)]
        //public string CartID { get; set; }

        //public int? ProductID { get; set; }
    }
}
