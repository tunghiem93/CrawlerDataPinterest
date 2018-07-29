namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTblGroupBoard_addColSeq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_GroupBoard", "Sequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_GroupBoard", "Sequence");
        }
    }
}
