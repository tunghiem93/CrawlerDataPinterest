namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Log
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_Log()
        {
        }

        [StringLength(60)]
        public string ID { get; set; }

        [StringLength(100)]
        public string Decription { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        [StringLength(4000)]
        public string JsonContent { get; set; }
    }
}
