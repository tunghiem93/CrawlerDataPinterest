namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTblAcc_editTblBoard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Account", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CMS_Board", "OwnerName", c => c.String());
            AddColumn("dbo.CMS_Board", "Description", c => c.String());
            AddColumn("dbo.CMS_Board", "Type", c => c.String());
            AddColumn("dbo.CMS_Board", "Pin_count", c => c.Int());
            AddColumn("dbo.CMS_Board", "Sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CMS_Board", "Sequence");
            DropColumn("dbo.CMS_Board", "Pin_count");
            DropColumn("dbo.CMS_Board", "Type");
            DropColumn("dbo.CMS_Board", "Description");
            DropColumn("dbo.CMS_Board", "OwnerName");
            DropColumn("dbo.CMS_Account", "IsActive");
        }
    }
}
