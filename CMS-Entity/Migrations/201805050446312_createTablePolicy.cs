namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTablePolicy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Policy",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Description = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMS_Policy");
        }
    }
}
