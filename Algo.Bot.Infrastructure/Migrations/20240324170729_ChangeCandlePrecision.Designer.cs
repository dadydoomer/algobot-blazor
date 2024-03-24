﻿// <auto-generated />
using System;
using Algo.Bot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Algo.Bot.Infrastructure.Migrations
{
    [DbContext(typeof(AlgobotDbContext))]
    [Migration("20240324170729_ChangeCandlePrecision")]
    partial class ChangeCandlePrecision
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Algo.Bot.Infrastructure.Database.CandleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Close")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("CoinId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("High")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<decimal>("Low")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("Open")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Candles");
                });
#pragma warning restore 612, 618
        }
    }
}
