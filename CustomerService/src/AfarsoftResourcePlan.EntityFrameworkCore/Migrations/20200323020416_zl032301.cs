using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AfarsoftResourcePlan.Migrations
{
    public partial class zl032301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OauthSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ThirdPlatName = table.Column<string>(nullable: true),
                    ThirdPlatCode = table.Column<string>(nullable: true),
                    GetCodeUrl = table.Column<string>(nullable: true),
                    RedirectUrl = table.Column<string>(nullable: true),
                    AppId = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OauthSetting", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OauthSetting");
        }
    }
}
