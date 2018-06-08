namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase0305 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Products", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Products", "IsActive");
        }
    }
}
