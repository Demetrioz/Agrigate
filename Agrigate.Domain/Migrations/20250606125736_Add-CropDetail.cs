using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Agrigate.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCropDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CropDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Crop = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cultivar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Dtm = table.Column<int>(type: "integer", nullable: false),
                    StackingDays = table.Column<int>(type: "integer", nullable: true),
                    BlackoutDays = table.Column<int>(type: "integer", nullable: true),
                    NurseryDays = table.Column<int>(type: "integer", nullable: true),
                    SoakHours = table.Column<int>(type: "integer", nullable: true),
                    PlantQuantity = table.Column<double>(type: "double precision", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CropDetails");
        }
    }
}
