namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableNews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_News",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Title = c.String(nullable: false, maxLength: 150),
                        Short_Description = c.String(nullable: false, maxLength: 250),
                        Description = c.String(storeType: "ntext"),
                        ImageUrl = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMS_News");
        }
    }
}
