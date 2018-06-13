namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_GroupKey",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDated = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CMS_R_GroupKey_KeyWord",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        GroupKeyID = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeyWordID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_KeyWord", t => t.KeyWordID)
                .ForeignKey("dbo.CMS_GroupKey", t => t.GroupKeyID)
                .Index(t => t.GroupKeyID)
                .Index(t => t.KeyWordID);
            
            CreateTable(
                "dbo.CMS_KeyWord",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeyWord = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CMS_R_KeyWord_Pin",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeyWordID = c.String(nullable: false, maxLength: 60, unicode: false),
                        PinID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_Pin", t => t.PinID)
                .ForeignKey("dbo.CMS_KeyWord", t => t.KeyWordID)
                .Index(t => t.KeyWordID)
                .Index(t => t.PinID);
            
            CreateTable(
                "dbo.CMS_Pin",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Link = c.String(nullable: false, maxLength: 256),
                        Domain = c.String(nullable: false, maxLength: 100),
                        Repin_count = c.Int(),
                        ImageUrl = c.String(maxLength: 256),
                        Created_At = c.DateTime(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AlterColumn("dbo.CMS_Employee", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.CMS_Employee", "CreatedDate", c => c.DateTime());
            DropTable("dbo.CMS_GroupSearch");
            DropTable("dbo.CMS_Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CMS_Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_GroupSearch",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeySearch = c.String(nullable: false, maxLength: 100, unicode: false),
                        Quantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CMS_R_GroupKey_KeyWord", "GroupKeyID", "dbo.CMS_GroupKey");
            DropForeignKey("dbo.CMS_R_KeyWord_Pin", "KeyWordID", "dbo.CMS_KeyWord");
            DropForeignKey("dbo.CMS_R_KeyWord_Pin", "PinID", "dbo.CMS_Pin");
            DropForeignKey("dbo.CMS_R_GroupKey_KeyWord", "KeyWordID", "dbo.CMS_KeyWord");
            DropIndex("dbo.CMS_R_KeyWord_Pin", new[] { "PinID" });
            DropIndex("dbo.CMS_R_KeyWord_Pin", new[] { "KeyWordID" });
            DropIndex("dbo.CMS_R_GroupKey_KeyWord", new[] { "KeyWordID" });
            DropIndex("dbo.CMS_R_GroupKey_KeyWord", new[] { "GroupKeyID" });
            AlterColumn("dbo.CMS_Employee", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CMS_Employee", "UpdatedDate", c => c.DateTime(nullable: false));
            DropTable("dbo.CMS_Pin");
            DropTable("dbo.CMS_R_KeyWord_Pin");
            DropTable("dbo.CMS_KeyWord");
            DropTable("dbo.CMS_R_GroupKey_KeyWord");
            DropTable("dbo.CMS_GroupKey");
        }
    }
}
