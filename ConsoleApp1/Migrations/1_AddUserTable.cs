using FluentMigrator;

namespace ConsoleApp1.Migrations
{
    [Migration(1)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("username").AsString()
                .WithColumn("password").AsString()
                .WithColumn("is_active").AsBoolean();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}