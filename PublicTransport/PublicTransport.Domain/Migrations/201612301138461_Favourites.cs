namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Favourites : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRoles", newName: "RoleUsers");
            DropPrimaryKey("dbo.RoleUsers");
            CreateTable(
                "dbo.UserRoutes",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Route_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Route_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Route_Id);
            
            CreateTable(
                "dbo.StopUsers",
                c => new
                    {
                        Stop_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stop_Id, t.User_Id })
                .ForeignKey("dbo.Stops", t => t.Stop_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Stop_Id)
                .Index(t => t.User_Id);
            
            AddPrimaryKey("dbo.RoleUsers", new[] { "Role_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StopUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.StopUsers", "Stop_Id", "dbo.Stops");
            DropForeignKey("dbo.UserRoutes", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.UserRoutes", "User_Id", "dbo.Users");
            DropIndex("dbo.StopUsers", new[] { "User_Id" });
            DropIndex("dbo.StopUsers", new[] { "Stop_Id" });
            DropIndex("dbo.UserRoutes", new[] { "Route_Id" });
            DropIndex("dbo.UserRoutes", new[] { "User_Id" });
            DropPrimaryKey("dbo.RoleUsers");
            DropTable("dbo.StopUsers");
            DropTable("dbo.UserRoutes");
            AddPrimaryKey("dbo.RoleUsers", new[] { "User_Id", "Role_Id" });
            RenameTable(name: "dbo.RoleUsers", newName: "UserRoles");
        }
    }
}
