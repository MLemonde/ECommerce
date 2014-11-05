namespace MFMElectronique.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        [Column("Comment")]
        [Required]
        public string Comment1 { get; set; }

        public DateTime? DateCreated { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        public virtual AspNetUser AspNetUsers { get; set; }

        public virtual Product Product { get; set; }
    }
}
