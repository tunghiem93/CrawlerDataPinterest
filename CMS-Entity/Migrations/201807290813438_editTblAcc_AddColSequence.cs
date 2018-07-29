namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTblAcc_AddColSequence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Account", "Sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Account", "Sequence");
        }
    }
}
