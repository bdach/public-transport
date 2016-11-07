namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredTransferCount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FareAttributes", "Transfers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FareAttributes", "Transfers", c => c.Int());
        }
    }
}
