namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb0611 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_GroupSearch", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CMS_GroupSearch", "CreatedBy", c => c.String());
            AddColumn("dbo.CMS_GroupSearch", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CMS_GroupSearch", "UpdatedBy", c => c.String());
            AddColumn("dbo.CMS_GroupSearch", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_GroupSearch", "CreatedDate");
            DropColumn("dbo.CMS_GroupSearch", "UpdatedBy");
            DropColumn("dbo.CMS_GroupSearch", "UpdatedDate");
            DropColumn("dbo.CMS_GroupSearch", "CreatedBy");
            DropColumn("dbo.CMS_GroupSearch", "IsActive");
        }
    }
}
