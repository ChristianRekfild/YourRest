﻿// <auto-generated />
using System;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HotelManagementWebApi.Infrastructure.Migrations
{
    [DbContext(typeof(HotelManagementDbContext))]
    partial class HotelManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HotelManagementWebApi.Domain.Entities.Booking.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("HotelManagementWebApi.Domain.Entities.Review.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("HotelManagementWebApi.Domain.Entities.Booking.Booking", b =>
                {
                    b.OwnsOne("HotelManagementWebApi.Domain.ValueObjects.Booking.BookingDate", "EndDate", b1 =>
                        {
                            b1.Property<int>("BookingId")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.HasKey("BookingId");

                            b1.ToTable("Bookings");

                            b1.WithOwner()
                                .HasForeignKey("BookingId");
                        });

                    b.OwnsOne("HotelManagementWebApi.Domain.ValueObjects.Booking.BookingDate", "StartDate", b1 =>
                        {
                            b1.Property<int>("BookingId")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("BookingId");

                            b1.ToTable("Bookings");

                            b1.WithOwner()
                                .HasForeignKey("BookingId");
                        });

                    b.OwnsOne("HotelManagementWebApi.Domain.ValueObjects.Booking.BookingStatus", "Status", b1 =>
                        {
                            b1.Property<int>("BookingId")
                                .HasColumnType("integer");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Status");

                            b1.HasKey("BookingId");

                            b1.ToTable("Bookings");

                            b1.WithOwner()
                                .HasForeignKey("BookingId");
                        });

                    b.Navigation("EndDate")
                        .IsRequired();

                    b.Navigation("StartDate")
                        .IsRequired();

                    b.Navigation("Status")
                        .IsRequired();
                });

            modelBuilder.Entity("HotelManagementWebApi.Domain.Entities.Review.Review", b =>
                {
                    b.HasOne("HotelManagementWebApi.Domain.Entities.Booking.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("HotelManagementWebApi.Domain.ValueObjects.Review.Comment", "Comment", b1 =>
                        {
                            b1.Property<int>("ReviewId")
                                .HasColumnType("integer");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Comment");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Reviews");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.OwnsOne("HotelManagementWebApi.Domain.ValueObjects.Review.Rating", "Rating", b1 =>
                        {
                            b1.Property<int>("ReviewId")
                                .HasColumnType("integer");

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("Rating");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Reviews");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.Navigation("Booking");

                    b.Navigation("Comment")
                        .IsRequired();

                    b.Navigation("Rating")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
