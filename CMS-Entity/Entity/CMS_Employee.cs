namespace CMS_Entity.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Employee
    {
        [StringLength(60)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Employee_Address { get; set; }

        [Required]
        [StringLength(11)]
        public string Employee_Phone { get; set; }

        [Required]
        [StringLength(250)]
        public string Employee_Email { get; set; }

        [StringLength(50)]
        public string Employee_IDCard { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }

        [StringLength(250)]
        public string ImageURL { get; set; }

        public bool IsSupperAdmin { get; set; }

        public bool IsActive { get; set; }

        [StringLength(60)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(60)]
        public string UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
