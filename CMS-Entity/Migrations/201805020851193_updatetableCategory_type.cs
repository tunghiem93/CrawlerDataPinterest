namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableCategory_type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Categories", "CategoryCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Categories", "CategoryCode", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
