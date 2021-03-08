namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "Name");
        }
    }
}
