using Microsoft.EntityFrameworkCore.Migrations;

namespace belt.Migrations
{
    public partial class FourMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_occasions_users_CoordinatorUserUserId",
                table: "occasions");

            migrationBuilder.AlterColumn<int>(
                name: "CoordinatorUserUserId",
                table: "occasions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CoordinatorId",
                table: "occasions",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.DropColumn(
                name: "CoordinatorId",
                table: "occasions");

            migrationBuilder.AlterColumn<int>(
                name: "CoordinatorUserUserId",
                table: "occasions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_occasions_users_CoordinatorUserUserId",
                table: "occasions",
                column: "CoordinatorUserUserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
