namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [Column(TypeName = "money")]
        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        public virtual AspNetUser AspNetUsers { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
