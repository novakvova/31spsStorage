namespace TestStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtblUsersandtblUserImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblUserImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 75),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tblUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 75),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Age = c.Int(nullable: false),
                        Sex = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblUserImages", "UserId", "dbo.tblUsers");
            DropIndex("dbo.tblUserImages", new[] { "UserId" });
            DropTable("dbo.tblUsers");
            DropTable("dbo.tblUserImages");
        }
    }
}
