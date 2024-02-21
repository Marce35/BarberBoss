namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        StartTimeString = c.String(),
                        BookTime = c.DateTime(nullable: false),
                        BookTimeString = c.String(),
                        IsBooked = c.Boolean(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        EndTimeString = c.String(),
                        ClientId = c.Int(nullable: false),
                        BarberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barbers", t => t.BarberId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.BarberId);
            
            CreateTable(
                "dbo.Barbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        InstagramProfile = c.String(),
                        FacebookProfile = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        Duration = c.Double(nullable: false),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ClientBarbers",
                c => new
                    {
                        Client_Id = c.Int(nullable: false),
                        Barber_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Client_Id, t.Barber_Id })
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Barbers", t => t.Barber_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Barber_Id);
            
            CreateTable(
                "dbo.ServiceOfferAppointments",
                c => new
                    {
                        ServiceOffer_Id = c.Int(nullable: false),
                        Appointment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceOffer_Id, t.Appointment_Id })
                .ForeignKey("dbo.ServiceOffers", t => t.ServiceOffer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Appointments", t => t.Appointment_Id, cascadeDelete: true)
                .Index(t => t.ServiceOffer_Id)
                .Index(t => t.Appointment_Id);
            
            CreateTable(
                "dbo.ServiceOfferBarbers",
                c => new
                    {
                        ServiceOffer_Id = c.Int(nullable: false),
                        Barber_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceOffer_Id, t.Barber_Id })
                .ForeignKey("dbo.ServiceOffers", t => t.ServiceOffer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Barbers", t => t.Barber_Id, cascadeDelete: true)
                .Index(t => t.ServiceOffer_Id)
                .Index(t => t.Barber_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ServiceOfferBarbers", "Barber_Id", "dbo.Barbers");
            DropForeignKey("dbo.ServiceOfferBarbers", "ServiceOffer_Id", "dbo.ServiceOffers");
            DropForeignKey("dbo.ServiceOfferAppointments", "Appointment_Id", "dbo.Appointments");
            DropForeignKey("dbo.ServiceOfferAppointments", "ServiceOffer_Id", "dbo.ServiceOffers");
            DropForeignKey("dbo.ClientBarbers", "Barber_Id", "dbo.Barbers");
            DropForeignKey("dbo.ClientBarbers", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Appointments", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Appointments", "BarberId", "dbo.Barbers");
            DropIndex("dbo.ServiceOfferBarbers", new[] { "Barber_Id" });
            DropIndex("dbo.ServiceOfferBarbers", new[] { "ServiceOffer_Id" });
            DropIndex("dbo.ServiceOfferAppointments", new[] { "Appointment_Id" });
            DropIndex("dbo.ServiceOfferAppointments", new[] { "ServiceOffer_Id" });
            DropIndex("dbo.ClientBarbers", new[] { "Barber_Id" });
            DropIndex("dbo.ClientBarbers", new[] { "Client_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Appointments", new[] { "BarberId" });
            DropIndex("dbo.Appointments", new[] { "ClientId" });
            DropTable("dbo.ServiceOfferBarbers");
            DropTable("dbo.ServiceOfferAppointments");
            DropTable("dbo.ClientBarbers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ServiceOffers");
            DropTable("dbo.Clients");
            DropTable("dbo.Barbers");
            DropTable("dbo.Appointments");
        }
    }
}
