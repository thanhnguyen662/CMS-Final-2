namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Description", c => c.String());
        }
    }
}
