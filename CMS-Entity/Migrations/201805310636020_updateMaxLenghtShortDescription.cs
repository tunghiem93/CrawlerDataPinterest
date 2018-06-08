namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMaxLenghtShortDescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_News", "Short_Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_News", "Short_Description", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
