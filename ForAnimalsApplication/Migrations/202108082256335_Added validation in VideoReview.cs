namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedvalidationinVideoReview : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VideoReviews", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VideoReviews", "Text", c => c.String());
        }
    }
}
