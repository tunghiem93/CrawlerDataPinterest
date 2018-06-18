namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_GroupKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_GroupKey()
        {
            CMS_R_GroupKey_KeyWord = new HashSet<CMS_R_GroupKey_KeyWord>();
        }

        [StringLength(60)]
        public string ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Sequence { get; set; }

        public int Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_GroupKey_KeyWord> CMS_R_GroupKey_KeyWord { get; set; }
    }
}
