namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotrequiredforCompetitionTypeandImagePath : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competitions", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Competitions", "ImagePath", c => c.String(nullable: false));
        }
    }
}
