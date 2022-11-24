﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCoresqlitetriggermigration.Migrations
{
    [DbContext(typeof(PetContext))]
    partial class PetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Pet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("ID");

                    b.ToTable("Pets");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Fluffy",
                            Type = "cat 🐱",
                            UpdatedAt = new DateTime(2022, 11, 24, 19, 21, 15, 901, DateTimeKind.Utc).AddTicks(5614)
                        },
                        new
                        {
                            ID = 2,
                            Name = "Wolfie",
                            Type = "dog 🐕",
                            UpdatedAt = new DateTime(2022, 11, 24, 19, 21, 15, 901, DateTimeKind.Utc).AddTicks(5620)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
