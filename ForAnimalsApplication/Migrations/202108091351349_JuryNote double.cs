namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JuryNotedouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhotoCompetitors", "JuryNote", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoCompetitors", "JuryNote", c => c.Int(nullable: false));
        }
    }
}
