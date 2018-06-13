namespace CMS_Entity.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CMS_Context : DbContext
    {
        public CMS_Context()
            : base("name=CMS_Context")
        {
        }

        public virtual DbSet<CMS_Employee> CMS_Employee { get; set; }
        public virtual DbSet<CMS_GroupKey> CMS_GroupKey { get; set; }
        public virtual DbSet<CMS_KeyWord> CMS_KeyWord { get; set; }
        public virtual DbSet<CMS_Pin> CMS_Pin { get; set; }
        public virtual DbSet<CMS_R_GroupKey_KeyWord> CMS_R_GroupKey_KeyWord { get; set; }
        public virtual DbSet<CMS_R_KeyWord_Pin> CMS_R_KeyWord_Pin { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.Employee_Phone)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.Employee_Email)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.Employee_IDCard)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.ImageURL)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Employee>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_GroupKey>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_GroupKey>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_GroupKey>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_GroupKey>()
                .HasMany(e => e.CMS_R_GroupKey_KeyWord)
                .WithRequired(e => e.CMS_GroupKey)
                .HasForeignKey(e => e.GroupKeyID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CMS_KeyWord>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_KeyWord>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_KeyWord>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_KeyWord>()
                .HasMany(e => e.CMS_R_GroupKey_KeyWord)
                .WithRequired(e => e.CMS_KeyWord)
                .HasForeignKey(e => e.KeyWordID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CMS_KeyWord>()
                .HasMany(e => e.CMS_R_KeyWord_Pin)
                .WithRequired(e => e.CMS_KeyWord)
                .HasForeignKey(e => e.KeyWordID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CMS_Pin>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Pin>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Pin>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_Pin>()
                .HasMany(e => e.CMS_R_KeyWord_Pin)
                .WithRequired(e => e.CMS_Pin)
                .HasForeignKey(e => e.PinID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CMS_R_GroupKey_KeyWord>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_GroupKey_KeyWord>()
                .Property(e => e.GroupKeyID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_GroupKey_KeyWord>()
                .Property(e => e.KeyWordID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_GroupKey_KeyWord>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_GroupKey_KeyWord>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_KeyWord_Pin>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_KeyWord_Pin>()
                .Property(e => e.KeyWordID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_KeyWord_Pin>()
                .Property(e => e.PinID)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_KeyWord_Pin>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CMS_R_KeyWord_Pin>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);
        }
    }
}
