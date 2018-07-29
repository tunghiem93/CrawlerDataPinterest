namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editPin_addColBoard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Pin", "BoardID", c => c.String(maxLength: 60));
            AddColumn("dbo.CMS_Pin", "BoardName", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Pin", "BoardName");
            DropColumn("dbo.CMS_Pin", "BoardID");
        }
    }
}
