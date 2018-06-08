namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDb06082018 : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.CMS_Employee",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Employee_Address = c.String(maxLength: 250),
                        Employee_Phone = c.String(nullable: false, maxLength: 11, unicode: false),
                        Employee_Email = c.String(nullable: false, maxLength: 250, unicode: false),
                        Employee_IDCard = c.String(maxLength: 50, unicode: false),
                        BirthDate = c.DateTime(),
                        Password = c.String(nullable: false, maxLength: 250, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsSupperAdmin = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CMS_Images", "ProductId", "dbo.CMS_Products");
            DropIndex("dbo.CMS_Images", new[] { "ProductId" });
            DropTable("dbo.CMS_Products");
            DropTable("dbo.CMS_Images");
            DropTable("dbo.CMS_Employee");
            DropTable("dbo.CMS_Board");
        }
    }
}
