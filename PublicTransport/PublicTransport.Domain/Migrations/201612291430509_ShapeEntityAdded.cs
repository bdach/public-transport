namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShapeEntityAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shapes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Longtitude = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Identifier = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.StopTimes", "ShapeId", c => c.Int());
            CreateIndex("dbo.StopTimes", "ShapeId");
            AddForeignKey("dbo.StopTimes", "ShapeId", "dbo.Shapes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StopTimes", "ShapeId", "dbo.Shapes");
            DropIndex("dbo.StopTimes", new[] { "ShapeId" });
            DropColumn("dbo.StopTimes", "ShapeId");
            DropTable("dbo.Shapes");
        }
    }
}
