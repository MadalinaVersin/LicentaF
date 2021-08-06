namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeAgefield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VideoCompetitors", "Age", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VideoCompetitors", "Age", c => c.Int(nullable: false));
        }
    }
}
