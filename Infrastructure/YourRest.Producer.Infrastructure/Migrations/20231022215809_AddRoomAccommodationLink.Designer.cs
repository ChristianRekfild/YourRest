﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YourRest.Producer.Infrastructure.DbContexts;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    [DbContext(typeof(SharedDbContext))]
    [Migration("20231022215809_AddRoomAccommodationLink")]
    partial class AddRoomAccommodationLink
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingId")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoomType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("SquareInMeter")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.RoomFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomFacilities");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.RoomType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KeyCloakId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.UserAccommodation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "AccommodationId");

                    b.HasIndex("AccommodationId");

                    b.ToTable("UserAccommodations");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Accommodation", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Address", "Address")
                        .WithOne()
                        .HasForeignKey("YourRest.Producer.Infrastructure.Entities.Accommodation", "AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Address", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.City", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Booking", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Accommodation", "Accommodation")
                        .WithMany()
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.City", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Region", "Region")
                        .WithMany("Cities")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Region", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Review", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Producer.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("YourRest.Domain.ValueObjects.Reviews.RatingVO", "Rating", b1 =>
                        {
                            b1.Property<int>("ReviewId")
                                .HasColumnType("integer");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Rating");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Reviews");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.Navigation("Booking");

                    b.Navigation("Rating")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Room", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Accommodation", "Accommodation")
                        .WithMany("Rooms")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.RoomFacility", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Room", "Room")
                        .WithMany("RoomFacilities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.UserAccommodation", b =>
                {
                    b.HasOne("YourRest.Producer.Infrastructure.Entities.Accommodation", "Accommodation")
                        .WithMany("UserAccommodations")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Producer.Infrastructure.Entities.User", "User")
                        .WithMany("UserAccommodations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Accommodation", b =>
                {
                    b.Navigation("Rooms");

                    b.Navigation("UserAccommodations");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.City", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Country", b =>
                {
                    b.Navigation("Regions");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Region", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.Room", b =>
                {
                    b.Navigation("RoomFacilities");
                });

            modelBuilder.Entity("YourRest.Producer.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("UserAccommodations");
                });
#pragma warning restore 612, 618
        }
    }
}
