namespace PublicTransport.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFullNameAndToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FullName", c => c.String());
            AddColumn("dbo.Users", "LatestToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LatestToken");
            DropColumn("dbo.Users", "FullName");
        }
    }
}
