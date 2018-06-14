namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCol_Sequece_TableKeyWord_GroupKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_GroupKey", "Sequence", c => c.Int(nullable: false));
            AddColumn("dbo.CMS_KeyWord", "Sequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_KeyWord", "Sequence");
            DropColumn("dbo.CMS_GroupKey", "Sequence");
        }
    }
}
