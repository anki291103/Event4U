namespace Event4U.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalAmountToPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "TotalAmount");
        }
    }
}
