namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedvalidationinCompetitionType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompetitionTypes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompetitionTypes", "Name", c => c.String());
        }
    }
}
