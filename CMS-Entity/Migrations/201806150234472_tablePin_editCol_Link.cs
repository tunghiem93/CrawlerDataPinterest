namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablePin_editCol_Link : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Pin", "Link", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Pin", "Link", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
