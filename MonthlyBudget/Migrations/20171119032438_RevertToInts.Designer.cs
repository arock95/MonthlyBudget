﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MonthlyBudget.Data;

namespace MonthlyBudget.Migrations
{
    [DbContext(typeof(BudgetDbContext))]
    [Migration("20171119032438_RevertToInts")]
    partial class RevertToInts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MonthlyBudget.Models.BudgetItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("BudgetMonth");

                    b.Property<int>("BudgetYear");

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("BudgetItems");
                });

            modelBuilder.Entity("MonthlyBudget.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MonthlyBudget.Models.Purchase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<int>("Cost");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<string>("PurchaseName")
                        .IsRequired();

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Purchases");
                });
        }
    }
}
