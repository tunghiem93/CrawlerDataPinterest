namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_R_GroupBoard_Board
    {
        [StringLength(60)]
        public string ID { get; set; }

        [StringLength(60)]
        public string GroupBoardID { get; set; }

        [StringLength(60)]
        public string BoardID { get; set; }

        public int? Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual CMS_Board CMS_Board { get; set; }

        public virtual CMS_GroupBoard CMS_GroupBoard { get; set; }
    }
}
