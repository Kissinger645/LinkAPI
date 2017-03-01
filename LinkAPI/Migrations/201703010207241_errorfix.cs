namespace LinkAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class errorfix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        ShortUrl = c.String(),
                        Created = c.DateTime(nullable: false),
                        Public = c.Boolean(nullable: false),
                        UserName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserName)
                .Index(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Links", "UserName", "dbo.AspNetUsers");
            DropIndex("dbo.Links", new[] { "UserName" });
            DropTable("dbo.Links");
        }
    }
}
