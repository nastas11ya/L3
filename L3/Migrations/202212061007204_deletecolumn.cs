namespace L3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletecolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sportsmen", "Mother");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sportsmen", "Mother", c => c.String());
        }
    }
}
