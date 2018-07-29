namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_Account()
        {
            CMS_Board = new HashSet<CMS_Board>();
            CMS_KeyWord = new HashSet<CMS_KeyWord>();
        }

        [StringLength(60)]
        public string ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Cookies { get; set; }

        public int? Status { get; set; }

        public int? State { get; set; }
        public int? Sequence { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_Board> CMS_Board { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_KeyWord> CMS_KeyWord { get; set; }
    }
}
