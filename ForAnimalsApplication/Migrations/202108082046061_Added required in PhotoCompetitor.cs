namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedrequiredinPhotoCompetitor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhotoCompetitors", "Age", c => c.String(nullable: false));
            AlterColumn("dbo.PhotoCompetitors", "Gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoCompetitors", "Gender", c => c.String());
            AlterColumn("dbo.PhotoCompetitors", "Age", c => c.String());
        }
    }
}
