namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_R_Board_Pin
    {
        [StringLength(60)]
        public string ID { get; set; }

        [Required]
        [StringLength(60)]
        public string BoardID { get; set; }

        [Required]
        [StringLength(60)]
        public string PinID { get; set; }

        public int Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public virtual CMS_Board CMS_Board { get; set; }

        public virtual CMS_Pin CMS_Pin { get; set; }
    }
}
