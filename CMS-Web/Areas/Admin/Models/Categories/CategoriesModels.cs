using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_Web.Areas.Admin.Models.Categories
{
    public class CategoriesModels
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int NumberOfProduct { get; set; }
        // upload image
        public byte[] PictureByte { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }

        public CategoriesModels()
        {
            IsActive = true;
        }
    }
}