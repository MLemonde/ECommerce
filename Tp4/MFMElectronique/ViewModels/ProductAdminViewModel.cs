using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFMElectronique.ViewModels
{
    public class ProductAdminViewModel
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [AllowHtml]
        [DisplayName("Description of the product in French")]
        public string DescriptionFR { get; set; }
        [AllowHtml]
        [DisplayName("Description of the product in English")]
        public string DescriptionEN { get; set; }
        [DisplayName("Is the produt discontinued?")]
        public bool discontinued { get; set; }
        [DisplayName("Would you kindly upload a picture please")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
        [DisplayName("Price")]
        public decimal Price { get; set; }
        [DisplayName("Category")]
        public int CategoryID { get; set; }
        [DisplayName("Brand")]
        public int BrandID { get; set; }

    }
}