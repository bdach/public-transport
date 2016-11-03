namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredCityName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String());
        }
    }
}
