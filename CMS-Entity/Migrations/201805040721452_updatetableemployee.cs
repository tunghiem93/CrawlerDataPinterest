namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableemployee : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CMS_Employee");
            AddColumn("dbo.CMS_Employee", "Id", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AddColumn("dbo.CMS_Employee", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.CMS_Employee", "LastName", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.CMS_Employee", "BirthDate", c => c.DateTime());
            AddColumn("dbo.CMS_Employee", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CMS_Employee", "CreatedBy", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.CMS_Employee", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CMS_Employee", "UpdatedBy", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.CMS_Employee", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CMS_Employee", "Employee_Address", c => c.String(maxLength: 250));
            AlterColumn("dbo.CMS_Employee", "Employee_Phone", c => c.String(nullable: false, maxLength: 11, unicode: false));
            AlterColumn("dbo.CMS_Employee", "Employee_Email", c => c.String(nullable: false, maxLength: 250, unicode: false));
            AlterColumn("dbo.CMS_Employee", "Employee_IDCard", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddPrimaryKey("dbo.CMS_Employee", "Id");
            DropColumn("dbo.CMS_Employee", "Employee_ID");
            DropColumn("dbo.CMS_Employee", "Employee_Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CMS_Employee", "Employee_Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.CMS_Employee", "Employee_ID", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.CMS_Employee");
            AlterColumn("dbo.CMS_Employee", "Employee_IDCard", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CMS_Employee", "Employee_Email", c => c.String());
            AlterColumn("dbo.CMS_Employee", "Employee_Phone", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.CMS_Employee", "Employee_Address", c => c.String());
            DropColumn("dbo.CMS_Employee", "CreatedDate");
            DropColumn("dbo.CMS_Employee", "UpdatedBy");
            DropColumn("dbo.CMS_Employee", "UpdatedDate");
            DropColumn("dbo.CMS_Employee", "CreatedBy");
            DropColumn("dbo.CMS_Employee", "IsActive");
            DropColumn("dbo.CMS_Employee", "BirthDate");
            DropColumn("dbo.CMS_Employee", "LastName");
            DropColumn("dbo.CMS_Employee", "FirstName");
            DropColumn("dbo.CMS_Employee", "Id");
            AddPrimaryKey("dbo.CMS_Employee", "Employee_ID");
        }
    }
}
