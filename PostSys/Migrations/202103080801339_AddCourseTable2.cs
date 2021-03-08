namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Name");
        }
    }
}
