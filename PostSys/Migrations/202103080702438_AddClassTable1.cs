namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "Name", c => c.String());
        }
    }
}
