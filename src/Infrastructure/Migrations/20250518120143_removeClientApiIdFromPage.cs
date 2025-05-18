using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaffarXPartnerApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeClientApiIdFromPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientApiId",
                table: "Pages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientApiId",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
