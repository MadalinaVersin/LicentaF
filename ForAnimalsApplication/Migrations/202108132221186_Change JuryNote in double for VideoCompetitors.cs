namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeJuryNoteindoubleforVideoCompetitors : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VideoCompetitors", "JuryNote", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VideoCompetitors", "JuryNote", c => c.Int(nullable: false));
        }
    }
}
