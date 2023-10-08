using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Th11s.TimeKeeping.Data.Entities;

#nullable disable

namespace Th11s.TimeKeeping.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abteilung",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abteilung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abteilung_Abteilung_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Abteilung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arbeitsplaetze",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArbeitnehmerId = table.Column<string>(type: "text", nullable: false),
                    AbteilungsId = table.Column<int>(type: "integer", nullable: false),
                    Standarddienstzeit = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbeitsplaetze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arbeitsplaetze_Abteilung_AbteilungsId",
                        column: x => x.AbteilungsId,
                        principalTable: "Abteilung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Arbeitsplaetze_AspNetUsers_ArbeitnehmerId",
                        column: x => x.ArbeitnehmerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tagesdienstzeiten",
                columns: table => new
                {
                    ArbeitsplatzId = table.Column<Guid>(type: "uuid", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Arbeitszeit = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Pausenzeit = table.Column<TimeSpan>(type: "interval", nullable: true),
                    HatPausezeitminimum = table.Column<bool>(type: "boolean", nullable: false),
                    Sollarbeitszeit = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Arbeitszeitgutschrift = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Zeitsaldo = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Probleme = table.Column<string[]>(type: "text[]", nullable: false),
                    HatProbleme = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagesdienstzeiten", x => new { x.ArbeitsplatzId, x.Datum });
                    table.ForeignKey(
                        name: "FK_Tagesdienstzeiten_Arbeitsplaetze_ArbeitsplatzId",
                        column: x => x.ArbeitsplatzId,
                        principalTable: "Arbeitsplaetze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zeiterfassung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ArbeitsplatzId = table.Column<Guid>(type: "uuid", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    Zeitstempel = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Stempeltyp = table.Column<int>(type: "integer", nullable: false),
                    Nachverfolgung = table.Column<Zeiterfassungsaudit[]>(type: "jsonb", nullable: true),
                    IstNachbuchung = table.Column<bool>(type: "boolean", nullable: false),
                    IstVorausbuchung = table.Column<bool>(type: "boolean", nullable: false),
                    HatAnpassungen = table.Column<bool>(type: "boolean", nullable: false),
                    IstEntfernt = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zeiterfassung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zeiterfassung_Arbeitsplaetze_ArbeitsplatzId",
                        column: x => x.ArbeitsplatzId,
                        principalTable: "Arbeitsplaetze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abteilung_ParentId",
                table: "Abteilung",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Arbeitsplaetze_AbteilungsId",
                table: "Arbeitsplaetze",
                column: "AbteilungsId");

            migrationBuilder.CreateIndex(
                name: "IX_Arbeitsplaetze_ArbeitnehmerId",
                table: "Arbeitsplaetze",
                column: "ArbeitnehmerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zeiterfassung_ArbeitsplatzId",
                table: "Zeiterfassung",
                column: "ArbeitsplatzId");

            migrationBuilder.CreateIndex(
                name: "IX_Zeiterfassung_Datum",
                table: "Zeiterfassung",
                column: "Datum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Tagesdienstzeiten");

            migrationBuilder.DropTable(
                name: "Zeiterfassung");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Arbeitsplaetze");

            migrationBuilder.DropTable(
                name: "Abteilung");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
