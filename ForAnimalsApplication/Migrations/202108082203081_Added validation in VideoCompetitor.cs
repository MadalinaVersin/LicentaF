namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedvalidationinVideoCompetitor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VideoCompetitors", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.VideoCompetitors", "Age", c => c.String(nullable: false));
            AlterColumn("dbo.VideoCompetitors", "Gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VideoCompetitors", "Gender", c => c.String());
            AlterColumn("dbo.VideoCompetitors", "Age", c => c.String());
            AlterColumn("dbo.VideoCompetitors", "Name", c => c.String());
        }
    }
}
