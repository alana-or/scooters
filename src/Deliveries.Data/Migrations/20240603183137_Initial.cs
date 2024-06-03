﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deliveries.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_person", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deliveries_rental",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    scooterid = table.Column<Guid>(name: "scooter-id", type: "uuid", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    licenceplate = table.Column<string>(name: "licence-plate", type: "text", nullable: false),
                    DeliveryPersonId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveries_rental", x => x.id);
                    table.ForeignKey(
                        name: "FK_deliveries_rental_delivery_person_DeliveryPersonId",
                        column: x => x.DeliveryPersonId,
                        principalTable: "delivery_person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_rental_DeliveryPersonId",
                table: "deliveries_rental",
                column: "DeliveryPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deliveries_rental");

            migrationBuilder.DropTable(
                name: "delivery_person");
        }
    }
}
