namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        ProductId = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMS_Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CMS_Images", "ProductId", "dbo.CMS_Products");
            DropIndex("dbo.CMS_Images", new[] { "ProductId" });
            DropTable("dbo.CMS_Images");
        }
    }
}
