namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedfieldsinVideoCompetitor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCompetitors", "MicrochipNumber", c => c.String());
            AddColumn("dbo.VideoCompetitors", "Winner", c => c.Boolean(nullable: false));
            AddColumn("dbo.VideoCompetitors", "Age", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCompetitors", "Age");
            DropColumn("dbo.VideoCompetitors", "Winner");
            DropColumn("dbo.VideoCompetitors", "MicrochipNumber");
        }
    }
}
