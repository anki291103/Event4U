namespace Event4U.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserBookingRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "EventName", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "Contact", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "Email", c => c.String());
            AlterColumn("dbo.Bookings", "Contact", c => c.String());
            AlterColumn("dbo.Bookings", "FullName", c => c.String());
            AlterColumn("dbo.Bookings", "EventName", c => c.String());
        }
    }
}
