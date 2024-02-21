namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedReviewModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false),
                        Grade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Appointments", t => t.ReviewId)
                .Index(t => t.ReviewId);
            
            AlterColumn("dbo.Appointments", "ReviewGrade", c => c.Double());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "ReviewId", "dbo.Appointments");
            DropIndex("dbo.Reviews", new[] { "ReviewId" });
            AlterColumn("dbo.Appointments", "ReviewGrade", c => c.Double(nullable: false));
            DropTable("dbo.Reviews");
        }
    }
}
