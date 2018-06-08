namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldPasswordInTableEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Employee", "Password", c => c.String(nullable: false, maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Employee", "Password");
        }
    }
}
