using Microsoft.EntityFrameworkCore.Migrations;

namespace belt.Migrations
{
    public partial class secMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_occasions_users_CoordinatorId",
                table: "occasions");

            migrationBuilder.DropIndex(
                name: "IX_occasions_CoordinatorId",
                table: "occasions");

            migrationBuilder.AddColumn<int>(
                name: "CoordinatorUserUserId",
                table: "occasions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_occasions_CoordinatorUserUserId",
                table: "occasions",
                column: "CoordinatorUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_occasions_users_CoordinatorUserUserId",
                table: "occasions",
                column: "CoordinatorUserUserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_occasions_users_CoordinatorUserUserId",
                table: "occasions");

            migrationBuilder.DropIndex(
                name: "IX_occasions_CoordinatorUserUserId",
                table: "occasions");

            migrationBuilder.DropColumn(
                name: "CoordinatorUserUserId",
                table: "occasions");

            migrationBuilder.CreateIndex(
                name: "IX_occasions_CoordinatorId",
                table: "occasions",
                column: "CoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_occasions_users_CoordinatorId",
                table: "occasions",
                column: "CoordinatorId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
