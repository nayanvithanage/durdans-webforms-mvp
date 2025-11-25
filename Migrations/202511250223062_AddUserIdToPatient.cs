namespace Durdans_WebForms_MVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToPatient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "UserId", c => c.Int());
            AddColumn("dbo.Users", "Patient_Id", c => c.Int());
            CreateIndex("dbo.Users", "Patient_Id");
            AddForeignKey("dbo.Users", "Patient_Id", "dbo.Patients", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Patient_Id", "dbo.Patients");
            DropIndex("dbo.Users", new[] { "Patient_Id" });
            DropColumn("dbo.Users", "Patient_Id");
            DropColumn("dbo.Patients", "UserId");
        }
    }
}
