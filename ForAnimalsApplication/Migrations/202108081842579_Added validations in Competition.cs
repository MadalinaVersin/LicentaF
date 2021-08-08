namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedvalidationsinCompetition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competitions", "CompetitionName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Competitions", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Competitions", "ImagePath", c => c.String());
            AlterColumn("dbo.Competitions", "CompetitionName", c => c.String());
        }
    }
}
