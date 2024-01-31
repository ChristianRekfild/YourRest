﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YourRest.Infrastructure.Core.DbContexts;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    [DbContext(typeof(SharedDbContext))]
    partial class SharedDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookingRoom", b =>
                {
                    b.Property<int>("RoomsId")
                        .HasColumnType("integer");

                    b.Property<int>("bookingsId")
                        .HasColumnType("integer");

                    b.HasKey("RoomsId", "bookingsId");

                    b.HasIndex("bookingsId");

                    b.ToTable("BookingRoom");
                });

            modelBuilder.Entity("RoomRoomFacility", b =>
                {
                    b.Property<int>("RoomFacilitiesId")
                        .HasColumnType("integer");

                    b.Property<int>("RoomsId")
                        .HasColumnType("integer");

                    b.HasKey("RoomFacilitiesId", "RoomsId");

                    b.HasIndex("RoomsId");

                    b.ToTable("RoomRoomFacility");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationTypeId")
                        .HasColumnType("integer");

                    b.Property<int?>("AddressId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationTypeId");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccommodationFacility");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationFacilityLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationFacilityId")
                        .HasColumnType("integer");

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationFacilityId");

                    b.HasIndex("AccommodationId");

                    b.ToTable("AccommodationFacilities");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.ToTable("AccommodationPhotos");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationStarRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<int>("Stars")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId")
                        .IsUnique();

                    b.ToTable("AccommodationStarRatings");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccommodationTypes");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Address", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.AgeRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AgeFrom")
                        .HasColumnType("integer");

                    b.Property<int>("AgeTo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AgeRanges");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AdultNumber")
                        .HasColumnType("integer");

                    b.Property<int>("ChildrenNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("SystemId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.CityPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("CityPhotos");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Country", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int?>("ExternalId")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PassportNumber")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("SystemId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Region", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.Review", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.Room", b =>
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

                    b.Property<int>("RoomTypeId")
                        .HasColumnType("integer");

                    b.Property<double>("SquareInMeter")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.RoomFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RoomFacilities");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.RoomPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomPhotos");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.RoomType", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.User", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.UserAccommodation", b =>
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

            modelBuilder.Entity("YourRest.Domain.Entities.UserPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserPhotos");
                });

            modelBuilder.Entity("BookingRoom", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.Booking", null)
                        .WithMany()
                        .HasForeignKey("bookingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoomRoomFacility", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.RoomFacility", null)
                        .WithMany()
                        .HasForeignKey("RoomFacilitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Accommodation", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.AccommodationType", "AccommodationType")
                        .WithMany()
                        .HasForeignKey("AccommodationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.Address", "Address")
                        .WithOne()
                        .HasForeignKey("YourRest.Domain.Entities.Accommodation", "AddressId");

                    b.Navigation("AccommodationType");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationFacilityLink", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.AccommodationFacility", "AccommodationFacility")
                        .WithMany("AccommodationFacilities")
                        .HasForeignKey("AccommodationFacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.Accommodation", "Accommodation")
                        .WithMany("AccommodationFacilities")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("AccommodationFacility");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationPhoto", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Accommodation", "Accommodation")
                        .WithMany()
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationStarRating", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Accommodation", "Accommodation")
                        .WithOne("StarRating")
                        .HasForeignKey("YourRest.Domain.Entities.AccommodationStarRating", "AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Address", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.City", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Booking", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.City", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Region", "Region")
                        .WithMany("Cities")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.CityPhoto", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.City", "City")
                        .WithMany("CityPhotos")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Region", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Review", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.User", "User")
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

            modelBuilder.Entity("YourRest.Domain.Entities.Room", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Accommodation", "Accommodation")
                        .WithMany("Rooms")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.RoomType", "RoomType")
                        .WithMany()
                        .HasForeignKey("RoomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.RoomPhoto", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.UserAccommodation", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.Accommodation", "Accommodation")
                        .WithMany("UserAccommodations")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YourRest.Domain.Entities.User", "User")
                        .WithMany("UserAccommodations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.UserPhoto", b =>
                {
                    b.HasOne("YourRest.Domain.Entities.User", "User")
                        .WithMany("UserPhotos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Accommodation", b =>
                {
                    b.Navigation("AccommodationFacilities");

                    b.Navigation("Rooms");

                    b.Navigation("StarRating");

                    b.Navigation("UserAccommodations");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.AccommodationFacility", b =>
                {
                    b.Navigation("AccommodationFacilities");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.City", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("CityPhotos");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Country", b =>
                {
                    b.Navigation("Regions");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.Region", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("YourRest.Domain.Entities.User", b =>
                {
                    b.Navigation("UserAccommodations");

                    b.Navigation("UserPhotos");
                });
#pragma warning restore 612, 618
        }
    }
}
