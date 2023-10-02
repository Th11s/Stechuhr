﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;

#nullable disable

namespace Th11s.TimeKeeping.Migrations.PostgreSQL
{
    [DbContext(typeof(NpgsqlDbContext))]
    partial class NpgsqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Abteilung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Abteilung");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Arbeitsplatz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AbteilungsId")
                        .HasColumnType("integer");

                    b.Property<string>("ArbeitnehmerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan>("Standarddienstzeit")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("AbteilungsId");

                    b.HasIndex("ArbeitnehmerId");

                    b.ToTable("Arbeitsplaetze");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Tagesdienstzeit", b =>
                {
                    b.Property<Guid>("ArbeitsplatzId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Datum")
                        .HasColumnType("date");

                    b.Property<TimeSpan?>("Arbeitszeit")
                        .HasColumnType("interval");

                    b.Property<TimeSpan?>("Arbeitszeitgutschrift")
                        .HasColumnType("interval");

                    b.Property<bool>("HatPausezeitminimum")
                        .HasColumnType("boolean");

                    b.Property<bool>("HatProbleme")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan?>("Pausenzeit")
                        .HasColumnType("interval");

                    b.Property<string[]>("Probleme")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<TimeSpan>("Sollarbeitszeit")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("Zeitsaldo")
                        .HasColumnType("interval");

                    b.HasKey("ArbeitsplatzId", "Datum");

                    b.ToTable("Tagesdienstzeiten");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Zeiterfassung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArbeitsplatzId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Datum")
                        .HasColumnType("date");

                    b.Property<bool>("HatAnpassungen")
                        .HasColumnType("boolean");

                    b.Property<bool>("IstEntfernt")
                        .HasColumnType("boolean");

                    b.Property<bool>("IstNachbuchung")
                        .HasColumnType("boolean");

                    b.Property<bool>("IstVorausbuchung")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Nachverfolgungseintrag[]>("Nachverfolgung")
                        .HasColumnType("jsonb");

                    b.Property<int>("Stempeltyp")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Zeitstempel")
                        .HasColumnType("timestamp with time zone");

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
                        .WithMany("Arbeitsplatz")
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

                    b.Navigation("Arbeitsplatz");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.Abteilung", b =>
                {
                    b.Navigation("Arbeitsplaetze");
                });

            modelBuilder.Entity("Th11s.TimeKeeping.Data.Entities.User", b =>
                {
                    b.Navigation("Arbeitsplatz");
                });
#pragma warning restore 612, 618
        }
    }
}
