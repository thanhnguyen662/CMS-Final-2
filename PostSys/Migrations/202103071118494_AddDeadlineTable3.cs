namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeadlineTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Deadlines", "CoordinatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Deadlines", new[] { "CoordinatorId" });
            DropColumn("dbo.Deadlines", "CoordinatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deadlines", "CoordinatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Deadlines", "CoordinatorId");
            AddForeignKey("dbo.Deadlines", "CoordinatorId", "dbo.AspNetUsers", "Id");
        }
    }
}
