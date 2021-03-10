namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuestZoneTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GuestZones", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GuestZones", "Name", c => c.String());
        }
    }
}
