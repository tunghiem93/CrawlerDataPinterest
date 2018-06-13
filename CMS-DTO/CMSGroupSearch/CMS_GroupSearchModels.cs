﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSGroupSearch
{
    public class CMS_GroupSearchModels
    {
        public string Id { get; set; }
        public string KeySearch { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
        public int OffSet { get; set; }

        public List<CMS_GroupSearchModels> ListKeyResult { get; set; }
        public CMS_GroupSearchModels()
        {
            ListKeyResult = new List<CMS_GroupSearchModels>();
        }
    }
}