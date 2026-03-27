using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankBills.Migrations
{
    /// <inheritdoc />
    public partial class RefatoracaoBancoEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransaction_Bank_BankId",
                table: "BankTransaction");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropIndex(
                name: "IX_BankTransaction_BankId",
                table: "BankTransaction");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "BankTransaction");

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "BankTransaction",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bank",
                table: "BankTransaction");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "BankTransaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransaction_BankId",
                table: "BankTransaction",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransaction_Bank_BankId",
                table: "BankTransaction",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
