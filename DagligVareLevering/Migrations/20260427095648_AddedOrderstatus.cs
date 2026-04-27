using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagligVareLevering.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WantsOfferEmails",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WantsOfferEmails",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
