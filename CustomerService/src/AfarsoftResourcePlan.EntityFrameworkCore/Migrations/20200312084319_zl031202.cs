using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AfarsoftResourcePlan.Migrations
{
    public partial class zl031202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceConnectRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<string>(nullable: true),
                    ServiceId = table.Column<Guid>(nullable: false),
                    ServiceCode = table.Column<string>(nullable: true),
                    ServiceNickName = table.Column<string>(nullable: true),
                    ServiceCount = table.Column<int>(nullable: false),
                    ServiceState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceConnectRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceConnectRecords");
        }
    }
}
