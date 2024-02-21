namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedPartialAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartialAppointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        startTime = c.DateTime(nullable: false),
                        startTimeString = c.String(),
                        bookTime = c.DateTime(nullable: false),
                        bookTimeString = c.String(),
                        OfferId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        BarberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PartialAppointments");
        }
    }
}
