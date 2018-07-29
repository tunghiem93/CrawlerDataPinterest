namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTbleName_TblBoardKeyWord_tblBoardPin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CMS_R_Board_KeyWord", "KeyWordID", "dbo.CMS_KeyWord");
            DropForeignKey("dbo.CMS_R_Board_KeyWord", "BoardID", "dbo.CMS_Board");
            DropIndex("dbo.CMS_R_Board_KeyWord", new[] { "BoardID" });
            DropIndex("dbo.CMS_R_Board_KeyWord", new[] { "KeyWordID" });
            CreateTable(
                "dbo.CMS_R_Board_Pin",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        BoardID = c.String(nullable: false, maxLength: 60, unicode: false),
                        PinID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_Pin", t => t.PinID)
                .ForeignKey("dbo.CMS_Board", t => t.BoardID)
                .Index(t => t.BoardID)
                .Index(t => t.PinID);
            
            DropTable("dbo.CMS_R_Board_KeyWord");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CMS_R_Board_KeyWord",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        BoardID = c.String(nullable: false, maxLength: 60, unicode: false),
                        KeyWordID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.CMS_R_Board_Pin", "BoardID", "dbo.CMS_Board");
            DropForeignKey("dbo.CMS_R_Board_Pin", "PinID", "dbo.CMS_Pin");
            DropIndex("dbo.CMS_R_Board_Pin", new[] { "PinID" });
            DropIndex("dbo.CMS_R_Board_Pin", new[] { "BoardID" });
            DropTable("dbo.CMS_R_Board_Pin");
            CreateIndex("dbo.CMS_R_Board_KeyWord", "KeyWordID");
            CreateIndex("dbo.CMS_R_Board_KeyWord", "BoardID");
            AddForeignKey("dbo.CMS_R_Board_KeyWord", "BoardID", "dbo.CMS_Board", "ID");
            AddForeignKey("dbo.CMS_R_Board_KeyWord", "KeyWordID", "dbo.CMS_KeyWord", "ID");
        }
    }
}
