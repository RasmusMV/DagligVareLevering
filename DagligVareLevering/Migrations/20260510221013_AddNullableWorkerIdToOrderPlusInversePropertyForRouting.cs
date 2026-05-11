using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagligVareLevering.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableWorkerIdToOrderPlusInversePropertyForRouting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Orders");
        }
    }
}
