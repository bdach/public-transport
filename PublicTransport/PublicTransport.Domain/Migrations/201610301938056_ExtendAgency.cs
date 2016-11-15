namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendAgency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agencies", "StreetId", c => c.Int());
            AddColumn("dbo.Agencies", "StreetNumber", c => c.String());
            AddColumn("dbo.Calendars", "Sunday", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Agencies", "StreetId");
            AddForeignKey("dbo.Agencies", "StreetId", "dbo.Streets", "Id");
            DropColumn("dbo.Calendars", "Sonday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Calendars", "Sonday", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Agencies", "StreetId", "dbo.Streets");
            DropIndex("dbo.Agencies", new[] { "StreetId" });
            DropColumn("dbo.Calendars", "Sunday");
            DropColumn("dbo.Agencies", "StreetNumber");
            DropColumn("dbo.Agencies", "StreetId");
        }
    }
}
