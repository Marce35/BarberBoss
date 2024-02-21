namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedAppointmentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceOfferAppointments", "ServiceOffer_Id", "dbo.ServiceOffers");
            DropForeignKey("dbo.ServiceOfferAppointments", "Appointment_Id", "dbo.Appointments");
            DropIndex("dbo.ServiceOfferAppointments", new[] { "ServiceOffer_Id" });
            DropIndex("dbo.ServiceOfferAppointments", new[] { "Appointment_Id" });
            AddColumn("dbo.Appointments", "ServiceOffer_Id", c => c.Int());
            CreateIndex("dbo.Appointments", "ServiceOffer_Id");
            AddForeignKey("dbo.Appointments", "ServiceOffer_Id", "dbo.ServiceOffers", "Id");
            DropColumn("dbo.Appointments", "EndTimeString");
            DropTable("dbo.ServiceOfferAppointments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceOfferAppointments",
                c => new
                    {
                        ServiceOffer_Id = c.Int(nullable: false),
                        Appointment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceOffer_Id, t.Appointment_Id });
            
            AddColumn("dbo.Appointments", "EndTimeString", c => c.String());
            DropForeignKey("dbo.Appointments", "ServiceOffer_Id", "dbo.ServiceOffers");
            DropIndex("dbo.Appointments", new[] { "ServiceOffer_Id" });
            DropColumn("dbo.Appointments", "ServiceOffer_Id");
            CreateIndex("dbo.ServiceOfferAppointments", "Appointment_Id");
            CreateIndex("dbo.ServiceOfferAppointments", "ServiceOffer_Id");
            AddForeignKey("dbo.ServiceOfferAppointments", "Appointment_Id", "dbo.Appointments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceOfferAppointments", "ServiceOffer_Id", "dbo.ServiceOffers", "Id", cascadeDelete: true);
        }
    }
}
