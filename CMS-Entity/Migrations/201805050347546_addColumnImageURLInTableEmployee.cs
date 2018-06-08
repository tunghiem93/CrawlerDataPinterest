namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnImageURLInTableEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Employee", "ImageURL", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Employee", "ImageURL");
        }
    }
}
