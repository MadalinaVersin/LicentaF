namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedtestrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhotoCompetitors", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoCompetitors", "Name", c => c.String());
        }
    }
}
