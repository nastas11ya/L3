namespace L3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onemigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sportsmen", "Mother", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sportsmen", "Mother");
        }
    }
}
