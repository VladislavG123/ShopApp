namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Amount = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductBaskets",
                c => new
                    {
                        Product_Id = c.Guid(nullable: false),
                        Basket_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Basket_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Baskets", t => t.Basket_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Basket_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Baskets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProductBaskets", "Basket_Id", "dbo.Baskets");
            DropForeignKey("dbo.ProductBaskets", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductBaskets", new[] { "Basket_Id" });
            DropIndex("dbo.ProductBaskets", new[] { "Product_Id" });
            DropIndex("dbo.Baskets", new[] { "User_Id" });
            DropTable("dbo.ProductBaskets");
            DropTable("dbo.Users");
            DropTable("dbo.Products");
            DropTable("dbo.Baskets");
        }
    }
}
