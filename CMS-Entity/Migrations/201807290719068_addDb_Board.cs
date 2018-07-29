namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDb_Board : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Board",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        BoardName = c.String(maxLength: 250),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CrawlAccountID = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_Account", t => t.CrawlAccountID)
                .Index(t => t.CrawlAccountID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_KeyWord", t => t.KeyWordID)
                .ForeignKey("dbo.CMS_Board", t => t.BoardID)
                .Index(t => t.BoardID)
                .Index(t => t.KeyWordID);
            
            CreateTable(
                "dbo.CMS_R_GroupBoard_Board",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        GroupBoardID = c.String(maxLength: 60, unicode: false),
                        BoardID = c.String(maxLength: 60, unicode: false),
                        Status = c.Int(),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CMS_GroupBoard", t => t.GroupBoardID)
                .ForeignKey("dbo.CMS_Board", t => t.BoardID)
                .Index(t => t.GroupBoardID)
                .Index(t => t.BoardID);
            
            CreateTable(
                "dbo.CMS_GroupBoard",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(maxLength: 10, fixedLength: true),
                        Status = c.Int(),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CMS_Account", "Name", c => c.String(maxLength: 250));
            AddColumn("dbo.CMS_Account", "State", c => c.Int());
            AddColumn("dbo.CMS_GroupKey", "CrawlAccountID", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.CMS_KeyWord", "CrawlAccountID", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.CMS_Account", "Status", c => c.Int());
            AlterColumn("dbo.CMS_Account", "Cookies", c => c.String(maxLength: 2000, unicode: false));
            CreateIndex("dbo.CMS_KeyWord", "CrawlAccountID");
            AddForeignKey("dbo.CMS_KeyWord", "CrawlAccountID", "dbo.CMS_Account", "ID");
            DropColumn("dbo.CMS_Account", "Sequence");
            DropColumn("dbo.CMS_Account", "Account");
            DropColumn("dbo.CMS_Account", "Password");
            DropColumn("dbo.CMS_Account", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CMS_Account", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CMS_Account", "Password", c => c.String(nullable: false, maxLength: 250, unicode: false));
            AddColumn("dbo.CMS_Account", "Account", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.CMS_Account", "Sequence", c => c.Int(nullable: false));
            DropForeignKey("dbo.CMS_KeyWord", "CrawlAccountID", "dbo.CMS_Account");
            DropForeignKey("dbo.CMS_Board", "CrawlAccountID", "dbo.CMS_Account");
            DropForeignKey("dbo.CMS_R_GroupBoard_Board", "BoardID", "dbo.CMS_Board");
            DropForeignKey("dbo.CMS_R_GroupBoard_Board", "GroupBoardID", "dbo.CMS_GroupBoard");
            DropForeignKey("dbo.CMS_R_Board_KeyWord", "BoardID", "dbo.CMS_Board");
            DropForeignKey("dbo.CMS_R_Board_KeyWord", "KeyWordID", "dbo.CMS_KeyWord");
            DropIndex("dbo.CMS_R_GroupBoard_Board", new[] { "BoardID" });
            DropIndex("dbo.CMS_R_GroupBoard_Board", new[] { "GroupBoardID" });
            DropIndex("dbo.CMS_KeyWord", new[] { "CrawlAccountID" });
            DropIndex("dbo.CMS_R_Board_KeyWord", new[] { "KeyWordID" });
            DropIndex("dbo.CMS_R_Board_KeyWord", new[] { "BoardID" });
            DropIndex("dbo.CMS_Board", new[] { "CrawlAccountID" });
            AlterColumn("dbo.CMS_Account", "Cookies", c => c.String(maxLength: 4000));
            AlterColumn("dbo.CMS_Account", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.CMS_KeyWord", "CrawlAccountID");
            DropColumn("dbo.CMS_GroupKey", "CrawlAccountID");
            DropColumn("dbo.CMS_Account", "State");
            DropColumn("dbo.CMS_Account", "Name");
            DropTable("dbo.CMS_GroupBoard");
            DropTable("dbo.CMS_R_GroupBoard_Board");
            DropTable("dbo.CMS_R_Board_KeyWord");
            DropTable("dbo.CMS_Board");
        }
    }
}
