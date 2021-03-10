namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuestZoneTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuestZones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GuestId = c.String(maxLength: 128),
                        FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculties", t => t.FacultyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.GuestId)
                .Index(t => t.GuestId)
                .Index(t => t.FacultyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GuestZones", "GuestId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GuestZones", "FacultyId", "dbo.Faculties");
            DropIndex("dbo.GuestZones", new[] { "FacultyId" });
            DropIndex("dbo.GuestZones", new[] { "GuestId" });
            DropTable("dbo.GuestZones");
        }
    }
}
