using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaffarXPartnerApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPageBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperAdminPage",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperAdminPage",
                table: "Pages");
        }
    }
}
