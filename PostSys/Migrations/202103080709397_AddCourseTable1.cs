namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Name", c => c.String());
        }
    }
}
