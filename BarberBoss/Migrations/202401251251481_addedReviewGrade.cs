namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedReviewGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "ReviewGrade", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "ReviewGrade");
        }
    }
}
