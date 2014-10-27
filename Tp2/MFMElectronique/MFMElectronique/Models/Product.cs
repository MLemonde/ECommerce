namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Comment = new HashSet<Comment>();
        }

        public int ID { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
    }
}
