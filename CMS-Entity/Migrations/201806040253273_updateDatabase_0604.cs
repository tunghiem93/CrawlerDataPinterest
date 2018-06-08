namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDatabase_0604 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Customers", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Customers", "BirthDate", c => c.String());
        }
    }
}
