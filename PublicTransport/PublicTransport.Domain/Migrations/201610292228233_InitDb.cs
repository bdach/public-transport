namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Url = c.String(),
                        Regon = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Calendars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Monday = c.Boolean(nullable: false),
                        Tuesday = c.Boolean(nullable: false),
                        Wednesday = c.Boolean(nullable: false),
                        Thursday = c.Boolean(nullable: false),
                        Friday = c.Boolean(nullable: false),
                        Saturday = c.Boolean(nullable: false),
                        Sonday = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FareAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FareRuleId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transfers = c.Int(),
                        TransferDuration = c.Int(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FareRules", t => t.FareRuleId, cascadeDelete: true)
                .Index(t => t.FareRuleId);
            
            CreateTable(
                "dbo.FareRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        OriginId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zones", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.OriginId, cascadeDelete: false)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.OriginId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgencyId = c.Int(nullable: false),
                        ShortName = c.String(nullable: false),
                        LongName = c.String(nullable: false),
                        RouteType = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agencies", t => t.AgencyId, cascadeDelete: true)
                .Index(t => t.AgencyId);
            
            CreateTable(
                "dbo.Stops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StreetId = c.Int(nullable: false),
                        ZoneId = c.Int(),
                        ParentStationId = c.Int(),
                        IsStation = c.Boolean(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stops", t => t.ParentStationId)
                .ForeignKey("dbo.Streets", t => t.StreetId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId)
                .Index(t => t.StreetId)
                .Index(t => t.ZoneId)
                .Index(t => t.ParentStationId);
            
            CreateTable(
                "dbo.Streets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.StopTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        StopId = c.Int(nullable: false),
                        ArrivalTime = c.Time(nullable: false, precision: 7),
                        DepartureTime = c.Time(nullable: false, precision: 7),
                        StopSequence = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stops", t => t.StopId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.StopId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        Headsign = c.String(),
                        ShortName = c.String(),
                        Direction = c.Boolean(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.Calendars", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.CalendarCalendarDates",
                c => new
                    {
                        Calendar_Id = c.Int(nullable: false),
                        CalendarDate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Calendar_Id, t.CalendarDate_Id })
                .ForeignKey("dbo.Calendars", t => t.Calendar_Id, cascadeDelete: true)
                .ForeignKey("dbo.CalendarDates", t => t.CalendarDate_Id, cascadeDelete: true)
                .Index(t => t.Calendar_Id)
                .Index(t => t.CalendarDate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StopTimes", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Trips", "ServiceId", "dbo.Calendars");
            DropForeignKey("dbo.Trips", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.StopTimes", "StopId", "dbo.Stops");
            DropForeignKey("dbo.Stops", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.Stops", "StreetId", "dbo.Streets");
            DropForeignKey("dbo.Streets", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Stops", "ParentStationId", "dbo.Stops");
            DropForeignKey("dbo.FareAttributes", "FareRuleId", "dbo.FareRules");
            DropForeignKey("dbo.FareRules", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.Routes", "AgencyId", "dbo.Agencies");
            DropForeignKey("dbo.FareRules", "OriginId", "dbo.Zones");
            DropForeignKey("dbo.FareRules", "DestinationId", "dbo.Zones");
            DropForeignKey("dbo.CalendarCalendarDates", "CalendarDate_Id", "dbo.CalendarDates");
            DropForeignKey("dbo.CalendarCalendarDates", "Calendar_Id", "dbo.Calendars");
            DropIndex("dbo.CalendarCalendarDates", new[] { "CalendarDate_Id" });
            DropIndex("dbo.CalendarCalendarDates", new[] { "Calendar_Id" });
            DropIndex("dbo.Trips", new[] { "ServiceId" });
            DropIndex("dbo.Trips", new[] { "RouteId" });
            DropIndex("dbo.StopTimes", new[] { "StopId" });
            DropIndex("dbo.StopTimes", new[] { "TripId" });
            DropIndex("dbo.Streets", new[] { "CityId" });
            DropIndex("dbo.Stops", new[] { "ParentStationId" });
            DropIndex("dbo.Stops", new[] { "ZoneId" });
            DropIndex("dbo.Stops", new[] { "StreetId" });
            DropIndex("dbo.Routes", new[] { "AgencyId" });
            DropIndex("dbo.FareRules", new[] { "DestinationId" });
            DropIndex("dbo.FareRules", new[] { "OriginId" });
            DropIndex("dbo.FareRules", new[] { "RouteId" });
            DropIndex("dbo.FareAttributes", new[] { "FareRuleId" });
            DropTable("dbo.CalendarCalendarDates");
            DropTable("dbo.Trips");
            DropTable("dbo.StopTimes");
            DropTable("dbo.Streets");
            DropTable("dbo.Stops");
            DropTable("dbo.Routes");
            DropTable("dbo.Zones");
            DropTable("dbo.FareRules");
            DropTable("dbo.FareAttributes");
            DropTable("dbo.Cities");
            DropTable("dbo.Calendars");
            DropTable("dbo.CalendarDates");
            DropTable("dbo.Agencies");
        }
    }
}
