﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestTube.Data;

#nullable disable

namespace TestTube.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250520204351_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestTube.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ScientistId")
                        .HasColumnType("integer");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScientistId");

                    b.ToTable("Equipment");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Manufacturer = "Zeiss",
                            Name = "Microscope",
                            Price = 15000.00m,
                            PurchaseDate = new DateTime(2021, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc),
                            ScientistId = 1,
                            SerialNumber = "MS-12345"
                        },
                        new
                        {
                            Id = 2,
                            Manufacturer = "Thermo Fisher",
                            Name = "Centrifuge",
                            Price = 8500.00m,
                            PurchaseDate = new DateTime(2022, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc),
                            ScientistId = 1,
                            SerialNumber = "CF-67890"
                        },
                        new
                        {
                            Id = 3,
                            Manufacturer = "Agilent",
                            Name = "Spectrometer",
                            Price = 22000.00m,
                            PurchaseDate = new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc),
                            ScientistId = 2,
                            SerialNumber = "SP-24680"
                        },
                        new
                        {
                            Id = 4,
                            Manufacturer = "Bio-Rad",
                            Name = "PCR Machine",
                            Price = 12000.00m,
                            PurchaseDate = new DateTime(2023, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                            ScientistId = 3,
                            SerialNumber = "PCR-13579"
                        });
                });

            modelBuilder.Entity("TestTube.Models.Scientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Scientists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = "Physics",
                            Email = "marie.curie@testtube.com",
                            HireDate = new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Marie Curie"
                        },
                        new
                        {
                            Id = 2,
                            Department = "Physics",
                            Email = "albert.einstein@testtube.com",
                            HireDate = new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Albert Einstein"
                        },
                        new
                        {
                            Id = 3,
                            Department = "Chemistry",
                            Email = "rosalind.franklin@testtube.com",
                            HireDate = new DateTime(2022, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Rosalind Franklin"
                        });
                });

            modelBuilder.Entity("TestTube.Models.Equipment", b =>
                {
                    b.HasOne("TestTube.Models.Scientist", "Scientist")
                        .WithMany("Equipment")
                        .HasForeignKey("ScientistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("TestTube.Models.Scientist", b =>
                {
                    b.Navigation("Equipment");
                });
#pragma warning restore 612, 618
        }
    }
}
