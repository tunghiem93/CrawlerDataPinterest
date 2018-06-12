using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSCrawler
{
    public class CMS_CrawlerModels
    {
        public List<PinsModels> Pins { get; set; }
        public PinsModels Pin { get; set; }
        public CMS_CrawlerModels()
        {
            Pins = new List<PinsModels>();
            Pin = new PinsModels();
        }
    }

    public class PinsModels
    {
        public string Domain { get; set; }
        public string Link { get; set; }
        public int Repin_count { get; set; }
        public string ID { get; set; }
        public DateTime Created_At { get; set; }
        public BoardModels Board { get; set; }
        public List<ImageModels> Images { get; set; }
        public PinsModels()
        {
            Board = new BoardModels();
            Images = new List<ImageModels>();
        }
    }

    public class BoardModels
    {
        public bool Is_collaborative { get; set; }
        public string Layout { get; set; }
        public string Name { get; set; }
        public string Privacy { get; set; }
        public string Url { get; set; }
        public bool Followed_by_me { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Image_thumbnail_url { get; set; }
    }

    public class ImageModels
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
