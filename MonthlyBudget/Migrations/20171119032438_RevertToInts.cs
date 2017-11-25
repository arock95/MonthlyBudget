using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonthlyBudget.Migrations
{
    public partial class RevertToInts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetDate",
                table: "BudgetItems");

            migrationBuilder.AddColumn<int>(
                name: "BudgetMonth",
                table: "BudgetItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BudgetYear",
                table: "BudgetItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetMonth",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "BudgetYear",
                table: "BudgetItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetDate",
                table: "BudgetItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
