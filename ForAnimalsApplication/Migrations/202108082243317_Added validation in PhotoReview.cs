namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedvalidationinPhotoReview : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhotoReviews", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoReviews", "Text", c => c.String());
        }
    }
}
