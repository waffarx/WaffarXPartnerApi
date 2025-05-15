using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaffarXPartnerApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPartnerPostback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartnerPostbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClientApiId = table.Column<int>(type: "int", nullable: false),
                    PostbackUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PostbackType = table.Column<int>(type: "int", nullable: false),
                    PostbackMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerPostbacks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerPostbacks");
        }
    }
}
