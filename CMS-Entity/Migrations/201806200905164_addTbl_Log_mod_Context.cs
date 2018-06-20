namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTbl_Log_mod_Context : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Log",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Decription = c.String(maxLength: 100, unicode: false),
                        CreatedDate = c.DateTime(),
                        JsonContent = c.String(maxLength: 4000, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMS_Log");
        }
    }
}
