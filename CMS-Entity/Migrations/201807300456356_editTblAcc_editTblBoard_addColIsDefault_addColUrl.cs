namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTblAcc_editTblBoard_addColIsDefault_addColUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CMS_Account", "IsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.CMS_Board", "Url", c => c.String());
            AlterColumn("dbo.CMS_Board", "BoardName", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CMS_Pin", "BoardName", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Pin", "BoardName", c => c.String(maxLength: 250));
            AlterColumn("dbo.CMS_Board", "BoardName", c => c.String(maxLength: 250));
            DropColumn("dbo.CMS_Board", "Url");
            DropColumn("dbo.CMS_Account", "IsDefault");
        }
    }
}
