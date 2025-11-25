namespace Durdans_WebForms_MVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSpecialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Doctors", "SpecializationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Doctors", "SpecializationId");
            AddForeignKey("dbo.Doctors", "SpecializationId", "dbo.Specializations", "Id");
            DropColumn("dbo.Doctors", "Specialization");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "Specialization", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.Doctors", "SpecializationId", "dbo.Specializations");
            DropIndex("dbo.Doctors", new[] { "SpecializationId" });
            DropColumn("dbo.Doctors", "SpecializationId");
            DropTable("dbo.Specializations");
        }
    }
}
