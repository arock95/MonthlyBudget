using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonthlyBudget.Migrations
{
    public partial class purchaseupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "BudgetItems",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Purchases");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "BudgetItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
