namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Pin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_Pin()
        {
            CMS_R_KeyWord_Pin = new HashSet<CMS_R_KeyWord_Pin>();
        }

        [StringLength(60)]
        public string ID { get; set; }

        [StringLength(2000)]
        public string Link { get; set; }

        [Required]
        [StringLength(100)]
        public string Domain { get; set; }

        public int? Repin_count { get; set; }

        [StringLength(2000)]
        public string ImageUrl { get; set; }

        public DateTime? Created_At { get; set; }

        public int Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_KeyWord_Pin> CMS_R_KeyWord_Pin { get; set; }
    }
}
