namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullValuesToBasket : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Baskets", "PaymentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Baskets", "PaymentDate", c => c.DateTime(nullable: false));
        }
    }
}
