using CMS_DTO.CMSCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSProduct
{
    public class ProductViewModels
    {
        public List<CMS_ProductsModels> ListProduct { get; set; }

        public string CateID { get; set; }
        public List<CMSCategoriesModels> ListCate { get; set; }
        public int TotalProduct { get; set; }
        public bool IsAddMore { get; set; }
        public int TotalPage { get; set; }
        public string Key { get; set; }
        public bool IsOrther { get; set; }
        public ProductViewModels()
        {
            ListCate = new List<CMSCategoriesModels>();
            ListProduct = new List<CMS_ProductsModels>();
        }
    }
}
