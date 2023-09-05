﻿// <auto-generated />
using Lina.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FarDragi.Hanabi.Migrations
{
    [DbContext(typeof(LinaDbContext))]
    [Migration("20230905141153_AddCandyTable")]
    partial class AddCandyTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FarDragi.Hanabi.Models.CandyEntity", b =>
                {
                    b.Property<ulong>("Id")
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Candies", (string)null);
                });

            modelBuilder.Entity("FarDragi.Hanabi.Models.InviteEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("Uses")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Invites", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
