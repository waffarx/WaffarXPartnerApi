using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaffarXPartnerApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationBetweenPageActionandTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPageActions_Pages_PageId",
                table: "TeamPageActions");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "TeamPageActions",
                newName: "PageActionId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPageAction_PageId",
                table: "TeamPageActions",
                newName: "IX_TeamPageAction_PageActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPageActions_PageActions_PageActionId",
                table: "TeamPageActions",
                column: "PageActionId",
                principalTable: "PageActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPageActions_PageActions_PageActionId",
                table: "TeamPageActions");

            migrationBuilder.RenameColumn(
                name: "PageActionId",
                table: "TeamPageActions",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPageAction_PageActionId",
                table: "TeamPageActions",
                newName: "IX_TeamPageAction_PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPageActions_Pages_PageId",
                table: "TeamPageActions",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
