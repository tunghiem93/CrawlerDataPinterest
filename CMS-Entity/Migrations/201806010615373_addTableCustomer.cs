namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        CompanyName = c.String(maxLength: 250),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(),
                        Password = c.String(nullable: false, maxLength: 250, unicode: false),
                        Phone = c.String(maxLength: 15, unicode: false),
                        BirthDate = c.String(),
                        Gender = c.Boolean(nullable: false),
                        Address = c.String(nullable: false, maxLength: 250),
                        MaritalStatus = c.Boolean(nullable: false),
                        Street = c.String(maxLength: 250),
                        City = c.String(),
                        Country = c.String(),
                        Description = c.String(),
                        ImageURL = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMS_Customers");
        }
    }
}
