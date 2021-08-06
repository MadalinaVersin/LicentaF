namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedfieldsinPhotoCompetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhotoCompetitors", "MicrochipNumber", c => c.String());
            AddColumn("dbo.PhotoCompetitors", "Winner", c => c.Boolean(nullable: false));
            AddColumn("dbo.PhotoCompetitors", "Age", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhotoCompetitors", "Age");
            DropColumn("dbo.PhotoCompetitors", "Winner");
            DropColumn("dbo.PhotoCompetitors", "MicrochipNumber");
        }
    }
}
