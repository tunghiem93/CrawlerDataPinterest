namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_R_GroupKey_KeyWord
    {
        [StringLength(60)]
        public string ID { get; set; }

        [Required]
        [StringLength(60)]
        public string GroupKeyID { get; set; }

        [Required]
        [StringLength(60)]
        public string KeyWordID { get; set; }

        public int Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual CMS_GroupKey CMS_GroupKey { get; set; }

        public virtual CMS_KeyWord CMS_KeyWord { get; set; }
    }
}
