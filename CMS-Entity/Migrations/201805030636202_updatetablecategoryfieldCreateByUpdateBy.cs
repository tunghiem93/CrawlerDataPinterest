namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetablecategoryfieldCreateByUpdateBy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Categories", "CreatedBy", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.CMS_Categories", "UpdatedBy", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Categories", "UpdatedBy", c => c.String(maxLength: 60));
            AlterColumn("dbo.CMS_Categories", "CreatedBy", c => c.String(maxLength: 60));
        }
    }
}
