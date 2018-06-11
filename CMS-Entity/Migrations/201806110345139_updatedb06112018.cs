namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb06112018 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CMS_Images", "ProductId", "dbo.CMS_Products");
            DropIndex("dbo.CMS_Images", new[] { "ProductId" });
            CreateTable(
                "dbo.CMS_GroupSearch",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeySearch = c.String(nullable: false, maxLength: 100, unicode: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.CMS_Images", "ProductId");
            DropTable("dbo.CMS_Board");
            DropTable("dbo.CMS_Products");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CMS_Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ProductName = c.String(nullable: false, maxLength: 500),
                        repin_count = c.Int(nullable: false),
                        Link = c.String(nullable: false, maxLength: 60),
                        Board = c.String(nullable: false, maxLength: 60),
                        Created_At = c.DateTime(nullable: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Board",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        BoardName = c.String(nullable: false, maxLength: 500),
                        repin_count = c.Int(nullable: false),
                        Link = c.String(nullable: false, maxLength: 60),
                        Created_At = c.DateTime(nullable: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CMS_Images", "ProductId", c => c.String(maxLength: 60, unicode: false));
            DropTable("dbo.CMS_GroupSearch");
            CreateIndex("dbo.CMS_Images", "ProductId");
            AddForeignKey("dbo.CMS_Images", "ProductId", "dbo.CMS_Products", "Id");
        }
    }
}
