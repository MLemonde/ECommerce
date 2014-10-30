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
            Cart = new HashSet<Cart>();
            Comment = new HashSet<Comment>();
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string DescriptionFR { get; set; }

        [Required]
        [StringLength(255)]
        public string DescriptionEN { get; set; }

        public bool discontinued { get; set; }

        [Required]
        public string PictureURL { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        public int BrandID { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

        public virtual ProductBrand ProductBrand { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
