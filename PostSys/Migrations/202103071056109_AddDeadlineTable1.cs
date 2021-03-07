namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeadlineTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deadlines", "CreateBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deadlines", "CreateBy");
        }
    }
}
