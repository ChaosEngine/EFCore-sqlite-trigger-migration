    using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoresqlitetriggermigration.Migrations
{
    /// <inheritdoc />
    public partial class TimestampAndTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pets",
                type: "TIMESTAMP",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

			//overcomming problem/bug:
			//Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 1: 'Cannot add a column with non-constant default'
			//https://stackoverflow.com/a/61967099/4429828
            //OMG, I am loosing hope in sqlite
			migrationBuilder.Sql(
                $"""
                    UPDATE "Pets" SET
                    "UpdatedAt" = datetime(CURRENT_TIMESTAMP, 'UTC')
                    WHERE "UpdatedAt" IS NULL
                """
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pets");
        }
    }
}
