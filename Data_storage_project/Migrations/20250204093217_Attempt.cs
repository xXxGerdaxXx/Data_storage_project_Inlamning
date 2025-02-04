using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_storage_project_library.Migrations
{
    /// <inheritdoc />
    public partial class Attempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Currencies_CurrencyEntityId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CurrencyEntityId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CurrencyEntityId",
                table: "Services");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyEntityId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CurrencyEntityId",
                table: "Services",
                column: "CurrencyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Currencies_CurrencyEntityId",
                table: "Services",
                column: "CurrencyEntityId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }
    }
}
