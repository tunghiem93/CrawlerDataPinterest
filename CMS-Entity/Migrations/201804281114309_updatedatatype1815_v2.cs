namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatatype1815_v2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CMS_Categories");
            CreateTable(
                "dbo.CMS_Employee",
                c => new
                    {
                        Employee_ID = c.String(nullable: false, maxLength: 128),
                        Employee_Name = c.String(nullable: false, maxLength: 50),
                        Employee_Address = c.String(),
                        Employee_Phone = c.String(nullable: false, maxLength: 11),
                        Employee_Email = c.String(),
                        Employee_IDCard = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Employee_ID);
            
            AlterColumn("dbo.CMS_Categories", "Id", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.CMS_Categories", "CategoryName", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.CMS_Categories", "CategoryCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CMS_Categories", "CreatedBy", c => c.String(maxLength: 60));
            AlterColumn("dbo.CMS_Categories", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.CMS_Categories", "UpdatedBy", c => c.String(maxLength: 60));
            AddPrimaryKey("dbo.CMS_Categories", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CMS_Categories");
            AlterColumn("dbo.CMS_Categories", "UpdatedBy", c => c.String());
            AlterColumn("dbo.CMS_Categories", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CMS_Categories", "CreatedBy", c => c.String());
            AlterColumn("dbo.CMS_Categories", "CategoryCode", c => c.String());
            AlterColumn("dbo.CMS_Categories", "CategoryName", c => c.String());
            AlterColumn("dbo.CMS_Categories", "Id", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.CMS_Employee");
            AddPrimaryKey("dbo.CMS_Categories", "Id");
        }
    }
}
