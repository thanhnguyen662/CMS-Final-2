namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeadlineTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deadlines", "CoordinatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Deadlines", "CoordinatorId");
            AddForeignKey("dbo.Deadlines", "CoordinatorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deadlines", "CoordinatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Deadlines", new[] { "CoordinatorId" });
            DropColumn("dbo.Deadlines", "CoordinatorId");
        }
    }
}
