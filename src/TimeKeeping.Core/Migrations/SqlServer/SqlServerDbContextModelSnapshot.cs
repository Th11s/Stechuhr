﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Th11s.TimeKeeping.Data;

#nullable disable

namespace Th11s.TimeKeeping.Migrations.SqlServer
{
    [DbContext(typeof(SqlServerDbContext))]
    partial class SqlServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Abteilung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Abteilung");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Arbeitsplatz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AbteilungsId")
                        .HasColumnType("int");

                    b.Property<string>("ArbeitnehmerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan>("Standarddienstzeit")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("AbteilungsId");

                    b.HasIndex("ArbeitnehmerId");

                    b.ToTable("Arbeitsplaetze");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Tagesdienstzeit", b =>
                {
                    b.Property<Guid>("ArbeitsplatzId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("Datum")
                        .HasColumnType("date");

                    b.Property<TimeSpan?>("Arbeitszeit")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("Arbeitszeitgutschrift")
                        .HasColumnType("time");

                    b.Property<bool>("HatPausezeitminimum")
                        .HasColumnType("bit");

                    b.Property<bool>("HatProbleme")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<TimeSpan?>("Pausenzeit")
                        .HasColumnType("time");

                    b.Property<string>("Probleme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Sollarbeitszeit")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("Zeitsaldo")
                        .HasColumnType("time");

                    b.HasKey("ArbeitsplatzId", "Datum");

                    b.ToTable("Tagesdienstzeiten");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Zeiterfassung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArbeitsplatzId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("Datum")
                        .HasColumnType("date");

                    b.Property<bool>("HatAnpassungen")
                        .HasColumnType("bit");

                    b.Property<bool>("IstEntfernt")
                        .HasColumnType("bit");

                    b.Property<bool>("IstNachbuchung")
                        .HasColumnType("bit");

                    b.Property<bool>("IstVorausbuchung")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Stempeltyp")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Zeitstempel")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("ArbeitsplatzId");

                    b.HasIndex("Datum");

                    b.ToTable("Zeiterfassung");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Th11s.TimeKeeping.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Abteilung", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.Abteilung", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Arbeitsplatz", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.Abteilung", "Abteilung")
                        .WithMany("Arbeitsplaetze")
                        .HasForeignKey("AbteilungsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Th11s.TimeKeeping.Data.Entities.User", "Arbeitnehmer")
                        .WithMany()
                        .HasForeignKey("ArbeitnehmerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Abteilung");

                    b.Navigation("Arbeitnehmer");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Tagesdienstzeit", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.Arbeitsplatz", "Arbeitsplatz")
                        .WithMany()
                        .HasForeignKey("ArbeitsplatzId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Arbeitsplatz");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Zeiterfassung", b =>
                {
                    b.HasOne("Th11s.TimeKeeping.Data.Entities.Arbeitsplatz", "Arbeitsplatz")
                        .WithMany()
                        .HasForeignKey("ArbeitsplatzId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Th11s.TimeKeeping.Data.Entities.Nachverfolgungseintrag", "Nachverfolgung", b1 =>
                        {
                            b1.Property<Guid>("ZeiterfassungId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<DateTimeOffset>("At")
                                .HasColumnType("datetimeoffset");

                            b1.Property<string>("Command")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Data")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<string>("UserName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ZeiterfassungId", "Id");

                            b1.ToTable("Zeiterfassung");

                            b1.ToJson("Nachverfolgung");

                            b1.WithOwner()
                                .HasForeignKey("ZeiterfassungId");
                        });

                    b.Navigation("Arbeitsplatz");

                    b.Navigation("Nachverfolgung");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Abteilung", b =>
                {
                    b.Navigation("Arbeitsplaetze");
                });
#pragma warning restore 612, 618
        }
    }
}
