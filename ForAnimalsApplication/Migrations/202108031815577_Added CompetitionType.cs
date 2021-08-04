namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompetitionType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompetitionTypes",
                c => new
                    {
                        CompetitionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CompetitionTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompetitionTypes");
        }
    }
}
