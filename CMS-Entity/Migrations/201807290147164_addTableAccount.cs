namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Account",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Sequence = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Account = c.String(nullable: false, maxLength: 100, unicode: false),
                        Password = c.String(nullable: false, maxLength: 250, unicode: false),
                        Cookies = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMS_Account");
        }
    }
}
