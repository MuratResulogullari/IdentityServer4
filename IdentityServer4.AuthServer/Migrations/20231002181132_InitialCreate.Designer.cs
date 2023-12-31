﻿// <auto-generated />
using IdentityServer4.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IdentityServer4.AuthServer.Migrations
{
    [DbContext(typeof(CustomDbContext))]
    [Migration("20231002181132_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IdentityServer4.AuthServer.Models.CustomUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CustomUsers");

                    b.HasData(
                        new
                        {
                            Id = "3a3aa9ee-d20f-4317-bbf1-b798c3fb0fa8",
                            City = "İzmir",
                            Email = "murat.resulogullari1@gmail.com",
                            Name = "Murat",
                            Password = "test.123",
                            Surname = "Resuloğulları",
                            Username = "murat.resulogullari"
                        },
                        new
                        {
                            Id = "e68b1917-cb76-49a8-b4c2-621763f77f1d",
                            City = "Konya",
                            Email = "ali.veli@gmail.com",
                            Name = "Ali",
                            Password = "test.123",
                            Surname = "Veli",
                            Username = "ali.veli"
                        },
                        new
                        {
                            Id = "25c946d0-eb7a-4279-a24b-7fb0a094d2cd",
                            City = "Amasya",
                            Email = "mehmet.hasan@gmail.com",
                            Name = "Mehmet",
                            Password = "test.123",
                            Surname = "Hasan",
                            Username = "mehmet.hasan"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
