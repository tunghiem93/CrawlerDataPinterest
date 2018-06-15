namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablePin_editCol_ImageUrl_length2000 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CMS_Pin", "ImageUrl", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CMS_Pin", "ImageUrl", c => c.String(maxLength: 256));
        }
    }
}
