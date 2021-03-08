namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeadlineTable4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Deadlines", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deadlines", "Name", c => c.String());
        }
    }
}
