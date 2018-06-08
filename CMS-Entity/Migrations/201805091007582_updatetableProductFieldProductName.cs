namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableProductFieldProductName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Products", "ProductName", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Products", "ProductName", c => c.String(nullable: false, maxLength: 250, unicode: false));
        }
    }
}
