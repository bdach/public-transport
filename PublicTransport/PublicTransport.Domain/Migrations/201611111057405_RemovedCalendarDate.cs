namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCalendarDate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CalendarCalendarDates", "Calendar_Id", "dbo.Calendars");
            DropForeignKey("dbo.CalendarCalendarDates", "CalendarDate_Id", "dbo.CalendarDates");
            DropIndex("dbo.CalendarCalendarDates", new[] { "Calendar_Id" });
            DropIndex("dbo.CalendarCalendarDates", new[] { "CalendarDate_Id" });
            DropTable("dbo.CalendarDates");
            DropTable("dbo.CalendarCalendarDates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CalendarCalendarDates",
                c => new
                    {
                        Calendar_Id = c.Int(nullable: false),
                        CalendarDate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Calendar_Id, t.CalendarDate_Id });
            
            CreateTable(
                "dbo.CalendarDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ExceptionType = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CalendarCalendarDates", "CalendarDate_Id");
            CreateIndex("dbo.CalendarCalendarDates", "Calendar_Id");
            AddForeignKey("dbo.CalendarCalendarDates", "CalendarDate_Id", "dbo.CalendarDates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CalendarCalendarDates", "Calendar_Id", "dbo.Calendars", "Id", cascadeDelete: true);
        }
    }
}
