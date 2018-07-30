namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTblPin_addColBoardUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Pin", "BoardUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Pin", "BoardUrl");
        }
    }
}
