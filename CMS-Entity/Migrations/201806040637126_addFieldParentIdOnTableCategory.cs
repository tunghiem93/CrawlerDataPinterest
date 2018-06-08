namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldParentIdOnTableCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Categories", "ParentId", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Categories", "ParentId");
        }
    }
}
