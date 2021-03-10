namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuestZoneTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GuestZones", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GuestZones", "Name");
        }
    }
}
