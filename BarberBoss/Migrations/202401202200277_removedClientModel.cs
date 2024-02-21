namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedClientModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientBarbers", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientBarbers", "Barber_Id", "dbo.Barbers");
            DropIndex("dbo.Appointments", new[] { "ClientId" });
            DropIndex("dbo.ClientBarbers", new[] { "Client_Id" });
            DropIndex("dbo.ClientBarbers", new[] { "Barber_Id" });
            AddColumn("dbo.PartialAppointments", "UserId", c => c.String());
            DropColumn("dbo.Appointments", "ClientId");
            DropColumn("dbo.PartialAppointments", "ClientId");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientBarbers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClientBarbers",
                c => new
                    {
                        Client_Id = c.Int(nullable: false),
                        Barber_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Client_Id, t.Barber_Id });
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PartialAppointments", "ClientId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "ClientId", c => c.Int(nullable: false));
            DropColumn("dbo.PartialAppointments", "UserId");
            CreateIndex("dbo.ClientBarbers", "Barber_Id");
            CreateIndex("dbo.ClientBarbers", "Client_Id");
            CreateIndex("dbo.Appointments", "ClientId");
            AddForeignKey("dbo.ClientBarbers", "Barber_Id", "dbo.Barbers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientBarbers", "Client_Id", "dbo.Clients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Appointments", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
    }
}
