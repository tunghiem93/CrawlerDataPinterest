namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_KeyWord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_KeyWord()
        {
            CMS_R_GroupKey_KeyWord = new HashSet<CMS_R_GroupKey_KeyWord>();
            CMS_R_KeyWord_Pin = new HashSet<CMS_R_KeyWord_Pin>();
            CMS_R_Board_KeyWord = new HashSet<CMS_R_Board_KeyWord>();
        }

        [StringLength(60)]
        public string ID { get; set; }

        [Required]
        [StringLength(100)]
        public string KeyWord { get; set; }

        public int Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Sequence { get; set; }

        [StringLength(60)]
        public string CrawlAccountID { get; set; }

        public virtual CMS_Account CMS_Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_GroupKey_KeyWord> CMS_R_GroupKey_KeyWord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_KeyWord_Pin> CMS_R_KeyWord_Pin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_Board_KeyWord> CMS_R_Board_KeyWord { get; set; }
    }
}
