using Microsoft.EntityFrameworkCore.Migrations;

namespace AfarsoftResourcePlan.Migrations
{
    public partial class zl032501 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GetCodeUrl",
                table: "OauthSetting",
                newName: "GetAccessTokenUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GetAccessTokenUrl",
                table: "OauthSetting",
                newName: "GetCodeUrl");
        }
    }
}
