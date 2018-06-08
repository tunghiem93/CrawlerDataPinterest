using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS_DTO.CMSHome
{
    public class CMSHomeModels
    {
        public SilderModels Silder { get; set; }

        public CMSHomeModels()
        {
            Silder = new SilderModels();
        }
    }

    public class SilderModels
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload1 { get; set; }

        public byte[] PictureByte1 { get; set; }
        public string ImageURL1 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload2 { get; set; }

        public byte[] PictureByte2 { get; set; }
        public string ImageURL2 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload3 { get; set; }

        public byte[] PictureByte3 { get; set; }
        public string ImageURL3 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload4 { get; set; }

        public byte[] PictureByte4 { get; set; }
        public string ImageURL4 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload5 { get; set; }

        public byte[] PictureByte5 { get; set; }
        public string ImageURL5 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload6 { get; set; }

        public byte[] PictureByte6 { get; set; }
        public string ImageURL6 { get; set; }
    }
}
