using System;
using Agrigate.Domain.Entities.Rules;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Agrigate.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelemetryRule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Operator = table.Column<int>(type: "integer", nullable: false),
                    Timespan = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryRule_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryRuleActionDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryRuleActionDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryRuleConditionDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryRuleConditionDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryRuleAction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryRuleAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryRuleAction_TelemetryRule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "TelemetryRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryRuleCondition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryRuleCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryRuleCondition_TelemetryRule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "TelemetryRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryRule_DeviceId",
                table: "TelemetryRule",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryRuleAction_RuleId",
                table: "TelemetryRuleAction",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryRuleCondition_RuleId",
                table: "TelemetryRuleCondition",
                column: "RuleId");

            // Default Inserts
            var upperLimitDefinition = new List<object>
            {
                new
                {
                    label = "Upper Limit",
                    key = "value",
                    type = "double"
                }
            };

            var lowerLimitDefinition = new List<object>
            {
                new
                {
                    label = "Lower Limit",
                    key = "value",
                    type = "double"
                }
            };

            var rangeDefinition = new List<object>
            {
                new 
                {
                    label = "Upper Limit",
                    key = "upperLimit",
                    type = "double"
                },
                new
                {
                    label = "Lower Limit",
                    key = "lowerLimit",
                    type = "double"
                }
            };

            var notificationDefinition = new List<object>
            {
                new
                {
                    label = "Type",
                    key = "channel",
                    type = "select",
                    options = new List<object>
                    {
                        new 
                        {
                            id = NotificationChannel.MQTT,
                            label = "MQTT"
                        },
                        new 
                        {
                            id = NotificationChannel.Email,
                            label = "Email"
                        },
                        new 
                        {
                            id = NotificationChannel.SMS,
                            label = "SMS"
                        }
                    }
                },
                new
                {
                    dependsOn = "channel",
                    label = new Dictionary<int, string>
                    {
                        { 0, "Topic" },
                        { 1, "Email Address" },
                        { 2, "Phone Number" }
                    },
                    key = "address",
                    type = "string"
                },
                new
                {
                    label = "Content",
                    key = "content",
                    type = "string"
                }
            };

            var now = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var upperLimitJson = JsonConvert.SerializeObject(upperLimitDefinition);
            var lowerLimitJson = JsonConvert.SerializeObject(lowerLimitDefinition);
            var rangeJson = JsonConvert.SerializeObject(rangeDefinition);
            var notificationJson = JsonConvert.SerializeObject(notificationDefinition);

            migrationBuilder.Sql(@$"
                INSERT INTO ""TelemetryRuleConditionDefinition"" (""Type"", ""Definition"", ""Created"", ""Modified"", ""IsDeleted"")
                VALUES
                ({(int)RuleCondition.UpperLimit}, '{upperLimitJson}', '{now}', '{now}', false),
                ({(int)RuleCondition.LowerLimit}, '{lowerLimitJson}', '{now}', '{now}', false),
                ({(int)RuleCondition.Range}, '{rangeJson}', '{now}', '{now}', false);
            ");

            migrationBuilder.Sql(@$"
                INSERT INTO ""TelemetryRuleActionDefinition"" (""Type"", ""Definition"", ""Created"", ""Modified"", ""IsDeleted"")
                VALUES
                ({(int)RuleAction.Notification}, '{notificationJson}', '{now}', '{now}', false);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelemetryRuleAction");

            migrationBuilder.DropTable(
                name: "TelemetryRuleActionDefinition");

            migrationBuilder.DropTable(
                name: "TelemetryRuleCondition");

            migrationBuilder.DropTable(
                name: "TelemetryRuleConditionDefinition");

            migrationBuilder.DropTable(
                name: "TelemetryRule");
        }
    }
}
