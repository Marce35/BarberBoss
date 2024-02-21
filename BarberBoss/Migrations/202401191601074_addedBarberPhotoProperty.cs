namespace BarberBoss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedBarberPhotoProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Barbers", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Barbers", "ImagePath");
        }
    }
}
