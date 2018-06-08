namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolumnIdInCategory : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CMS_Categories");
            AlterColumn("dbo.CMS_Categories", "Id", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AddPrimaryKey("dbo.CMS_Categories", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CMS_Categories");
            AlterColumn("dbo.CMS_Categories", "Id", c => c.String(nullable: false, maxLength: 60));
            AddPrimaryKey("dbo.CMS_Categories", "Id");
        }
    }
}
