namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedgenderinVideoCompetitor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCompetitors", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCompetitors", "Gender");
        }
    }
}
