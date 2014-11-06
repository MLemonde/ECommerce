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


        public string Name { get; set; }

        [AllowHtml]
        public string DescriptionFR { get; set; }

        [AllowHtml]
        public string DescriptionEN { get; set; }

        public bool discontinued { get; set; }

        [DisplayName("Uploader une photo")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        public int BrandID { get; set; }

    }
}