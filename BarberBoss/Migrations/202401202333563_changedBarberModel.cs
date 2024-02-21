namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedBarberModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Barber_Id", "dbo.Barbers");
            DropIndex("dbo.AspNetUsers", new[] { "Barber_Id" });
            DropColumn("dbo.AspNetUsers", "Barber_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Barber_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Barber_Id");
            AddForeignKey("dbo.AspNetUsers", "Barber_Id", "dbo.Barbers", "Id");
        }
    }
}
