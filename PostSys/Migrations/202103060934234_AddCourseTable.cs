namespace PostSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ClassId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.ClassId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ClassId", "dbo.Classes");
            DropIndex("dbo.Courses", new[] { "StudentId" });
            DropIndex("dbo.Courses", new[] { "ClassId" });
            DropTable("dbo.Courses");
        }
    }
}
