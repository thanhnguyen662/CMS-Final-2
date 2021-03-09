namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "EnrollmentKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "EnrollmentKey");
        }
    }
}
