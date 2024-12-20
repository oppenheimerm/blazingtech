﻿// <auto-generated />
using System;
using BT.Authentication.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BT.Authentication.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241110183605_BTUSER_AccountLockedOut")]
    partial class BTUSER_AccountLockedOut
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BT.Shared.Domain.BTUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AccountLockedOut")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LasttName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Mobile")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("BT.Shared.Domain.Role", b =>
                {
                    b.Property<string>("RoleCode")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleCode");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BT.Shared.Domain.UserRole", b =>
                {
                    b.Property<Guid?>("BTUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleCode")
                        .HasColumnType("nvarchar(4)");

                    b.Property<DateTime?>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("BTUserId", "RoleCode");

                    b.HasIndex("RoleCode");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("BT.Shared.Domain.UserRole", b =>
                {
                    b.HasOne("BT.Shared.Domain.BTUser", "BTUser")
                        .WithMany()
                        .HasForeignKey("BTUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BT.Shared.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BTUser");

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
