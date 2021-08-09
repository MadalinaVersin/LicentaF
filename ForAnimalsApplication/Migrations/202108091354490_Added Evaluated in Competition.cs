namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEvaluatedinCompetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "Evaluated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "Evaluated");
        }
    }
}
