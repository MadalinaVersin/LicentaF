namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGenderinPhotoCompetitor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhotoCompetitors", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhotoCompetitors", "Gender");
        }
    }
}
