namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

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
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DefaultValue("")]
        [AllowHtml]
        [DataType(DataType.Html)]
        [Display(Name = "Description in French")]
        public string DescriptionFR { get; set; }

        [Required]
        [DefaultValue("")]
        [AllowHtml]
        [DataType(DataType.Html)]
        [Display(Name = "Description in English")]
        public string DescriptionEN { get; set; }

        [Display(Name = "Product is discontinued")]
        public bool discontinued { get; set; }

        [Required]
        [Display(Name = "The pictures")]
        public string PictureURL { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Brand")]
        public int BrandID { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

        public virtual ProductBrand ProductBrand { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
