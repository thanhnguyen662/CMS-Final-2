namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "EnrollmentKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "EnrollmentKey");
        }
    }
}
