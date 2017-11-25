using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonthlyBudget.Migrations
{
    public partial class Budget_date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BudgetItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetDate",
                table: "BudgetItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetDate",
                table: "BudgetItems");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "BudgetItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "BudgetItems",
                nullable: false,
                defaultValue: 0);
        }
    }
}
