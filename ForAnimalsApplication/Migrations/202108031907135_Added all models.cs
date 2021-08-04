namespace ForAnimalsApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedallmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        CompetitionId = c.Int(nullable: false, identity: true),
                        CompetitionName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        CompetitionTypeId = c.Int(nullable: false),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.CompetitionId)
                .ForeignKey("dbo.CompetitionTypes", t => t.CompetitionTypeId, cascadeDelete: true)
                .Index(t => t.CompetitionTypeId);
            
            CreateTable(
                "dbo.PhotoCompetitors",
                c => new
                    {
                        PhotoCompetitorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        JuryNote = c.Int(nullable: false),
                        FinalNote = c.Double(nullable: false),
                        ImagePath = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        CompetitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoCompetitorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.PhotoReviews",
                c => new
                    {
                        PhotoReviewId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        ReviewDate = c.DateTime(nullable: false),
                        Note = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        PhotoCompetitorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoReviewId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.PhotoCompetitors", t => t.PhotoCompetitorId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.PhotoCompetitorId);
            
            CreateTable(
                "dbo.VideoCompetitors",
                c => new
                    {
                        VideoCompetitorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        JuryNote = c.Int(nullable: false),
                        FinalNote = c.Double(nullable: false),
                        Vname = c.String(),
                        Vpath = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        CompetitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoCompetitorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.VideoReviews",
                c => new
                    {
                        VideoReviewId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        ReviewDate = c.DateTime(nullable: false),
                        Note = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        VideoCompetitorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoReviewId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.VideoCompetitors", t => t.VideoCompetitorId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.VideoCompetitorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoReviews", "VideoCompetitorId", "dbo.VideoCompetitors");
            DropForeignKey("dbo.VideoReviews", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.VideoCompetitors", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.VideoCompetitors", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.PhotoReviews", "PhotoCompetitorId", "dbo.PhotoCompetitors");
            DropForeignKey("dbo.PhotoReviews", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.PhotoCompetitors", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.PhotoCompetitors", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes");
            DropIndex("dbo.VideoReviews", new[] { "VideoCompetitorId" });
            DropIndex("dbo.VideoReviews", new[] { "ApplicationUserID" });
            DropIndex("dbo.VideoCompetitors", new[] { "CompetitionId" });
            DropIndex("dbo.VideoCompetitors", new[] { "ApplicationUserID" });
            DropIndex("dbo.PhotoReviews", new[] { "PhotoCompetitorId" });
            DropIndex("dbo.PhotoReviews", new[] { "ApplicationUserID" });
            DropIndex("dbo.PhotoCompetitors", new[] { "CompetitionId" });
            DropIndex("dbo.PhotoCompetitors", new[] { "ApplicationUserID" });
            DropIndex("dbo.Competitions", new[] { "CompetitionTypeId" });
            DropTable("dbo.VideoReviews");
            DropTable("dbo.VideoCompetitors");
            DropTable("dbo.PhotoReviews");
            DropTable("dbo.PhotoCompetitors");
            DropTable("dbo.Competitions");
        }
    }
}
