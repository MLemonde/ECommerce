namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShoppingCart")]
    public partial class ShoppingCart
    {
        public int Id { get; set; }

        public int? Quantity { get; set; }

        public DateTime? DateCreated { get; set; }

        [StringLength(10)]
        public string CartID { get; set; }

        public int? ProductID { get; set; }
    }
}
