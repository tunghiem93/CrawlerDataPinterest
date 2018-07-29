namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Board
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_Board()
        {
            CMS_R_Board_KeyWord = new HashSet<CMS_R_Board_KeyWord>();
            CMS_R_GroupBoard_Board = new HashSet<CMS_R_GroupBoard_Board>();
        }

        [StringLength(60)]
        public string ID { get; set; }

        [StringLength(250)]
        public string BoardName { get; set; }

        public int Status { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        [StringLength(60)]
        public string CrawlAccountID { get; set; }

        public virtual CMS_Account CMS_Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_Board_KeyWord> CMS_R_Board_KeyWord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_R_GroupBoard_Board> CMS_R_GroupBoard_Board { get; set; }
    }
}
