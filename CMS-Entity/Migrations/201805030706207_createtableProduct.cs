namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ProductCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProductName = c.String(nullable: false, maxLength: 250, unicode: false),
                        ProductPrice = c.Decimal(precision: 18, scale: 2),
                        CategoryId = c.String(nullable: false, maxLength: 60, unicode: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMS_Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CMS_Products", "CategoryId", "dbo.CMS_Categories");
            DropIndex("dbo.CMS_Products", new[] { "CategoryId" });
            DropTable("dbo.CMS_Products");
        }
    }
}
