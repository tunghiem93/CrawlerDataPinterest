namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Categories", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CMS_Categories", "CreatedBy", c => c.String());
            AddColumn("dbo.CMS_Categories", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CMS_Categories", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Categories", "UpdatedBy");
            DropColumn("dbo.CMS_Categories", "UpdatedDate");
            DropColumn("dbo.CMS_Categories", "CreatedBy");
            DropColumn("dbo.CMS_Categories", "CreatedDate");
        }
    }
}
