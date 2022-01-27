using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetLearn.QueryFilterDemo.Migrations
{
    public partial class AddSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Tenants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Customers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Customers");
        }
    }
}
