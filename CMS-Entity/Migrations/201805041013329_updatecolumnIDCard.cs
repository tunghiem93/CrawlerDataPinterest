namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolumnIDCard : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Employee", "Employee_IDCard", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Employee", "Employee_IDCard", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
