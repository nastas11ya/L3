namespace L3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SportsmanId = c.Int(nullable: false),
                        DataOfIssue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sportsmen", t => t.SportsmanId, cascadeDelete: true)
                .Index(t => t.SportsmanId);
            
            CreateTable(
                "dbo.Sportsmen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SurName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscriptionActivities",
                c => new
                    {
                        Subscription_Id = c.Int(nullable: false),
                        Activity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subscription_Id, t.Activity_Id })
                .ForeignKey("dbo.Subscriptions", t => t.Subscription_Id, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.Activity_Id, cascadeDelete: true)
                .Index(t => t.Subscription_Id)
                .Index(t => t.Activity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "SportsmanId", "dbo.Sportsmen");
            DropForeignKey("dbo.SubscriptionActivities", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.SubscriptionActivities", "Subscription_Id", "dbo.Subscriptions");
            DropIndex("dbo.SubscriptionActivities", new[] { "Activity_Id" });
            DropIndex("dbo.SubscriptionActivities", new[] { "Subscription_Id" });
            DropIndex("dbo.Subscriptions", new[] { "SportsmanId" });
            DropTable("dbo.SubscriptionActivities");
            DropTable("dbo.Sportsmen");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Activities");
        }
    }
}
