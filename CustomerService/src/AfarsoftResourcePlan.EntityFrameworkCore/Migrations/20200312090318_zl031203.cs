using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AfarsoftResourcePlan.Migrations
{
    public partial class zl031203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceRecordsId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    CustomerDeviceId = table.Column<string>(nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    CustomerNickName = table.Column<string>(nullable: true),
                    CustomerFaceImg = table.Column<string>(nullable: true),
                    ServiceId = table.Column<Guid>(nullable: false),
                    ServiceCode = table.Column<string>(nullable: true),
                    ServiceNickName = table.Column<string>(nullable: true),
                    ServiceFaceImg = table.Column<string>(nullable: true),
                    SendDateTime = table.Column<DateTime>(nullable: false),
                    SendInfoType = table.Column<int>(nullable: false),
                    SendContent = table.Column<string>(nullable: true),
                    SendSource = table.Column<int>(nullable: false),
                    ReceiveState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerConnectRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false),
                    OpenId = table.Column<string>(nullable: true),
                    UnionId = table.Column<string>(nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    CustomerNickName = table.Column<string>(nullable: true),
                    CustomerFaceImg = table.Column<string>(nullable: true),
                    CustomerState = table.Column<int>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerConnectRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerConnectRecordsId = table.Column<int>(nullable: false),
                    ServiceConnectRecordsId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false),
                    CustomerDeviceId = table.Column<string>(nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    CustomerNickName = table.Column<string>(nullable: true),
                    CustomerFaceImg = table.Column<string>(nullable: true),
                    CustomerContentDate = table.Column<DateTime>(nullable: true),
                    CustomerUnContentDate = table.Column<DateTime>(nullable: true),
                    CustomerState = table.Column<int>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    ServiceCode = table.Column<string>(nullable: true),
                    ServiceNickName = table.Column<string>(nullable: true),
                    ServiceFaceImg = table.Column<string>(nullable: true),
                    ServiceContentDate = table.Column<DateTime>(nullable: true),
                    ServiceUnContentDate = table.Column<DateTime>(nullable: true),
                    ServiceState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRecords");

            migrationBuilder.DropTable(
                name: "CustomerConnectRecords");

            migrationBuilder.DropTable(
                name: "ServiceRecords");
        }
    }
}
