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
        public int RecordID { get; set; }

        [Required]
        [StringLength(128)]
        public string CartID { get; set; }

        public int ProductID { get; set; }

        public int Count { get; set; }

        public DateTime? DateCreated { get; set; }

        public virtual Product Product { get; set; }
    }
}
