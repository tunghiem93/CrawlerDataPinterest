namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb13062018 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Employee", "ImageURL", c => c.String(maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Employee", "ImageURL", c => c.String(maxLength: 60, unicode: false));
        }
    }
}
