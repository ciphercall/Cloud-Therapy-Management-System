namespace AS_Therapy_GL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_PCalendarImage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Long(nullable: false),
                        Month = c.String(nullable: false, maxLength: 128),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.Year, t.Month });
            
            CreateTable(
                "dbo.ASL_SchedularCalendar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        USERID = c.Long(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.COMPID, t.USERID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ASL_SchedularCalendar");
            DropTable("dbo.ASL_PCalendarImage");
        }
    }
}
